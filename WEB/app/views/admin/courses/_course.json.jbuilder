json.extract! course, :id, :title, :code, :study_type, :language, :created_at, :updated_at
json.url course_url(course, format: :json)
