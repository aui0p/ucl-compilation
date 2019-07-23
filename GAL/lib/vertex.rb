# Class representing vertex in a graph
class Vertex
  # id of the +Vertex+
  attr_reader :id

  attr_accessor :neighbouring_vertices
  attr_accessor :neighbouring_edges

  # create instance of +self+ by simple storing of all parameters
  def initialize id
    @id = id
    @neighbouring_vertices = []
    @neighbouring_edges = {} 
  end
end

