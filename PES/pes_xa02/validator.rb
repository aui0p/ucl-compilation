require 'singleton'
require_relative "product.rb"
class Validator
  include Singleton

  PRODUCT_CODE_REGEX = /(^[P-T][AEIOU]([0-3][0-9][0-9]|4[0-4][0-9]|45[0]))/

  def validate_product_code(code)
    return code =~ PRODUCT_CODE_REGEX
  end

  def validate_non_negative_number(number)
    return number >= 0
  end

  def validate_existing_product(code)
    return true unless Product.get(code).nil?
    return false
  end

  def validate_corresponding_product_data(code, price, name)
    return (validate_existing_product(code)) && Product.get(code).price == price && Product.get(code).name == name
  end

  def validate_available_amount(capacity, quantity)
    return quantity <= capacity
  end
end