require_relative "boolean_array"
require_relative "singly_linked_list"

class QueueEmptyException < Exception
  def initialize(queue)
    @queue = queue
  end

  def message
    "Queue is empty"
  end
end

class QueueFullException < Exception
  def initialize(queue)
    @queue = queue
  end

  def message
    "Queue is full"
  end
end

class SinglyListQueue
  def initialize
    @data = SinglyLinkedList.new
  end

  def queue(x)
    @data << x
  end

  def dequeue
    raise StackEmptyException.new(self) if @data.length == 0
    @data.delete_at(0)
  end
end

class BooleanArrayQueue
  def initialize(size)
    @data = BooleanArray.new(size)
    @start = -1
    @end = 0
    @size = size
  end

  def queue(x)
    raise StackFullException.new(self) if @start == @end
    @data[@end] = x
    @end = (@end+1) % @size
    @start = 0 if @start == -1
    self
  end

  def dequeue
    raise StackEmptyException.new(self) if @start == -1
    temp = @start
    @start = (@start+1) % @size
    if @start == @end
      @start = -1
      @end = 0
    end
    @data[temp]
  end
end
