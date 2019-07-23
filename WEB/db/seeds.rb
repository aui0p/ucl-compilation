# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rails db:seed command (or created alongside the database with db:setup).
#
# Examples:
#
#   movies = Movie.create([{ name: 'Star Wars' }, { name: 'Lord of the Rings' }])
#   Character.create(name: 'Luke', movie: movies.first)

unless Building.any?

  buildings = Building.create([{title: "Areál Parukářka", code: "PAR", description: "Lorem ipsum dolor sit amet..."},
                               {title: "Classic 7", code: "C7", description: "Lorem ipsum dolor sit amet..."},
                               {title: "Areál Kolín", code: "KLN", description: "Lorem ipsum dolor sit amet..."},
                               {title: "Areál Brno", code: "BRN", description: "Lorem ipsum dolor sit amet..."}])

  buildings.each do |building|
    Room.create([{title: "Edison #{building.title}", code: "ED_#{building.code}", building: building},
                 {title: "Watt #{building.title}", code: "WT_#{building.code}", building: building},
                 {title: "Einstein #{building.title}", code: "ES_#{building.code}", building: building}])
  end

  courses = Course.create([{code: "AJ1", title: "Anglictina", language: 1, study_type: 1},
                           {code: "MA1E", title: "Mathematics", language: 2, study_type: 1},
                           {code: "WEB", title: "Webove technologie", language: 1, study_type: 1},
                           {code: "MA1", title: "Matematika", language: 1, study_type: 2},
                           {code: "AJ2", title: "Anglictina", language: 1, study_type: 2},
                           {code: "ME", title: "Macroeconomics", language: 2, study_type: 1}])



  students = Student.create([{first_name: "Queen", last_name: "Elizabeth", email: "elizabeth@crown.co.uk", study_type: 2},
                             {first_name: "Peter", last_name: "Quill", email: "peter@gotg.com", study_type: 1},
                             {first_name: "Floyd", last_name: "Lawton", email: "deadshot@aim.org", study_type: 1},
                             {first_name: "Jack", last_name: "Torrance", email: "dull.boy.jack@overlookhotel.com", study_type: 2},
                             {first_name: "Luca", last_name: "Brasi", email: "luca.b@fishing.cz", study_type: 1}])

  teacher = Teacher.create([{first_name: "Henri", last_name: "Poincaré", email: "herni.p@ucl.cz"},
                            {first_name: "John Forbes", last_name: "Nash", email: "john.f.n@ucl.cz"},
                            {first_name: "Abin", last_name: "Sur", email: "abin.s@ucl.cz"}])

  lessons = Lesson.create({start_at: Time.now.at_beginning_of_day, end_at: Time.now.at_end_of_day, teacher_id: 1, room_id:1, course_id:1})

end