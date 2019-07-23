class Admin::AdminController < ApplicationController
  http_basic_authenticate_with name: "admin", password: "admin"
layout 'admin'
end
