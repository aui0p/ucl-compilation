# == Schema Information
#
# Table name: rooms
#
#  id         :integer          not null, primary key
#  title      :string
#  code       :string
#  created_at :datetime         not null
#  updated_at :datetime         not null
#

class Room < ApplicationRecord
  require 'date'
  require 'active_support/all'
  belongs_to :building
  has_many :lessons

  validates :title, presence: true
  validates :code, presence: true

  def lessons_for_current_week
    Lesson.where(room_id: id).where(start_at: Date.today.beginning_of_week.beginning_of_day..Date.today.end_of_week.end_of_day)
          .order(start_at: :asc).includes(:course).includes(:teacher)
  end

  def lessons_for_current_day
    Lesson.where(room_id: id).where(start_at: DateTime.now.beginning_of_day..DateTime.now.end_of_day)
          .order(start_at: :asc).includes(:course).includes(:teacher)
  end
  DAYS = %w[Monday Tuesday Wednesday Thursday Friday Saturday Sunday].freeze
  HOURS = %w[08000930 09301100 11001230 12301400 14001530 15301700 17001830 18302000].freeze
end
