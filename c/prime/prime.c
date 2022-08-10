#include <stdbool.h>
#include <math.h>

#include "prime.h"

bool is_prime(UC int num) {
    if (num < 2) return false;
    if ((num % 2) == 0) return false;
    if (num < 4) return true;

    for (int i = 3; i < floor(sqrt((double)num)); i+=2) {
        if (num % i == 0) {
            return false;
        }
    }
    return true;
}

int next_consecutive_prime(unsigned int p) {
    while(!is_prime(p)) {
        ++p;
    }
    return p;
}