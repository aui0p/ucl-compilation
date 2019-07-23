require_relative 'unsorted_array'
require_relative 'binary_search_tree'
require_relative 'sorted_array'
require "benchmark"
require "CSV"

# SortedArray nema defaultne metodu find
class SortedArray
  def find(value, type) 
  	case type
	  	when :find_binarys
	  		return binary_search(value)
	  	when :find_interps
	  		return interpolation_search(value)
  	end

  		
    return binary_search(value)
  end
end

class Float
  def signif(signs) #zaokrouhleni na 3 platne cifry
    Float("%.#{signs}g" % self)
  end
end


class Testbench

	def initialize(repeats, permutations)
		#INIT DATA STRUCTURES
		@repeats = repeats

		@arrays_to_find = []
		@arrays_to_insert = []
		@arrays_to_delete = []

		@permutations = permutations

		@insert_elapsed_times = {}
    	@find_elapsed_times = {}
    	@delete_elapsed_times = {}
    	@elements_count = {}

		@permutations.each do |perm|
			m = perm[0]
			@insert_elapsed_times[m] = {unsorted_array: [],
                            sorted_array: [],
                            bst: []}
			@find_elapsed_times[m] = {unsorted_array: [],
			                              sorted_interps: [],
			                              sorted_binarys: [],
			                              bst: []}
			@delete_elapsed_times[m] = {unsorted_array: [],
			                            sorted_array: [],
			                            bst: []}

			@elements_count[m] = {}
		end
	end

	def run()

		@repeats.times do
			@permutations.each do |perm|
				@m = perm[0]
				@n = perm[1]
				# DATA GENERATION FOR EACH CYCLE
				@arrays_to_find = generate_data_arrays()
	  			@arrays_to_insert = generate_data_arrays()
	  			@arrays_to_delete = generate_data_arrays()

	  			unsorted_arrays = Array.new(@n) { UnsortedArray.new }
        		sorted_arrays = Array.new(@n) { SortedArray.new }
        		binary_search_trees = Array.new(@n) { BinarySearchTree.new }

        		@insert_elapsed_times[@m][:unsorted_array] << call_benchmark(unsorted_arrays, :insert)
        		@insert_elapsed_times[@m][:sorted_array] << call_benchmark(sorted_arrays, :insert)
        		@insert_elapsed_times[@m][:bst] << call_benchmark(binary_search_trees, :insert)

        		@find_elapsed_times[@m][:unsorted_array] << call_benchmark(unsorted_arrays, :find)
       		    @find_elapsed_times[@m][:sorted_interps] << call_benchmark(sorted_arrays, :find_interps)
       		    @find_elapsed_times[@m][:sorted_binarys] << call_benchmark(sorted_arrays, :find_binarys)
        		@find_elapsed_times[@m][:bst] << call_benchmark(binary_search_trees, :find)


        		@delete_elapsed_times[@m][:unsorted_array]  << call_benchmark(unsorted_arrays, :delete)
        		@delete_elapsed_times[@m][:sorted_array] << call_benchmark(sorted_arrays, :delete)
        		@delete_elapsed_times[@m][:bst] << call_benchmark(binary_search_trees, :delete)

  			end
		end
	end

	def generate_output()
		generate_statistics()
		CSV.open("times_avg.txt", "w", col_sep: "\t") do |file|

		    file << ["# m", "UA_insert", "SA_insert", "BST_insert", "UA_search", "SA_binarys", "SA_interps", "BTS_search", "UA_delete", "SA_delete", "BST_delete"]
		    	
			@permutations.each do |perm|
		    	m = perm[0]
		    	file << [m, @insert[m][:unsorted_array][:avg],@insert[m][:sorted_array][:avg],@insert[m][:bst][:avg],@find[m][:unsorted_array][:avg],@find[m][:sorted_binarys][:avg],@find[m][:sorted_interps][:avg],@find[m][:bst][:avg],@delete[m][:unsorted_array][:avg],@delete[m][:sorted_array][:avg],@delete[m][:bst][:avg]]
			end
		end


		CSV.open("times_med.txt", "w", col_sep: "\t") do |file|

		    file << ["# m", "UA_insert", "SA_insert", "BST_insert", "UA_search", "SA_binarys", "SA_interps", "BTS_search", "UA_delete", "SA_delete", "BST_delete"]
		    
			@permutations.each do |perm|
		    	m = perm[0]
		    	file << [m, @insert[m][:unsorted_array][:med],@insert[m][:sorted_array][:med],@insert[m][:bst][:med],@find[m][:unsorted_array][:med],@find[m][:sorted_binarys][:med],@find[m][:sorted_interps][:med],@find[m][:bst][:med],@delete[m][:unsorted_array][:med],@delete[m][:sorted_array][:med],@delete[m][:bst][:med]]
			end
		end
		
	end

	private

	def generate_statistics()
		  @insert = {}
		  @find = {}
		  @delete = {}
		  @permutations.each do |perm|
		  	m = perm[0]

		    @insert[m] = {unsorted_array: {avg: nil, med: nil},
		                     sorted_array: {avg: nil, med: nil},
		                     bst: {avg: nil, med: nil}}
		    @find[m] = {unsorted_array: {avg: nil, med: nil},
		                   sorted_interps: {avg: nil, med: nil},
		                        sorted_binarys: {avg: nil, med: nil},
		                   bst: {avg: nil, med: nil}}
		    @delete[m] = {unsorted_array: {avg: nil, med: nil},
		                     sorted_array: {avg: nil, med: nil},
		                     bst: {avg: nil, med: nil}}
		  end

		  fill_statistics_data(@insert_elapsed_times, @insert)
          fill_statistics_data(@find_elapsed_times, @find)
    	  fill_statistics_data(@delete_elapsed_times, @delete)
	end

	def call_benchmark(structures, type)
		#TIME MEASURE OF ONE ARRAYS SET

		# DO THE BENCHMARKING WITH HASH SET CREATION AND RETURN JUST THE HASHSET
		bench_result = Benchmark.realtime do
		    (@n-1).times do |i|
		    	case type
		    	when :insert
		    		@arrays_to_insert[i].each do |number|
		        		structures[i].insert(number)
		      		end
		    	when :find
		    		@arrays_to_find[i].each do |number|
		        		structures[i].find(number)
		      		end
		      	when :find_interps, :find_binarys
		      		@arrays_to_find[i].each do |number|
		        		structures[i].find(number, type)
		      		end
		    	when :delete
		    		@arrays_to_delete[i].each do |number|
		        		structures[i].delete(number)
		      		end
		    	end
		    end
		  end

		  bench_result / (@m * @n)
	end

	def fill_statistics_data(structure, data)
			structure.each do |n, values|
				values.each do |structure, data_array|
					data[n][structure][:avg] = avg(data_array)
					data[n][structure][:med] = median(data_array)
				end
			end
	end

	def generate_data_arrays()
		return Array.new(@n) { Array.new(@m) { rand(@m) } }
	end

	def median(data)
		data.sort!
		length = data.length
		if (length % 2) == 0
		  ((data[length/2] + data[length/2 - 1]).to_f/2).to_f.signif(3)
		else
		  (data[length/2]).to_f.signif(3)
		end
	end

	def avg(data)
	  total = 0
	  data.each do |number|
	    total += number
	  end
	  total/data.length.to_f.signif(3)
	end

end

PERMUTATIONS = [[10,50000],[50,10000],[100,5000],[500,1000],[1000,100],[5000,10]]

t = Testbench.new(10, PERMUTATIONS)
t.run
t.generate_output()
