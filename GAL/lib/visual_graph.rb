require 'ruby-graphviz'
require_relative 'visual_edge'
require_relative 'visual_vertex'

# Visual graph storing representation of graph for plotting.
class VisualGraph
  # Instances of +VisualVertex+ classes
  attr_reader :visual_vertices
  # Instances of +VisualEdge+ classes
  attr_reader :visual_edges
  # Corresponding +Graph+ Class
  attr_reader :graph
  # Scale for printing to output needed for GraphViz
  attr_reader :scale

  attr_reader :start_vertex
  attr_reader :end_vertex

  # Create instance of +self+ by simple storing of all given parameters.
  def initialize(graph, visual_vertices, visual_edges, bounds)
  	@graph = graph
    @visual_vertices = visual_vertices
    @visual_edges = visual_edges
    @bounds = bounds
    @scale = ([bounds[:maxlon].to_f - bounds[:minlon].to_f, bounds[:maxlat].to_f - bounds[:minlat].to_f].min).abs / 10.0
    @total_distance_of_path = nil
    @total_time_of_path = nil
  end

  # Export +self+ into Graphviz file given by +export_filename+.
  def export_graphviz(export_filename)
    # create GraphViz object from ruby-graphviz package
    graph_viz_output = GraphViz.new( :G, 
    								                  use: :neato, 
		                                  truecolor: true,
                              		    inputscale: @scale,
                              		    margin: 0,
                              		    bb: "#{@bounds[:minlon]},#{@bounds[:minlat]},
                                  		    #{@bounds[:maxlon]},#{@bounds[:maxlat]}",
                              		    outputorder: :nodesfirst)

    # append all vertices
    @visual_vertices.each { |k,v|

    	graph_viz_output.add_nodes( v.id , :shape => 'point', 
                                         :comment => "#{v.lat},#{v.lon}!", 
                                         :pos => "#{v.y},#{v.x}!")


      graph_viz_output.add_nodes( v.id , :label => "#{v.id}",
                                         :shape => 'point', 
                                         :comment => "#{v.lat},#{v.lon}!", 
                                         :pos => "#{v.y},#{v.x}!",
                                         :color => "red",
                                         :fontcolor => "red") if v.emphesized == true
	  }

    # append all edges
	  @visual_edges.each { |edge| 
      edge.emphesized == true ?
      graph_viz_output.add_edges( edge.v1.id, edge.v2.id, 'arrowhead' => 'none', :color => "red", :penwidth => 3 ) :
    	graph_viz_output.add_edges( edge.v1.id, edge.v2.id, 'arrowhead' => 'none' )

	  }

    # export to a given format
    format_sym = export_filename.slice(export_filename.rindex('.')+1,export_filename.size).to_sym
    graph_viz_output.output( format_sym => export_filename )
  end

  def list_vertices
    @visual_vertices.each do |key, vertex|
      puts "#{vertex.id}: #{vertex.lat}, #{vertex.lon}"
    end
  end

  def color_by_id(id1, id2)
    @visual_vertices[id1].emphesized = true if @visual_vertices.key?(id1)
    @visual_vertices[id2].emphesized = true if @visual_vertices.key?(id2)
  end

  def color_by_id(id)
    @visual_vertices[id].emphesized = true if @visual_vertices.key?(id)
  end

  def color_neighbours(neighbouring_vertices)
     neighbouring_vertices.each do |vertex|
      color_by_id(vertex)

    end
  end

  def color_neighbours_by_coordinates(start_lat, start_lon, end_lat, end_lon)
    #get_vertices_by_coordinates(start_lat, start_lon, end_lat, end_lon)

    p "Found: #{@visual_vertices[closest(start_lat, start_lon)[0]].lat} -- original: #{start_lat}"
    p "Found: #{@visual_vertices[closest(start_lat, start_lon)[0]].lon} -- original: #{start_lon}"

   
    start_neighbours = @visual_vertices[closest(start_lat, start_lon)[0]].vertex.neighbouring_vertices

    end_neighbours = @visual_vertices[closest(end_lat, end_lon)[0]].vertex.neighbouring_vertices


    color_neighbours(start_neighbours)

    color_neighbours(end_neighbours)

    # closest_set(start_lat, start_lon)
  end

  def get_vertices_by_coordinates(start_lat, start_lon, end_lat, end_lon)
    @visual_vertices.each do |key, vertex|
      @start_vertex = vertex if (vertex.lat == start_lat && vertex.lon == start_lon)
      @end_vertex = vertex if (vertex.lat == end_lat && vertex.lon == end_lon)
    end

    #return v1, v2
  end


  def distance(lat1, lon1, lat2, lon2)
      p = 0.017453292519943295
      a = 0.5 - Math.cos((lat2.to_f-lat1.to_f)*p)/2 + Math.cos(lat1.to_f*p)*Math.cos(lat2.to_f*p) * (1-Math.cos((lon2.to_f-lon1.to_f)*p)) / 2
      return 12742 * Math.asin(Math.sqrt(a))
  end
  def closest(lat,lon)
      return @visual_vertices.min_by { |key,vertex| distance(lat.to_f,lon.to_f,vertex.lat.to_f, vertex.lon.to_f) }
  end

  def closest_set(lat,lon)
     @vertices_sorted = @visual_vertices.sort_by { |key, vertex| closest(lat, lon)}
  end

  def set_total_distance(distance)
    @total_distance_of_path = distance
  end

  def set_total_time(time)
    @total_time_of_path = time
  end

  def print_path_info(starting_vertex, destination_vertex)
    unless @total_time_of_path.nil? && @total_distance_of_path.nil?

      p "Total distance from #{starting_vertex} to #{destination_vertex} is approximately #{@total_distance_of_path.round(2)}m (#{(@total_distance_of_path/1000).round(2)}km)."
      p "Total time for traveling from #{starting_vertex} to #{destination_vertex} is approximately #{@total_time_of_path.round(2)} seconds (#{(@total_time_of_path/60).round(2)} minutes)."

    end
  end

  def emphesize(vertices)
    vertices.each {|v| @visual_vertices[v].emphesized = true}
  end

end
