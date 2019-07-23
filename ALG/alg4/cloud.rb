def generate_computers computernum
  computers = []
  computernum.times do |i|
    computers << i + 1
  end
  computers
end

def prepare_timetables computers
  timetable = {}
  timetables_sum = {}
  computers.each do |computer|
    timetable[computer] = []
    timetables_sum[computer] = 0
  end
  [timetable, timetables_sum]
end

def generate_tasks num
  tasks = {}
  num.times do |i|
    tasks[i+1] = rand(1..10)
  end
  tasks
end

def print_output timetable
  timetable.each do |computer|
    print "#PC#{computer[0]}|"
    computer[1].each do |task|
      print "#{task[0]}"
      task[1].times { print "*" }
      print "|"
    end
    puts
  end
end

def load_balance tasks, computernum
  timetable, timetable_sum = prepare_timetables(generate_computers(computernum))
  tasks_sorted = tasks.sort_by { |key,value| [-value, key]}.to_h

  tasks_sorted.each do |task|
    timetable_sorted = timetable_sum.sort_by { |key,value| [value, key]}.to_h
    timetable_sum[timetable_sorted.keys[0]] += task[1]
    timetable[timetable_sorted.keys[0]] << task
  end

  print_output timetable
  timetable_sum.sort_by{|key, value| -value}.first[1]
end

TASKS_NUMBER = 9
COMPUTERS_NUMBER = 4


p "Delka rozvrhu je: #{load_balance(generate_tasks(TASKS_NUMBER), COMPUTERS_NUMBER)}"