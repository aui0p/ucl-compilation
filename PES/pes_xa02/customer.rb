class Address
  attr_reader :street, :city, :state, :zip, :country

  def initialize(street, city, state, zip, country)
    @street = street
    @city = city
    @state = state
    @zip = zip
    @country = country
  end
end

class Customer
  attr_reader :name, :address

  @@customers = []
  def initialize(name, address)
    @name = name
    @address = address

    Customer.add(self)
  end

  def Customer.add(customer)
    @@customers << customer
  end

  def Customer.get_customers
    return @@customers
  end

end
