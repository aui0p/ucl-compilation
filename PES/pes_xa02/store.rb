class Store
  attr_reader :products

  def initialize
    @products = {}
  end

  def add_product(code, amount)
    @products[code] = amount
  end

  def sell_product(code, amount)
    @products[code] -= amount
  end

  def available_amount(code)
    return @products[code]
  end
end
