#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Taylor's sum(1/k!), k->inf approximation

int main (int argc, char** argv) {
    if (argc < 3) {
        printf("Nedostatek parametru!");
        exit(EXIT_FAILURE);
    }

    int N;
    if (argv[1] && argv[2] && strcmp(argv[1], "-n") == 0) {
        N = atoi(argv[2]);
    } else {
        printf("Nevhodne parametry!");
        exit(EXIT_FAILURE);
    }

    int size = N;
    int n = N, x;
    int a[size];

    while (--n) {
        a[n] = 1 + (1 / n);
    }

    while (N > 0) {
        for (n = N--; --n;) {
            a[n] = x%n;
            x = (10 * a[n-1]) + (x / n);
        } 
        printf("%d", x);
   }
}