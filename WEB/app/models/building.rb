# == Schema Information
#
# Table name: buildings
#
#  id         :integer          not null, primary key
#  title      :string
#  code       :string
#  created_at :datetime         not null
#  updated_at :datetime         not null
#

class Building < ApplicationRecord
  has_many :rooms

  validates :title, presence: true
  validates :code, presence: true
end
