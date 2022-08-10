require 'singleton'

class Logger
  include Singleton


  attr_accessor :process


  def initialize
    @process = 1
    @error_count = 1
    @process_text = ''
    @process_entity = 'Products'
  end

  def log_error(error_message)
    puts "---------------ERROR #{@error_count}---------------"
    puts "Process: #{@process_text}"
    puts 'Error text:'
    puts error_message
    puts '--------------- END OF ERROR -----------'
    @error_count += 1
  end

  def log_process(process_number)
    puts 'Current process:'
    @process = process_number
    case @process
      when 1
        @process_entity = 'Products'
        puts @process_text = 'Extracting products data'
      when 2
        @process_entity = 'Store'
        puts @process_text = 'Extracting store data'
      when 3
        @process_entity = 'Orders'
        puts @process_text = 'Extracting orders data'
      when 4
        @process_entity = 'Orders'
        puts @process_text = 'Processing orders'
      when 5
        @process_entity = 'Store'
        puts @process_text = 'Exporting store data'
      when 6
        @process_entity = 'Customers'
        puts @process_text = 'Exporting customers data'

    end
    puts '---------------------------'
  end

  def abort_non_existing_file(filename)
    abort "#{@process_entity} file does not exist - aborting application" unless File.exists?(filename)
  end

  def abort_not_enough_parameters(parameters_count)
    abort "Not enough parameters given - aborting application" unless parameters_count == 5
  end
end