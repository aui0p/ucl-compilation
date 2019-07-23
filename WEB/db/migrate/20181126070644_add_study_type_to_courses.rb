class AddStudyTypeToCourses < ActiveRecord::Migration[5.1]
  def change
    add_column :courses, :study_type, :integer
  end
end
