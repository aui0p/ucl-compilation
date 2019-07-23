class AddCourseRefToStudentAssignments < ActiveRecord::Migration[5.1]
  def change
    add_reference :student_assignments, :course, foreign_key: true
  end
end
