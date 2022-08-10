


def triplet_function
  num1 = 0
  num2 = 0
  num3 = 0

  nums = {}
  n = 1000000
   n.times do |i| break if i >= 15
    num1 = i
    break unless n.times do |j| break if j >= 15
      num2 = j
      break if ((num1+num2) >= 15)
     break unless  n.times do |k| break if k >= 15
        num3 = k
        add1 = num1 + num2
        add2 = num2 + num3
        add3 = add1 + add2
        added = num1 + num2 + num3 + add1 + add2 + add3
        nums[added] = [num1, num2, num3, add1, add2, add3] if added == 15
      end
    end
  end

  nums
end

p triplet_function
