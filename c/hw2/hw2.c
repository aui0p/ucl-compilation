#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <stdbool.h>

typedef int (*passed_fn)(int, int);

typedef struct matrix {
    int** data;
    int n;
    int m;
} matrix;

matrix* init_matrix(int n, int m) {
    matrix* mt = malloc(sizeof(matrix));
    mt->n = n;
    mt->m = m;

    mt->data = malloc(n*sizeof(int*));
    if (!mt->data) {
        printf("Nedostatek pameti pro alokaci matice!");
        exit(EXIT_FAILURE);
    }

    for (size_t i = 0; i < n; i++) {
        mt->data[i] = malloc(m*sizeof(int));
        if (!mt->data[i]) {
            printf("Nedostatek pameti pro alokaci matice!");
            exit(EXIT_FAILURE);
        }
    }
    return mt;    
}

void delete_matrix(matrix* mt) {
    for (size_t i = 0; i < mt->n; i++) {
        free(mt->data[i]);
    }
    free(mt->data);
    free(mt);
}

void print_matrix(matrix* mt) {
    printf("Matice %d x %d:\n", mt->n, mt->m);
    for (size_t x = 0; x < mt->n; x++) {
        for (size_t y = 0; y < mt->m; y++) {
            printf("%d ", mt->data[x][y]);
        }
        printf("\n");
    }
}

void print_matrix_to_file(matrix* mt, char* filename) {
    FILE* file = fopen(filename, "w");
    if (file == NULL) {
        printf("Spatna cesta k souboru!");
        exit(EXIT_FAILURE);
    }

    for (size_t x = 0; x < mt->n; x++) {
        for (size_t y = 0; y < mt->m; y++) {
            fprintf(file, "%d ", mt->data[x][y]);
        }
        fprintf(file, "\n");
    }
    printf("Matice vypsana do souboru: %s\n", filename);
    free(filename);
    fclose(file);
}

int gcd(int a, int b) {
    a = (a > 0) ? a : -a;
    b = (b > 0) ? b : -b;
    int gcd = fmin(a,b);

    while (gcd > 0) {
        if ((a % gcd == 0) && (b % gcd == 0)) break;
        --gcd;
    }
    return gcd;
}

int lcm(int a, int b) {
    return (a*b) / gcd(a,b);
}

void insert_into_matrix(matrix* mt, passed_fn fn, int x, int y) {
    if(mt->n < x || mt->m < y) {
        printf("Koordinaty mimo hranice matice!\n");
        exit(EXIT_FAILURE);
    }

    mt->data[x][y] = fn(x + 1,y + 1);
}

void generate_pairs(matrix* mt, passed_fn fn) {
    for (size_t x = 0; x < mt->n; x++) 
        for (size_t y = 0; y < mt->m; y++) 
            insert_into_matrix(mt, fn, x, y);
}

int main(int argc, char** argv) {
    if (argc < 3 || !argv[1]) {
        printf("Nedostatek parametru!\n");
        return -1;
    }

    passed_fn fn;
    bool save_to_file;
    char* filename = NULL;
    int m, n, jump;

    if (strcmp(argv[1], "-nsn") == 0) {
        fn = lcm;
    } else if (strcmp(argv[1], "-nsd") == 0) {
        fn = gcd;
    } else {
        printf("Nevhodny prvni parametr!\n");
        return -1;
    }

    if (argv[2] && strcmp(argv[2], "-f") == 0 && argv[3]) {
        save_to_file = true;
        filename = malloc(strlen(argv[3])*sizeof(char));
        strcpy(filename, argv[3]);
    } 

    jump = save_to_file ? 4 : 2;
    if (argv[jump] && argv[jump+1]) {
        n = atoi(argv[jump]);
        m = atoi(argv[jump+1]);
    } else if (argv[jump]) {
        n = atoi(argv[jump]);
        m = n;
    } else {
        printf("Neplatne parametry!\n");
        return -1;
    }

    matrix* mt = init_matrix(n, m);
    generate_pairs(mt, fn);
    
    print_matrix(mt);
    if (save_to_file) print_matrix_to_file(mt, filename);

    delete_matrix(mt);
    getchar();
    return 0;
}