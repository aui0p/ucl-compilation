# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# Note that this schema.rb definition is the authoritative source for your
# database schema. If you need to create the application database on another
# system, you should be using db:schema:load, not running all the migrations
# from scratch. The latter is a flawed and unsustainable approach (the more migrations
# you'll amass, the slower it'll run and the greater likelihood for issues).
#
# It's strongly recommended that you check this file into your version control system.

ActiveRecord::Schema.define(version: 20181126122346) do

  create_table "buildings", force: :cascade do |t|
    t.string "title"
    t.string "code"
    t.string "description"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
  end

  create_table "courses", force: :cascade do |t|
    t.string "title"
    t.string "code"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.integer "study_type"
    t.integer "language"
  end

  create_table "lessons", force: :cascade do |t|
    t.datetime "start_at"
    t.datetime "end_at"
    t.integer "duration"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.integer "room_id"
    t.integer "teacher_id"
    t.integer "course_id"
    t.index ["course_id"], name: "index_lessons_on_course_id"
    t.index ["room_id"], name: "index_lessons_on_room_id"
    t.index ["teacher_id"], name: "index_lessons_on_teacher_id"
  end

  create_table "rooms", force: :cascade do |t|
    t.string "title"
    t.string "code"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.integer "building_id"
    t.index ["building_id"], name: "index_rooms_on_building_id"
  end

  create_table "student_assignments", force: :cascade do |t|
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.integer "student_id"
    t.integer "course_id"
    t.index ["course_id"], name: "index_student_assignments_on_course_id"
    t.index ["student_id"], name: "index_student_assignments_on_student_id"
  end

  create_table "students", force: :cascade do |t|
    t.string "first_name"
    t.string "last_name"
    t.string "email"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.integer "study_type"
  end

  create_table "teacher_assignments", force: :cascade do |t|
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.integer "teacher_id"
    t.integer "course_id"
    t.index ["course_id"], name: "index_teacher_assignments_on_course_id"
    t.index ["teacher_id"], name: "index_teacher_assignments_on_teacher_id"
  end

  create_table "teachers", force: :cascade do |t|
    t.string "first_name"
    t.string "last_name"
    t.string "email"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
  end

end
