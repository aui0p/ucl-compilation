class BuildingsController < ApplicationController
  helper BuildingsHelper
  before_action :set_building, only: [:show, :overview]

  # GET /buildings
  # GET /buildings.json
  def index
    @buildings = Building.all
  end

  # GET /buildings/1
  # GET /buildings/1.json
  def show
    # breadcrumb
  end

  # GET /buildings/1
  # GET /buildings/1.json
  def overview
    # breadcrumb

  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_building
      @building = Building.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def building_params
      params.require(:building).permit(:title, :code)
    end
end
