class Array
  def hrabisort!(ktype = 1)
  	length = self.length
    kjump = []
      tmp = 1
    if ktype == 1
        tmp = 2
        length.times do
            length/tmp == 0 ? break : kjump << length/tmp
            tmp = tmp * 2
        end
    elsif ktype == 2
      while (length/3).ceil > tmp
        kjump << tmp
        tmp = tmp*3 + 1
      end
    elsif ktype == 3
      length**(1/3.0).ceil.times do |i|
        length**(1/2.0).ceil.times do |j|
          tmp = (3**i) * (2**j)
          break if tmp > length
          kjump << tmp
        end
      end
      kjump.sort!
    end

    kjump.reverse_each do |jump|
      (length-jump+1).times do |i|
        tmp_iter1 = 0
        tmp_iter2 = jump
        while (i - tmp_iter2 >= 0) and (self[i - tmp_iter1] < self[i - tmp_iter2])
          tmp = self[i - tmp_iter1]
          self[i - tmp_iter1] = self[i - tmp_iter2]
          self[i - tmp_iter2] = tmp
          tmp_iter1 += jump
          tmp_iter2 += jump
        end
      end
    end

    return self
  end 
end
