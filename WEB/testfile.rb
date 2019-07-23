require 'date'
require 'active_support/all'

start_at = Date.today.beginning_of_week.beginning_of_day..Date.today.end_of_week.end_of_day

p Time.now.at_beginning_of_day

p start_at
p Date.today.beginning_of_week.beginning_of_day
p Date.today.end_of_week.end_of_day

p Time.now.at_end_of_day

p Date::DAYNAMES