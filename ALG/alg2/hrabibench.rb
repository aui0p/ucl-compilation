require_relative 'hrabisort'
require_relative 'insertion_sort'
require_relative 'quicksort'
require "benchmark"
# cyklus přes všechny dvojice (m, n)
# vytvoření n polí délky m
# pro každý algoritmus pak:
# - zduplikování polí
# - změření času setřídění
class Float
  def signif(signs) # zaokrouhleni na 3 platne cifry
    Float("%.#{signs}g" % self)
  end
end


class Hrabibench

  def initialize()
    @times_output = []
    @ratios_output = []
    prepare_output()
  end


  def execute(permutations, algs)
    permutations.each do |perm|
      bench_arrays = []
      perm[1].times do
        arr = []

        perm[0].times do
          arr << rand(0..perm[0]-1)
        end

      bench_arrays << arr
      end
      times_tmp = []
      ratios_tmp = []
      times_tmp << perm[0]
      ratios_tmp << perm[0]
      algs.each do |alg|

        bench_time_result = call_benchmark(alg, bench_arrays)

        times_tmp << (bench_time_result.to_f/perm[1]).to_f.signif(3)
        ratios_tmp << ((bench_time_result.to_f/perm[1])/(perm[0]*Math.log(perm[0]))).to_f.signif(3)
      end

      @times_output << times_tmp
      @ratios_output << ratios_tmp
    end
  end

  def generate_output()
    File.open('ratios.txt', 'w') do |file|
      @ratios_output.each do |input|
        file.write(input)
        file.puts
      end
    end
    File.open('times.txt', 'w') do |file|
      @times_output.each do |input|
        file.write(input)
        file.puts
      end
    end
  end


  private

  def array_deep_copy(arr)
    return Marshal.load(Marshal.dump(arr))
  end

  def prepare_output()
    @times_output << ["# m", "K1", "K2", "K3", "insert", "quick"]
    @ratios_output << ["# m", "K1", "K2", "K3", "insert", "quick"]
  end

   def call_benchmark(sort_type, test_data)
    bench_array = array_deep_copy(test_data)

    benchmark_time = Benchmark.realtime do
      bench_array.each do |arr|
        if sort_type[1] == 0
          arr.public_send("#{sort_type[0]}")
        else
          arr.public_send("#{sort_type[0]}", sort_type[1])
        end
      end
    end

    return benchmark_time
  end

end


algs = [["hrabisort!", 1], ["hrabisort!", 2], ["hrabisort!", 3], ["insertion_sort!", 0], ["quicksort!", 0]]
permutations = [[10,50000],[40,10000],[160,2000],[640,200],[2560,20],[10240,5]]

hb = Hrabibench.new

hb.execute(permutations, algs)

hb.generate_output