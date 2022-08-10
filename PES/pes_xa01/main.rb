require "csv"

class Programmer
  attr_reader :name, :speed, :daily_wage
  attr_accessor :project

  def initialize(name, speed, daily_wage)
    @name = name
    @speed = speed
    @daily_wage = daily_wage
    @project = nil
  end

  def clear_project
    @project = nil
  end

  def write_code
    @project.receive_work(@speed)
  end

  def project_name
    return project.nil? ? "nil" : project.name
  end

  def print_debug_info
    puts "Programmer #{name}:"
    puts "\tSpeed: #{speed}"
    puts "\tDaily wage: #{daily_wage}"
    puts "\tProject: #{project_name}"
  end
end

class Project
  attr_reader :name, :man_days, :price
  attr_reader :man_days_done
  attr_accessor :state

  def initialize(name, man_days, price)
    @name = name
    @man_days = man_days
    @man_days_done = 0
    @price = price
    @state = :waiting
  end

  def receive_work(man_days)
    @man_days_done += man_days
  end

  def print_debug_info
    puts "Project #{name}:"
    puts "\tState: #{state}"
    puts "\tMan-days total: #{man_days}"
    puts "\tMan-days done: #{man_days_done}"
    puts "\tPrice: #{price}"
  end

  def reset
    @state = :waiting
    @man_days_done = 0
  end
end

class Company
  attr_reader :name, :capacity, :daily_expenses, :budget
  attr_reader :days, :programmers
  attr_reader :projects_waiting, :projects_current, :projects_done
  attr_reader :state

  def initialize(name, capacity, daily_expenses, budget)
    @name = name
    @capacity = capacity
    @daily_expenses = daily_expenses
    @budget = budget
    @state = :idle
    @days = 0
    @projects_waiting = []
    @projects_current = []
    @projects_done = []
  end

  def allocate_projects(projects_array)
    projects_array.each do |proj|
      @projects_waiting << proj
    end
  end

  def allocate_programmers(programmers_array)
    @programmers = []

    programmers_array.sort_by!{|programmer| -(programmer.speed / programmer.daily_wage)}

    @capacity.times do |i|
      @programmers << programmers_array[i]
    end
  end

  def check_projects
    currently_done = []
    @projects_current.reject! { |project| project.man_days_done >= project.man_days && currently_done << project }

    currently_done.each do |project|
      project.state = :done
      @projects_done << project
      @budget += project.price
    end
  end

  def check_programmers
    @programmers.each do |programmer|
      programmer.clear_project if programmer.project.state == :done
    end
  end

  def assign_new_projects
    @projects_current.sort_by!{|project| -(project.man_days - project.man_days_done)}

    @programmers.each do |programmer|
      if programmer.project == nil
        if @projects_waiting.length != 0
            @projects_waiting.first.state = :current
            programmer.project = @projects_waiting.first
            @projects_current << @projects_waiting.first
            @projects_waiting.delete(@projects_waiting.first)
        else
          programmer.project = @projects_current.first
        end
      end
    end
  end

  def programmers_work
    @programmers.each do |programmer|
      programmer.write_code
      @budget -= programmer.daily_wage
    end
    @budget -= daily_expenses
  end

  def check_company_state
    if @budget < 0
      @state = :bankrupt
    else if @projects_current.length == 0 && @budget >= 0
      @state = :finished
         end
    end
  end

  def run
    @state = :running
    while @state != :bankrupt and @state != :finished and @days <= 1000
      @days += 1
      assign_new_projects
      programmers_work
      check_projects
      check_programmers
      check_company_state

      # puts
      # print_debug_info
    end
    @state = :idle if @state == :running
  end

  def output_result
    puts
    puts "Company name #{name}"
    puts "Days running #{@days}"
    puts "Final budget #{@budget}"
    puts "Final state #{@state}"
    puts "Number of projects done #{@projects_done.size}"
  end

  def print_debug_info
    puts "Company #{name}, day #{days}:"
    puts "\tState: #{state}"
    puts "\tCurrent cash flow: #{budget}"
    puts "\tDaily expenses: #{daily_expenses}"
    puts "\tCapacity: #{capacity}"
    puts "\tProgrammers: #{programmers.collect { |programmer| programmer.name + " (" + programmer.project_name + ")" }.join(", ")}"
    puts "\tPROJECTS WAITING: #{projects_waiting.collect { |project| project.name }.join(", ")}"
    puts "\tPROJECTS CURRENT:"
    projects_current.each { |project| project.print_debug_info }
    puts "\tPROJECTS DONE:"
    projects_done.each { |project| project.print_debug_info }
  end
end
FILE_NAME_COMPANIES = "companies.csv"
FILE_NAME_PROGRAMMERS = "programmers.csv"
FILE_NAME_PROJECTS = "projects.csv"
companies = []
programmers = []
projects = []

CSV.open(FILE_NAME_COMPANIES, 'r') do |file|

  file.readline

  file.each do |row|
      companies << Company.new(row[0], row[1].to_i, row[2].to_i, row[3].to_i)
  end
end


CSV.open(FILE_NAME_PROGRAMMERS, 'r') do |file|

  file.readline

  file.each do |row|
      programmers << Programmer.new(row[0], row[1].to_f, row[2].to_i)
  end
end

CSV.open(FILE_NAME_PROJECTS, 'r') do |file|

  file.readline

  file.each do |row|
      projects << Project.new(row[0], row[1].to_f, row[2].to_i)
  end
end

companies.each do |c|
  c.allocate_programmers(programmers)
  c.allocate_projects(projects)
  c.run
  c.output_result
  programmers.each do |prg|
    prg.clear_project
  end
  projects.each do |prj|
    prj.reset
  end
end

puts 'Write simulation`s output to a file? y/n'
case gets.strip.to_s
  when 'y'
    puts 'Writing into results.csv ......'
    CSV.open('results.csv', 'w') do |file|
      file << ['CompanyName', 'DaysRunning', 'FinalBudget', 'FinalState', 'NumberOfProjectsDone']
      companies.each do |company|
        file << [company.name, company.days, company.budget, company.state, company.projects_done.size]
      end
    end
    puts 'Writing finished, exiting'
  when 'n'
    puts 'Exiting'
    exit
  else
    puts 'Invalid option entered, exiting'
    exit
end
