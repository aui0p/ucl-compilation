require_relative "boolean_array"
require_relative "singly_linked_list"

class StackEmptyException < Exception
  def initialize(stack)
    @stack = stack
  end

  def message
    "Stack is empty"
  end
end

class StackFullException < Exception
  def initialize(stack)
    @stack = stack
  end

  def message
    "Stack is full"
  end
end

class SinglyListStack
  def initialize
    @data = SinglyLinkedList.new
  end

  def push(x)
    @data << x
  end

  def pop
    raise StackEmptyException.new(self) if @data.length == 0
    @data = delete_at(@data.length)
  end
end

class BooleanArrayStack
  def initialize(size)
    @data = BooleanArray.new(size)
  end

  def push(x)
    raise StackFullException.new(self) if @data.length == @data.used_length
    @data << x
  end

  def pop
    raise StackEmptyException.new(self) if @data.used_length == 0
    @data.delete_at(@data.used_length-1)
  end
end
