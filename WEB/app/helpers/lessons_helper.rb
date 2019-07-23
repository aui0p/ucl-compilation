module LessonsHelper
  def self.get_by_days(lessons)
    result = {}
    Date::DAYNAMES.each do |day|
    	result[day] = []
    end

    lessons = lessons.sort_by &:start_at

    lessons.each do |lesson|
      result[lesson.day] << lesson
    end
    result
  end
end
