# == Schema Information
#
# Table name: student_assignments
#
#  id         :integer          not null, primary key
#  created_at :datetime         not null
#  updated_at :datetime         not null
#

class StudentAssignment < ApplicationRecord
  belongs_to :student
  belongs_to :course
end
