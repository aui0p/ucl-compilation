class DFSSolver

	#attr_reader :largest_component

	def initialize()
		# Graph to DFS through
		@hash_of_vertices = {}
		@components = {}
		@component_id = -1
		@visited_vertices = {}
	end

	# Base DFS method, stores each component of a given graph as a Hash of vertices +Vertex+ in a @components Hash
	# @components = Hash{@component_id => Hash{}}
	def dfs_solve()
		@hash_of_vertices.each { |key, vertex|
			@visited_vertices[key] = false
		}
		
		@hash_of_vertices.each { |key, vertex| 

			next if @visited_vertices[key]
			@component_id += 1
			@components[@component_id] = {}
			# call the recursion with current vertex
			dfs_visit(vertex)
		} 
		
	end 

	# Recursive DFS call method
	def dfs_visit(vertex)

		@visited_vertices[vertex.id] = true
		@components[@component_id][vertex.id] = vertex

		vertex.neighbouring_vertices.each { |neighbouring_vertex|

			next if @visited_vertices[@hash_of_vertices[neighbouring_vertex].id]
			dfs_visit(@hash_of_vertices[neighbouring_vertex])
		}
	end

	# Calls the DFS algorithms and returns the largest component of given +Graph+
	# @return Hash{vertex_id => Vertex(id, [neighbouring_vertices])}
	def get_largest_component(hash_of_vertices)
		@hash_of_vertices = hash_of_vertices
		dfs_solve()

		#return @components.sort_by{|k, vertices| vertices.length}.reverse[0][1]
		component = @components.max_by{|k, vertices| vertices.length}[0]
		return @components[component]
	end
end