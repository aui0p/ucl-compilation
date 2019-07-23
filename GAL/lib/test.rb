require 'ruby-graphviz'
require 'nokogiri'


# Create a new graph
g = GraphViz.new( :G, :type => :digraph )

# Create two nodes
hello = g.add_nodes( "Hello" )
world = g.add_nodes( "World" )

# Create an edge between the two nodes
g.add_edges( hello, world )

# Generate output image
#g.output( :png => "hello_world.png" )

def stringify_highway_attributes
	fltr = String.new
	filter.each_with_index do |el, i|
		fltr += el
		fltr += ', ' unless i == (filter.length - 1)
	end
end

	filter = ['residential', 'motorway', 'trunk', 'primary', 'secondary', 'tertiary', 'unclassified']
	fltr = String.new
	filter.each_with_index do |el, i|
		fltr += el
		fltr += ', ' unless i == (filter.length - 1)
	end


	def stringify_test
		return ['residential', 'motorway', 'trunk', 'primary', 'secondary', 'tertiary', 'unclassified'].join(", ")
	end


	# Parses min and max lot and lat from <bounds> tag
	def parse_bounds
		map_xml_feed = Nokogiri::XML(open("near_ucl.osm"))

		graph_bounds = map_xml_feed.xpath("//bounds")
		return [graph_bounds.xpath("@minlat").to_s, graph_bounds.xpath("@minlon").to_s, graph_bounds.xpath("@maxlat").to_s, graph_bounds.xpath("@maxlon").to_s]
	end


	def capacity_return
		filters = ['residential', 'motorway', 'trunk', 'primary', 'secondary', 'tertiary', 'unclassified']

		filters.each_with_index do |filter, i|
			p i
			(
				p filter
				prev = filter + ","
				p prev

			) unless (i+1).eql? filters.size
		end
		
	end

	def true_return_test
		test_hash = {}
		10.times do |index|
			test_hash[index] = false
			index += 1
		end

		p "rand" unless test_hash[5]
	end

	def hash_test
		test_hash = {}
		10.times do |index|
			test_hash[index] << index
			index += 1
		end

		test_hash.each do |item|
			p item
		end
	end

hash_test














