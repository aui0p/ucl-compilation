class DijkstraSolver


  def initialize()
  	
		@to_be_emphesized = []
  end
  # public method for Dijkstra call
  def get_shortest_path(graph)
  	@graph = graph
  	@starting_vertex, @destination_vertex = @graph.start_vertex.id.to_s, @graph.end_vertex.id.to_s
  	calculate_shortest_path()
  end

# the very Dijkstra algorithm
 private

	def calculate_shortest_path()
		dijkstras = {}
		visited_vertices = {}
		total_distance = 0
		total_time = 0
		# finding end-point vertices and setting base edge costs
  		@graph.visual_vertices.each do |key, vertex|

    		dijkstras[key] = {cost: Float::INFINITY, parent_vertex: nil}
    	end

    	# UNCOMMENT IN CASE OF NOT KNOWN COORINATES - finding of closest vertices will be called

    	# starting_vertex = @graph.closest(start_lat, start_lon)[0] if starting_vertex.nil?
    	# destination_vertex = @graph.closest(end_lat, end_lon)[0] if destination_vertex.nil?
    	# UNCOMMENT IN CASE OF NOT KNOWN COORINATES

		  dijkstras[@starting_vertex] = {cost: 0, parent_vertex: :start}
		  # main Dijkstra cycle
		  next_vertex = @starting_vertex
		  dijkstras.size.times do
		  	# picking next vertex by a minimal cost
		    next_vertex = dijkstras.min_by{ |k,v| v[:cost] }[0] unless dijkstras.empty?
		    # iterating through all neighbouring edges
		    @graph.graph.vertices[next_vertex].neighbouring_edges.each do |key, edge|
		      next if dijkstras[key].nil?
		      # determine the cost function and possibly update cost data 
		      if dijkstras[next_vertex][:cost] + edge.weight < dijkstras[key][:cost]
		      	#p edge.weight
		        dijkstras[key][:cost] = dijkstras[next_vertex][:cost] + edge.weight
		        dijkstras[key][:parent_vertex] = next_vertex
		      end
		    end
		    # yayx lets go to another vertex
		    visited_vertices[next_vertex] = dijkstras[next_vertex].clone
		    dijkstras.delete(next_vertex)
		  end

		  next_vertex = @destination_vertex

		  while !next_vertex.eql?(@starting_vertex)
		  	# building of the shortest path - emphesizing all found edges and vertices, calculating time and distance required
		  		@graph.visual_edges.each { |visual_edge| 
		  			if visual_edge.edge == @graph.visual_vertices[visited_vertices[next_vertex][:parent_vertex]].vertex.neighbouring_edges[next_vertex.to_s] 
		  				
		  				visual_edge.emphesized = true 
		  				total_distance += @graph.visual_vertices[visited_vertices[next_vertex][:parent_vertex]].vertex.neighbouring_edges[next_vertex.to_s].physical_distance 
		  				total_time += @graph.visual_vertices[visited_vertices[next_vertex][:parent_vertex]].vertex.neighbouring_edges[next_vertex.to_s].real_time

		  			end
		  			}

		    next_vertex = visited_vertices[next_vertex][:parent_vertex]
		  end

		  @to_be_emphesized << @starting_vertex
		  @to_be_emphesized << @destination_vertex

		  @graph.emphesize(@to_be_emphesized)
		  @graph.set_total_distance(total_distance)
		  @graph.set_total_time(total_time)
		  @graph.print_path_info(@starting_vertex, @destination_vertex)
	end
	
end