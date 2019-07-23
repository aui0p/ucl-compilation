Rails.application.routes.draw do

  namespace :admin do
    resources :students, :courses, :teachers, :lessons, :rooms, :buildings
    get 'dashboard' => 'dashboard#home'
    root 'dashboard#home'
  end

  root 'buildings#index'


  #resources :students
  #resources :courses
  #resources :teachers
  #resources :lessons
  
  resources :buildings, only: [:index, :show, :overview]
  resources :rooms, only: [:index, :show]

  get 'buildings/overview/:id' => 'buildings#overview'
  # For details on the DSL available within this file, see http://guides.rubyonrails.org/routing.html
end
