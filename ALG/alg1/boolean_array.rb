require_relative "index_out_of_bounds_exception"
require_relative "boolean_expected_exception"

class BooleanArray
  attr_reader :length
  attr_reader :used_length

  def initialize(length)
    @length = length
    @used_length = 0
    @inner = Array.new((length - 1) / 8 + 1, 0)
  end

  private

  def get_byte_bit(index)
    return [index / 8, index % 8]
  end

  public

  # get(i)
  def [](index)
    raise IndexOutOfBoundsException.new(index, @length) if (index < 0 || index >= @used_length)
    byte, bit = get_byte_bit(index)
    return ((@inner[byte]) & (1 << bit)) != 0
  end

  # set(i, value)
  # always returns value
  def []=(index, value)
    raise IndexOutOfBoundsException.new(index, @length) if (index < 0 || index >= @length)
    raise BooleanExpectedException.new(value) unless value.instance_of?(TrueClass) || value.instance_of?(FalseClass)
    byte, bit = get_byte_bit(index)
    if value
      @inner[byte] |= 1 << bit
    else
      @inner[byte] &= 255 - (1 << bit)
    end
    @used_length = index + 1 if index >= @used_length
    return value
  end

  # find(value)
  # returns nil when value is not found
  def find(value)
    index = 0
    self.each do |i|
      if i == value
        return index
      else
        index+=1
      end
    end
    nil
  end

  # iterate(callback)
  def each
    @used_length.times do |i|
      yield self[i]
    end
  end

  # insert(i, value)
  # always returns self
  def insert(index, value)
    # raises IndexOutOfBoundsException.new(index, @length) if the index is out of the correct bounds [0, @length), or if insert into the full array
    # raises BooleanExpectedException.new(value) if the value is not an instance of TrueClass or FalseClass
    raise IndexOutOfBoundsException.new(index, @length) if  index > @used_length || index<0 || @used_length == @length || @length == 0
    raise BooleanExpectedException.new(value) unless value.instance_of?(TrueClass) || value.instance_of?(FalseClass)

    if index == @used_length
      self<<value
    else
      (@used_length - index+1).times do |i|
        self[@used_length - i] = self[@used_length - i -1]
      end
      self[index] = value
    end
    self
  end

  # remove(i)
  # returns the removed value
  def delete_at(index)
    # raises IndexOutOfBoundsException.new(index, @length) if the index is out of the correct bounds [0, @used_length)
    # does nothing when
    raise IndexOutOfBoundsException.new(index, @length) if (index >= @used_length || index<0 || @length == 0)
    item = self[index]
    (@used_length-index-1).times do |i|
      self[index + i] = self[index + 1 + i]
    end
    @used_length -= 1
    
    item
  end

  # append(value)
  # always returns self
  def <<(value)
    raise IndexOutOfBoundsException.new(@used_length, @length) if @used_length == @length
    self[@used_length] = value
    return self
  end

  # prepend(value)
  # always returns self
  def unshift(value)
    # raises IndexOutOfBoundsException.new(index, @length) if the array is full
    raise IndexOutOfBoundsException.new(@used_length, @length) if @used_length == @length
    raise BooleanExpectedException.new(value) unless value.instance_of?(TrueClass) || value.instance_of?(FalseClass)
    insert(0,value)
    return self
  end

  # converts self to a standard Ruby Array
  def to_a
    result = []
    self.each do |item|
      result << item
    end
    return result
  end

end