# == Schema Information
#
# Table name: courses
#
#  id         :integer          not null, primary key
#  title      :string
#  code       :string
#  created_at :datetime         not null
#  updated_at :datetime         not null
#  study_type :integer
#  language   :integer
#

class Course < ApplicationRecord
  has_many :teacher_assignments
  has_many :teachers, through: :teacher_assignments


  has_many :student_assignments
  has_many :students, through: :student_assignments

  has_many :lessons

  extend Enumerize

  enumerize :language, in: { czech: 1, english: 2 }, default: :czech, scope: true, predicates: true
  enumerize :study_type, in: { full_time: 1, part_time: 2 }, default: :full_time, scope: true, predicates: true
  # enumerize :study_type, in: [ "Full Time", "Part time"], default: "Full Time", scope: true

  validates :title, presence: true
  validates :code, presence: true
  validates :study_type, presence: true, numericality: true
  validates :language, presence: true, numericality: true
end
