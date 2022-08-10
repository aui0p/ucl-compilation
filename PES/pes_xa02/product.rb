class Product
  @@products = {}

  attr_reader :code, :name, :price

  def initialize(code, name, price)

    @code = code
    @name = name
    @price = price

    Product.add(self)
  end

  def Product.add(product)
    @@products[product.code] = product
  end

  def Product.get(code)
    return @@products[code]
  end
end
