require_relative 'lib/graph_loader';
require_relative 'process_logger';
require_relative 'lib/dijkstra';

# Class representing simple navigation based on OpenStreetMap project
class OSMSimpleNav

	# Creates an instance of navigation. No input file is specified in this moment.
	def initialize
		# register
		@load_cmds_list = ['--load', '--load-comp']
		@actions_list = ['--export','--show-nodes','--mindist']

		@usage_text = <<-END.gsub(/^ {6}/, '')
	  	Usage:\truby osm_simple_nav.rb <load_command> <input.IN> <action_command> <output.OUT> 
	  	\tLoad commands: 
	  	\t\t --load ... load map from file <input.IN>, IN can be ['DOT']
	  	\t\t --load-comp ... load map and extract the largest component from file <input.IN>, IN can be ['OSM', 'XML']
	  	\tAction commands: 
	  	\t\t --export ... export graph into file <output.OUT>, OUT can be ['PDF','PNG','DOT']
	  	\t\t --show-nodes ... parameter dependent action - 
	  	\t\t\t no parameter lists all vertices of 
	  	\t\t\t id1 and id2 parameters represent IDs of two vertices, these are highlighted and exported to <output.OUT> file
	  	\t\t\t start_lat, start_lon, end_lat, end_lon parameter represent approximate geographical coordinates of two vertices, two closest are found and highlighted and exported to <output.OUT> file
	  	\t\t --mindist ... find a shortes path between two vertices represented with start_lat, start_lon, end_lat, end_lon parameters and export the graph with the shortest path highlighted to <output.OUT>
		END
	end

	# Prints text specifying its usage
	def usage
		puts @usage_text
	end

	# Command line handling
	def process_args
		# not enough parameters - at least load command, input file and action command must be given
		unless ARGV.length >= 3
		  puts "Not enough parameters!"
		  puts usage
		  exit 1
		end

		# read load command, input file and action command 
		@load_cmd = ARGV.shift
		unless @load_cmds_list.include?(@load_cmd)
		  puts "Load command not registred!"
		  puts usage
		  exit 1			
		end
		@map_file = ARGV.shift
		unless File.file?(@map_file)
		  puts "File #{@map_file} does not exist!"
		  puts usage
		  exit 1						
		end
		@operation = ARGV.shift
		unless @actions_list.include?(@operation)
		  puts "Action command not registred!"
		  puts usage
		  exit 1			
		end
		if @operation == '--show-nodes'
			case ARGV.size
			  when 0
			  	@sub_operation = 'show-only'
			  	return
			  when 3
			  	@id1, @id2 = ARGV.shift, ARGV.shift
			  	@sub_operation = 'show-vertices'
			  when 5	
			  	@lat_start, @lon_start, @lat_end, @lon_end = ARGV.shift, ARGV.shift, ARGV.shift, ARGV.shift
			  	@sub_operation = 'show-closest'
			end
		end

		if @operation == '--mindist'
			@lat_start, @lon_start, @lat_end, @lon_end = ARGV.shift, ARGV.shift, ARGV.shift, ARGV.shift
		end
		# possibly load other parameters of the action
		if @operation == '--export'
		end

		# load output file
		@out_file = ARGV.shift
	end

	# Determine type of file given by +file_name+ as suffix.
	#
	# @return [String]
	def file_type(file_name)
		return file_name[file_name.rindex(".")+1,file_name.size]
	end

	# Specify log name to be used to log processing information.
	def prepare_log
		ProcessLogger.construct('log/logfile.log')
	end

	# Load graph from OSM file. This methods loads graph and create +Graph+ as well as +VisualGraph+ instances.
	def load_graph
		graph_loader = GraphLoader.new(@map_file, @highway_attributes)
		@graph, @visual_graph = graph_loader.load_graph()
	end

	# Load graph from Graphviz file. This methods loads graph and create +Graph+ as well as +VisualGraph+ instances.
	def import_graph
		graph_loader = GraphLoader.new(@map_file, @highway_attributes)
		@graph, @visual_graph = graph_loader.load_graph_viz()
	end

	# Run navigation according to arguments from command line
	def run
		# prepare log and read command line arguments
		prepare_log
	    process_args

	    # load graph - action depends on last suffix
	    #@highway_attributes = ['residential', 'motorway', 'trunk', 'primary', 'secondary', 'tertiary', 'unclassified']
	    @highway_attributes = ['residential', 'motorway', 'trunk', 'primary', 'secondary', 'tertiary', 'unclassified']
	    #@highway_attributes = ['residential']
	    if file_type(@map_file) == "osm" or file_type(@map_file) == "xml" then
	    	if @load_cmd == '--load'
	    	# Uncomment in case of reverting to non-osm load
	    		puts ".OSM file format not supported with --load command!"
	    		puts "Use --load-comp or .DOT graph file"
	    		usage
	    		exit 1
	    	end

	    	load_graph
	    elsif file_type(@map_file) == "dot" or file_type(@map_file) == "gv" then
	    	import_graph
	    else
	    	puts "Imput file type not recognized!"
	    	usage
	    end

		# perform the operation
	    case @operation
	      when '--show-nodes'
	      	case @sub_operation
	      	  when 'show-only'
	      	  	@visual_graph.list_vertices()

	      	  when 'show-closest'
	      	  	@visual_graph.color_neighbours_by_coordinates(@lat_start, @lon_start, @lat_end, @lon_end)
	      		@visual_graph.export_graphviz(@out_file)
	      	

	      	  when 'show-vertices'
	      	  	@visual_graph.color_by_id(@id1, @id2)
	      	  	@visual_graph.export_graphviz(@out_file)
	      	end

	      when '--mindist'
	      	dijkstra = DijkstraSolver.new()
	      	@visual_graph.get_vertices_by_coordinates(@lat_start, @lon_start, @lat_end, @lon_end)
	      	dijkstra.get_shortest_path(@visual_graph)

	      	@visual_graph.export_graphviz(@out_file)

	      when '--export'
	      	@visual_graph.export_graphviz(@out_file)
	      	return
	      else
	        usage
	        exit 1
	    end	
	end	
end

osm_simple_nav = OSMSimpleNav.new
osm_simple_nav.run
