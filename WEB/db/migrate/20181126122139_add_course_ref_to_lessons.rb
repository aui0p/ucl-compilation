class AddCourseRefToLessons < ActiveRecord::Migration[5.1]
  def change
    add_reference :lessons, :course, foreign_key: true
  end
end
