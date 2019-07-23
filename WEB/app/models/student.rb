# == Schema Information
#
# Table name: students
#
#  id         :integer          not null, primary key
#  first_name :string
#  last_name  :string
#  email      :string
#  created_at :datetime         not null
#  updated_at :datetime         not null
#  study_type :integer
#

class Student < ApplicationRecord
  has_many :student_assignments
  has_many :courses, through: :student_assignments

  extend Enumerize

  enumerize :study_type, in: { full_time: 1, part_time: 2 }, default: :full_time

  validates :first_name, presence: true
  validates :last_name, presence: true
  validates :email, presence: true
  validates :study_type, presence: true, numericality: true

  def to_s
    "#{self.first_name} #{self.last_name}"
  end
end
