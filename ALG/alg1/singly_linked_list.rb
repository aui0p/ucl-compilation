require_relative "index_out_of_bounds_exception"
require_relative "wrong_list_exception"

class SinglyLinkedListItem
  attr_accessor :object, :next
  attr_reader :list
  def initialize(object, list)
    @object = object
    @list = list
  end
end

class SinglyLinkedList
  attr_reader :length

  def initialize
    @length = 0
    @head = nil
    @tail = nil
  end

  private

  # get_item(i)
  def get_item(index)
    raise IndexOutOfBoundsException.new(index, @length) if (index < 0 || index >= @length)
    item = @head
    index.times do
      item=item.next
    end
    item
  end

  public

  # get(i)
  def [](index)
    raise IndexOutOfBoundsException.new(index, @length) if (index < 0 || index >= @length)
   get_item(index).object
  end

  # set(i, value)
  # always returns value
  def []=(index, object)
    raise IndexOutOfBoundsException.new(index, @length) if (index < 0 || index >= @length)
    if index == @length+1
      self<<(object)
    else
      get_item(index).object = object
    end
    object
  end

  # find(value)
  # returns nil when value is not found
  def find(object)
    @length.times do |i|
      if self[i] == object
        return get_item(i)
      end
    end
    nil
  end

  # iterate(callback)
  def each
    if @length > 0
      item = @head
      begin
        yield item.object
        item = item.next
      end until item.nil?
    end
  end

  # insert_before(item, value)
  # returns the new list item
  def insert_before(item, object)
    if item == @head
      return unshift(object)
    else
      tmp = @head
      before = nil
      @length.times do |i|
        if tmp == item
          before
          break
        end
        before = tmp
        tmp = tmp.next
      end
      new_item = SinglyLinkedListItem.new(object, self)
      @length += 1
      before.next = new_item
      new_item.next = item
      return new_item
    end
  end

  # insert_after(item, value)
  # returns the new list item
  def insert_after(item, object)
    raise WrongListException.new(item, self) unless item.list == self
    tmp=item.next
    item.next=SinglyLinkedListItem.new(object,self)
    item.next.next=tmp
    @length+=1
    if item==@tail
      @tail=item.next
    end
    item.next
  end

  # insert(i, value)
  # always returns self
  def insert(index, object)
    raise IndexOutOfBoundsException.new(index, @length) if (index < 0 || index > @length)
    if index == 0
      tmp = get_item(index)
      @head = SinglyLinkedListItem.new(object,self)
      @head.next = tmp
    else
      tmp = get_item(index-1)
      to_move = tmp.next
      a = SinglyLinkedListItem.new(object,self)
      tmp.next = a
      a.next = to_move
    end
    @length += 1
    return self
  end

  # remove_item(item)
  # returns a value of the removed item
  def remove_item(item)
    raise WrongListException.new(item, self) unless item.list == self
    current = item
    if item == @head
      @head = @head.next
    else
      a = @head
      while a.next != item
        a = a.next
      end
      a.next = item.next
    end
    @length -=1
    current.object
  end

  # remove(i)
  # returns the removed value
  def delete_at(index)
    raise IndexOutOfBoundsException.new(index, @length) if (index < 0 || index >= @length)
    if index == 0
      tmp = get_item(index)
      to_return = tmp.object
      @head = tmp.next
      @tail = tmp.next
      tmp = tmp.next
      return to_return
    elsif index == (@length-1)
      temp=get_item(index-1)
      to_return = temp.next
      temp.next = to_return.next
      @tail = tmp
      return to_return.object
    else
      temp=get_item(index-1)
      to_return = temp.next
      temp.next = to_return.next
      return to_return.object
    end
    @length -=1
  end

  # append(value)
  # always returns self
  def <<(object)
    if @tail != nil
    @tail.next = SinglyLinkedListItem.new(object,self)
    @tail = @tail.next
    else
      @head = SinglyLinkedListItem.new(object,self)
      @tail=@head
    end
    @length +=1
    self
  end

  # prepend(value)
  # always returns self
  def unshift(object)
    if @head != nil
      tmp_head = @head
      @head = SinglyLinkedListItem.new(object,self)
      @head.next = tmp_head
    else
      @head = SinglyLinkedListItem.new(object,self)
      @tail = @head
    end
    @length +=1
    self
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
