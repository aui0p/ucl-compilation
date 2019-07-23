# Class representing edge of a graph
class Edge
  # Staring vertex of an edge
  attr_reader :v1
  # Target vertex of an edge
  attr_reader :v2
  # Maximal speed on this edge
  attr_reader :max_speed
  # Indicator of on direction edge - i.e. can be passed only from +v1+ to +v2+
  attr_reader :one_way
  # Weight of an edge calculated based on Lat and Lon position of its vertices and maximum allowed speed
  attr_reader :weight

  attr_reader :physical_distance

  attr_reader :real_time

  # Create instance of +self+ by simple storing of all parameters
  def initialize(v1, v2, max_speed, one_way)
    @v1 = v1
    @v2 = v2
    @max_speed = max_speed
    @one_way = one_way
    @weight = 0
    @physical_distance = 0
    @real_time = 0
  end

  # Gets calculated weight from VisualEdge
  def set_edge_weight(weight)
    @weight = (weight / @max_speed)
  end

  # Gets calculated physical distance from VisualEdge
  def set_edge_physical_distance(physical_distance)
    @physical_distance = physical_distance
  end

  def set_real_time()
    speed_sec = (@max_speed / 3.6).ceil(2)
    @real_time = (@physical_distance / speed_sec).ceil(2)
  end
end

