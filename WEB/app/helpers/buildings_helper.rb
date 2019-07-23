module BuildingsHelper
	require 'date'
	def lessons_count(building)
		count = 0

		building.rooms.each do |room|
			count += room.lessons_for_current_day.size
		end

		count
	end

	def lesson_progress_width(start_at, end_at)
		return 100 if DateTime.now >= end_at
		return 0 if DateTime.now <= start_at

		return ((Time.now.strftime("%H:%M")).to_f / (end_at.strftime("%H:%M").to_f)) * 100
	end

	def has_image_asset?(image)
		 return false if Rails.application.assets.find_asset("#{image}.jpg").nil?

		 return true
	end
end
