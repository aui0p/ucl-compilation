#include "../ds/da/dyn_a.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int comp(const void* a, const void* b) {
    char* s1 = *(char**)a;
    char* s2 = *(char**)b;

    if(s1 > s2) return 1;
    if(s1 < s2) return -1;
    return 0;
}

FILE* open_input_file(char* filename) {
    FILE* f = fopen(filename, "r");
    if (f == NULL) {
        printf("Soubor se nepodarilo otevrit!\n");
        exit(EXIT_FAILURE);
    }
    free(filename);
    return f;
}

char* read_sentences(FILE* f) {
    int buffer_size = 32, next = 0, c;
    char* sentence_buffer = malloc(buffer_size);
    while (1) { 
        if (next == buffer_size) {
            sentence_buffer = realloc(sentence_buffer, buffer_size*=2);
            if (sentence_buffer == NULL) {
                printf("Nedostatek pameti!\n");
                exit(EXIT_FAILURE);
            }
        }
        c = fgetc(f);
        if(c == '\n' ) continue;
        // || (c == '-' && fgetc(f) == '\n')
        if (c == EOF || c == '.') {
            sentence_buffer[next++] = 0;
            break;
        }
        sentence_buffer[next++] = c;
    }

    if (c == EOF && next == 1) {
        free(sentence_buffer);
        sentence_buffer = NULL;
    }
    return sentence_buffer;
}

void parse_sentences(FILE* input_file, dyn_a* s_array) {
    char *sentence;
    while ((sentence = read_sentences(input_file))) {
        dyn_a_add(s_array, sentence);
        printf("%s\n", sentence);
        free(sentence);
    }
}

void print_sentences(char** s_arr, const int c) {
    printf("Lexikograficky serazene vety:\n");
    for (int i = 0; i < c; i++)
        printf("%s\n", s_arr[i]);
}

void solve(FILE* input_file) {
    dyn_a* s_array = dyn_a_init_dynamic_array(4);

    parse_sentences(input_file, s_array);
    qsort(s_array->items, s_array->size, sizeof(char*), comp);
    print_sentences(s_array->items, s_array->size);

    dyn_a_delete_array(s_array);
}

int main(int argc, char** argv) {
    if (argc < 3) {
        printf("Nedostatek parametru!\n");
        exit(EXIT_FAILURE);
    }

    char* filename = NULL;
    if (argv[1] && argv[2] && strcmp(argv[1], "-i") == 0){
        filename = malloc(strlen(argv[2])*sizeof(char));
        filename = strcpy(filename, argv[2]);
    }

    FILE* f = open_input_file(filename);
    solve(f);
}