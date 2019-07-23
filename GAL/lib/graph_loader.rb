require_relative '../process_logger'
require 'nokogiri'
require 'ruby-graphviz'
require_relative 'graph'
require_relative 'visual_graph'
require_relative 'dfs'


# Class to load graph from various formats. Actually implemented is Graphviz formats. Future is OSM format.
class GraphLoader
	attr_reader :highway_attributes

	# Create an instance, save +filename+ and preset highway attributes
	def initialize(filename, highway_attributes)
		@filename = filename
		@highway_attributes = highway_attributes 
		@DFS_solver
	end

	# Load graph from Graphviz file which was previously constructed from this application, i.e. contains necessary data.
	# File needs to contain 
	# => 1) For node its 'id', 'pos' (containing its re-computed position on graphviz space) and 'comment' containig string with comma separated lat and lon
	# => 2) Edge (instead of source and target nodes) might contains info about 'speed' and 'one_way'
	# => 3) Generaly, graph contains parametr 'bb' containing array with bounds of map as minlon, minlat, maxlon, maxlat
	#
	# @return [+Graph+, +VisualGraph+]
	def load_graph_viz()
		ProcessLogger.log("Loading graph from GraphViz file #{@filename}.")
		gv = GraphViz.parse(@filename)

		# aux data structures
		hash_of_vertices = {}
		list_of_edges = []
		hash_of_visual_vertices = {}
		list_of_visual_edges = []		

		# process vertices
		ProcessLogger.log("Processing vertices")
		gv.node_count.times { |node_index|
			node = gv.get_node_at_index(node_index)
			vid = node.id

			v = Vertex.new(vid) unless hash_of_vertices.has_key?(vid)
			ProcessLogger.log("\t Vertex #{vid} loaded")
			hash_of_vertices[vid] = v

			geo_pos = node["comment"].to_s.delete("\"").split(",")
			pos = node["pos"].to_s.delete("\"").split(",")	
			hash_of_visual_vertices[vid] = VisualVertex.new(vid, v, geo_pos[0], geo_pos[1], pos[1], pos[0])
			ProcessLogger.log("\t Visual vertex #{vid} in ")
		}

		# process edges
		gv.edge_count.times { |edge_index|
			link = gv.get_edge_at_index(edge_index)
			vid_from = link.node_one.delete("\"")
			vid_to = link.node_two.delete("\"")
			speed = 50
			one_way = false
			link.each_attribute { |k,v|
				speed = v if k == "speed"
				one_way = true if k == "oneway"
			}
			e = Edge.new(vid_from, vid_to, speed, one_way)
			list_of_edges << e
			list_of_visual_edges << VisualEdge.new(e, hash_of_visual_vertices[vid_from], hash_of_visual_vertices[vid_to])
		}

		# Create Graph instance
		g = Graph.new(hash_of_vertices, list_of_edges)

		# Create VisualGraph instance
		bounds = {}
		bounds[:minlon], bounds[:minlat], bounds[:maxlon], bounds[:maxlat] = gv["bb"].to_s.delete("\"").split(",")
		vg = VisualGraph.new(g, hash_of_visual_vertices, list_of_visual_edges, bounds)

		return g, vg
	end

	# Method to load graph from OSM file and create +Graph+ and +VisualGraph+ instances from +self.filename+
	#
	# @return [+Graph+, +VisualGraph+]
	def load_graph()

		ProcessLogger.log("Loading graph from .OSM or .XML file #{@filename}.")


		# aux data structures - change from the original ones
		hash_of_vertices = {}
		list_of_edges = []
		hash_of_visual_vertices = {}
		list_of_visual_edges = []

		# stringifies array of attributes into a single filter string
		ways_filter_attributes = stringify_highway_attributes

		ProcessLogger.log("Processing ways")
                                              
		map_xml_feed = Nokogiri::XML(open(@filename))

		graph_bounds = map_xml_feed.xpath("//bounds")

		map_xml_feed.xpath("//way[tag[contains(@k,'highway') and contains('#{ways_filter_attributes}', @v)]]").each { |way_item|

			way_id = way_item.xpath('./@id')

			ProcessLogger.log("Way #{way_id} loaded")
			speed = way_item.xpath("./tag[@k='maxspeed']/@v").to_s == "" ? 50 : way_item.xpath("./tag[@k='maxspeed']/@v").to_s.to_i
			

			one_way = way_item.xpath("./tag[@k='oneway']/@v").to_s.eql?('yes') ? true : false
			nodes = way_item.xpath( './nd/@ref' )

			nodes.each_with_index { |node_id, i|

				(
					# Set indexing - avoid missing .to_s casts in hash calling
					current_node_id = node_id.to_s
					next_node_id = nodes[i+1].to_s

					hash_of_vertices[current_node_id] = Vertex.new(current_node_id) unless hash_of_vertices.has_key?(current_node_id)
					hash_of_vertices[next_node_id] = Vertex.new(next_node_id) unless hash_of_vertices.has_key?(next_node_id)
					
					ProcessLogger.log("\t Vertex #{current_node_id} loaded")
					ProcessLogger.log("\t Vertex #{next_node_id} loaded")
					
					current_edge = Edge.new(hash_of_vertices[current_node_id], hash_of_vertices[next_node_id], speed, one_way)

					# Include neighbouring edges for Dijkstra
					hash_of_vertices[current_node_id].neighbouring_edges[next_node_id] = current_edge
					# comment unless one_way for an undirected graph search
					hash_of_vertices[next_node_id].neighbouring_edges[current_node_id] = current_edge unless one_way

					list_of_edges << current_edge

				) unless (i+1).eql? nodes.size
				
				# Include closest neighbours to iterate through in DFS
				hash_of_vertices[node_id.to_s].neighbouring_vertices << nodes[i+1].to_s unless (i+1).eql?(nodes.size)
      			hash_of_vertices[node_id.to_s].neighbouring_vertices << nodes[i-1].to_s unless (i-1) < 0
				
			}

		}
		# Instantiate DFS solver and run DFS
		dfs_solver = DFSSolver.new()

		# Filter Vertices and edges from largest component
		hash_of_vertices = dfs_solver.get_largest_component(hash_of_vertices)

		list_of_edges.select! {|edge| hash_of_vertices.include?(edge.v1.id) && hash_of_vertices.include?(edge.v2.id)}	
	
		# Extract nodes
		map_xml_feed.xpath('//node').to_a.each { |node_item|
			next if node_item.xpath('./@visible').to_s.eql? 'false'
			
			node_id = node_item.xpath('./@id').to_s
			next if hash_of_vertices[node_id].nil?

			node_lat, node_lon = [node_item.xpath( './@lat' ).to_s, node_item.xpath( './@lon' ).to_s]
			
			hash_of_visual_vertices[node_id] = VisualVertex.new(node_id, hash_of_vertices[node_id], node_lat, node_lon, node_lat, node_lon)
			ProcessLogger.log("\t Visual vertex #{node_id} in ")
		}

		# Additionaly create VisualEdge instances and calculate weight of Edges
		list_of_edges.each { |edge|
			visual_edge = VisualEdge.new(edge, hash_of_visual_vertices[edge.v1.id], hash_of_visual_vertices[edge.v2.id])
			
			# Calculate weight and real distance of each Edge
			visual_edge.calculate_edge_weight
			visual_edge.calculate_edge_physical_distance
			visual_edge.calculate_real_time

			list_of_visual_edges << visual_edge
		}
		
		# TODO - move lat and lon to VisualVertex

		#prep_lon = node_item.xpath( './@lon' ).to_s

		#prep_lat = node_item.xpath( './@lat' ).to_s

		g = Graph.new(hash_of_vertices, list_of_edges)

		# Create VisualGraph instance
		bounds = {}
		bounds[:minlon], bounds[:minlat], bounds[:maxlon], bounds[:maxlat] = [graph_bounds.xpath("@minlon").to_s, graph_bounds.xpath("@minlat").to_s, graph_bounds.xpath("@maxlon").to_s, graph_bounds.xpath("@maxlat").to_s]
		vg = VisualGraph.new(g, hash_of_visual_vertices, list_of_visual_edges, bounds)

		return g, vg

	end

	# Converts the @highway_attributes to a single string filter separated with comma
	def stringify_highway_attributes
		return @highway_attributes.join(", ")
	end

	# Parses min and max lot and lat from <bounds> tag
	#def parse_bounds xml_feed
		#graph_bounds = xml_feed.xpath("//bounds")
		#return [graph_bounds.xpath("@minlon").to_s, graph_bounds.xpath("@minlat").to_s, graph_bounds.xpath("@maxlon").to_s, graph_bounds.xpath("@maxlat").to_s]
	#end
end
