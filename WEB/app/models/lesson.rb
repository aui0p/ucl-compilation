# == Schema Information
#
# Table name: lessons
#
#  id         :integer          not null, primary key
#  start_at   :datetime
#  end_at     :datetime
#  durration  :integer
#  created_at :datetime         not null
#  updated_at :datetime         not null
#

class Lesson < ApplicationRecord
  belongs_to :room
  belongs_to :teacher
  belongs_to :course

  def day
    start_at.strftime("%A")
  end

  def lessons_for_current_week
    Lesson.where(room_id: id).where(start_at: Date.today.beginning_of_week.beginning_of_day..Date.today.end_of_week.end_of_day)
          .order(start_at: :asc).includes(:course).includes(:teacher)
  end
end
