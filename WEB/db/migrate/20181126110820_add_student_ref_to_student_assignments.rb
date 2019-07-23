class AddStudentRefToStudentAssignments < ActiveRecord::Migration[5.1]
  def change
    add_reference :student_assignments, :student, foreign_key: true
  end
end
