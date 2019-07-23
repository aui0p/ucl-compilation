class AddTeacherRefToTeacherAssignments < ActiveRecord::Migration[5.1]
  def change
    add_reference :teacher_assignments, :teacher, foreign_key: true
  end
end
