class OrderItem
  attr_reader :code, :name, :price, :quantity

  def initialize(product_code, product_name, price, quantity)
    @code = product_code
    @name = product_name
    @price = price
    @quantity = quantity
  end
end

class Order
  attr_reader :date
  def initialize(order_number, date, shipping_address, billing_address)

    @order_number = order_number
    @date = date
    @shipping_address = shipping_address
    @billing_address = billing_address
    @items = []
  end

  def add_item(item)
    @items << item
  end

  def get_items
    return @items
  end
end
