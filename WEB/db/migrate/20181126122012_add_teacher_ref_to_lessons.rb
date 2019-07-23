class AddTeacherRefToLessons < ActiveRecord::Migration[5.1]
  def change
    add_reference :lessons, :teacher, foreign_key: true
  end
end
