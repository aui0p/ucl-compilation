require_relative "product.rb"
require_relative "store.rb"
require_relative "customer.rb"
require_relative "order.rb"
require_relative "logger.rb"
require_relative "validator.rb"

require 'rubygems'
require 'nokogiri'
require 'time'

class Shop



  def initialize(arguments)
    @logger = Logger.instance
    @validator = Validator.instance

    #abort with less parameters
    @logger.abort_not_enough_parameters(arguments.length)

    #initialize instance variables with file names
    @products_input_filename, @store_input_filename, @orders_input_filename, @store_output_filename, @customers_output_filename = arguments.map { |i| i.to_s}

  end

  def load_products

    #abort non-existing file
    @logger.abort_non_existing_file(@products_input_filename)

    @logger.log_process(1)

    products_feed = Nokogiri::XML(open(@products_input_filename))

  #extract product nodes
    products_feed.xpath('//product').to_a.each do |product|
       product_code = product.xpath('./@code').to_s
       product_name = product.xpath('./name/text()').to_s
       product_price = product.xpath('./price/text()').to_s

       #validate inputs, log message & skip adding if not valid or already saved
       unless @validator.validate_non_negative_number(product_price.to_i) && @validator.validate_product_code(product_code) && !(@validator.validate_existing_product(product_code))
         @logger.log_error("Given code (#{product_code}) is not valid or already saved in products database, or given price (#{product_price}) is a negative number - ignoring product.")
         next
       end

       Product.new(product_code, product_name, product_price.to_i)
    end

  end

  def load_store

    @store = Store.new

    #abort non-existing file
    @logger.abort_non_existing_file(@store_input_filename)


    @logger.log_process(2)

    store_feed = Nokogiri::XML(open(@store_input_filename))

    #extract product nodes
    store_feed.xpath('//product').to_a.each do |product|
      product_code = product.xpath('./@code').to_s
      product_amount = product.xpath('./@amount').to_s

      #validate inputs, log message & skip adding if not valid
      unless @validator.validate_existing_product(product_code) && @validator.validate_non_negative_number(product_amount.to_i)
        @logger.log_error("Given code (#{product_code}) is not present in products database or given amount (#{product_amount}) is a negative number - ignoring product.")
        next
      end

      #add product to store
      @store.add_product(product_code, product_amount.to_i)
    end
  end

  def load_orders
    @orders = []

    #abort non-existing file
    @logger.abort_non_existing_file(@orders_input_filename)

    @logger.log_process(3)

    orders_feed = Nokogiri::XML(open(@orders_input_filename))

    #extract order nodes
    orders_feed.xpath('//order').to_a.each do |order|
      order_date = order.xpath('./@date').to_s
      order_number = order.xpath('./@number').to_s

      customer_name = order.xpath("./address[@type='billing']/name/text()").to_s

      address_street = order.xpath("./address[@type='billing']/street/text()").to_s

      address_city = order.xpath("./address[@type='billing']/city/text()").to_s

      address_state = order.xpath("./address[@type='billing']/state/text()").to_s

      address_zip = order.xpath("./address[@type='billing']/zip/text()").to_s

      address_country = order.xpath("./address[@type='billing']/country/text()").to_s

      order_notes = order.xpath('./notes/text()').to_s

      billing_address = Address.new(address_street,address_city,address_state,address_zip,address_country)

      #vytvoreni a ulozeni zakaznika uz zde - prislo mi "primocarejsi" udelat to uz pri parsovani souboru. (ukladal bych ho, i kdyby nebyla validni zadna
      # z jeho objednavek, takze by stejne nemelo dojit k nejake necekane vyjimce s jeho ulozenim)
      Customer.new(customer_name, billing_address)

      #nil pro shipping_address - neni treba ji ani parsovat, neb neni nijak vyuzivana
      order_instance = Order.new(order_number, order_date,nil,billing_address)

      #extract order items nodes
      order.xpath('./items/item').to_a.each do |item|
        item_code = item.xpath('./@code').to_s
        item_name = item.xpath('./name/text()').to_s
        item_quantity = item.xpath('./quantity/text()').to_s
        item_price = item.xpath('./price/text()').to_s

        order_instance.add_item(OrderItem.new(item_code, item_name,item_price.to_i,item_quantity.to_i))
      end
      @orders << order_instance
    end
  end

  def process_order(order)

    order.get_items.each do |order_item|
      #validate inputs, log message & skip selling if not valid
      unless @validator.validate_corresponding_product_data(order_item.code,order_item.price,order_item.name) && @validator.validate_available_amount(@store.available_amount(order_item.code),order_item.quantity)
        @logger.log_error("Given code (#{order_item.code}) is not present in products database or given name (#{order_item.name}) and price (#{order_item.price}) are not corresponding, or store does not contain enough amount #{order_item.quantity} - ignoring order item.")
        next
      end
     @store.sell_product(order_item.code, order_item.quantity)
    end

  end

  def process_orders

    @logger.log_process(4)

    @orders.sort_by!{|o| Time.xmlschema(o.date)}

     @orders.each do |order|
      process_order(order)
    end
  end

  def save_store

    @logger.log_process(5)


    xml = Nokogiri::XML::Builder.new do |xml|
        xml.store do
          @store.products.each do |code, product|
            xml.product(:code => code, :amount => @store.available_amount(code))
          end
        end
    end
    #p xml.to_xml

    File.write(@store_output_filename, xml.to_xml)
  end

  def save_customers

    @logger.log_process(6)


    xml = Nokogiri::XML::Builder.new do |xml|
      xml.customers do
        Customer.get_customers.each do |customer|

          xml.customer(:name => customer.name) do
            xml.address do
              xml.street customer.address.street
              xml.city customer.address.city
              xml.state customer.address.state
              xml.zip customer.address.zip
              xml.country customer.address.country
            end
          end
        end
      end
    end
    #p xml.to_xml

    File.write(@customers_output_filename, xml.to_xml)
  end

  def run
    load_products
    load_store
    load_orders
    process_orders
    save_store
    save_customers
  end
end


shop = Shop.new(ARGV)
shop.run

