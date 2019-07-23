# Class representing visual representation of edge
class VisualEdge
  # Starting +VisualVertex+ of this visual edge
  attr_reader :v1
  # Target +VisualVertex+ of this visual edge
  attr_reader :v2
  # Corresponding edge in the graph
  attr_reader :edge
  # Boolean value given directness
  attr_reader :directed
  # Boolean value emphasize character - drawn differently on output (TODO)
  attr_accessor :emphesized

  # create instance of +self+ by simple storing of all parameters
  def initialize(edge, v1, v2)
  	@edge = edge
    @v1 = v1
    @v2 = v2
    @emphesized = false
  end

  def calculate_edge_weight
    weight = Math::sqrt(((v2.lat.to_f - v1.lat.to_f) ** 2).abs + ((v2.lon.to_f - v1.lon.to_f) ** 2).abs)

    edge.set_edge_weight(weight)
  end

  def calculate_edge_physical_distance
    rad_per_deg = Math::PI/180
    rkm = 6371
    rm = rkm * 1000


    dlat_rad = (v2.lat.to_f - v1.lat.to_f) * rad_per_deg
    dlon_rad = (v2.lon.to_f - v1.lon.to_f) * rad_per_deg

    lat1_rad, lon1_rad = (v1.lat.to_f * rad_per_deg), (v1.lon.to_f * rad_per_deg)
    lat2_rad, lon2_rad = (v2.lat.to_f * rad_per_deg), (v2.lon.to_f * rad_per_deg)

    a = Math.sin(dlat_rad/2)**2 + Math.cos(lat1_rad) * Math.cos(lat2_rad) * Math.sin(dlon_rad/2)**2
    c = 2 * Math::atan2(Math::sqrt(a), Math::sqrt(1-a))

    edge.set_edge_physical_distance((rm * c).round)
    #edge.set_edge_weight((rm*c))
  end

  def calculate_real_time
    edge.set_real_time()
  end

end

