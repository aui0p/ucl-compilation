class CreateTeacherAssignments < ActiveRecord::Migration[5.1]
  def change
    create_table :teacher_assignments do |t|

      t.timestamps
    end
  end
end
