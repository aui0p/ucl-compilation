class AddKeysUsingImmigration < ActiveRecord::Migration[5.1]
  def change
    add_foreign_key "rooms", "buildings", name: "rooms_building_id_fk"
    add_foreign_key "student_assignments", "courses", name: "student_assignments_course_id_fk"
    add_foreign_key "student_assignments", "students", name: "student_assignments_student_id_fk"
    add_foreign_key "teacher_assignments", "courses", name: "teacher_assignments_course_id_fk"
    add_foreign_key "teacher_assignments", "teachers", name: "teacher_assignments_teacher_id_fk"
  end
end
