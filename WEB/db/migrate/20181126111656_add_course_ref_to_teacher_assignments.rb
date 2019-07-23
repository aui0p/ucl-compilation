class AddCourseRefToTeacherAssignments < ActiveRecord::Migration[5.1]
  def change
    add_reference :teacher_assignments, :course, foreign_key: true
  end
end
