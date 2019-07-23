# == Schema Information
#
# Table name: teacher_assignments
#
#  id         :integer          not null, primary key
#  created_at :datetime         not null
#  updated_at :datetime         not null
#

class TeacherAssignment < ApplicationRecord
  belongs_to :teacher
  belongs_to :course
end
