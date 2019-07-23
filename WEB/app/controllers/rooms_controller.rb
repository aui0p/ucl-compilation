class RoomsController < ApplicationController
  before_action :set_room, only: :show

  # GET /rooms
  # GET /rooms.json
  def index
    @rooms = Room.all
  end

  # GET /rooms/1
  # GET /rooms/1.json
  def show
    @lessons_by_days = LessonsHelper.get_by_days @room.lessons_for_current_week
  end

  private
  # Use callbacks to share common setup or constraints between actions.
  def set_room
    @room = Room.find(params[:id])
  end

  def set_building
    
  end

  # Never trust parameters from the scary internet, only allow the white list through.
  def room_params
    params.require(:room).permit(:title, :code, :building_id)
  end
end
