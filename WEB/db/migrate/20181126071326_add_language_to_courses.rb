class AddLanguageToCourses < ActiveRecord::Migration[5.1]
  def change
    add_column :courses, :language, :integer
  end
end
