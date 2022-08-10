#include "../hashtable/src/hashtable.h"
#include "../ds/da/dyn_a.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

typedef void (*word_processor)(void*, char*);

int dht_comp(const void* a, const void* b) {
    dht_entry* e1 = *(dht_entry**)a;
    dht_entry* e2 = *(dht_entry**)b;
    if(!e1 && !e2) return 0;
    if(!e1) return 1;
    if(!e2) return -1;

    return e2->value - e1->value;
}

void insert_into_ht(void* ht, char* word_key) {
    dht_table* cast_ht = (dht_table*)ht;
    int new_val = 0;
    int v = dht_find(cast_ht, word_key);
    if (v != -1) new_val = v;
    ++new_val;

    dht_insert(cast_ht, word_key, new_val);
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

char* read_line(FILE* f) {
    int buffer_size = 32,  next = 0, c;
    char* line_buffer = malloc(buffer_size);
    while (1) { 
        if (next == buffer_size) {
            line_buffer = realloc(line_buffer, buffer_size*=2);
            if (line_buffer == NULL) {
                printf("Nedostatek pameti!\n");
                exit(EXIT_FAILURE);
            }
        }
        c = fgetc(f);
        if (isalpha(c) && isupper(c)) c = tolower(c);

        if (c == EOF || c == '\n') {
            line_buffer[next++] = 0;
            break;
        }
        line_buffer[next++] = c;
    }

    if (c == EOF && next == 1) {
        free(line_buffer);
        line_buffer = NULL;
    }
    return line_buffer;
}

void parse_words(FILE* input_file, void* ds, word_processor process_fn) {
    char *line, *word;
    while ((line = read_line(input_file))) {
        while (1) {
            word = strtok(line, ",.!? \n");
            line = NULL;

            if(word == NULL) break;

            process_fn(ds, word);
        }
        free(line);
    }
}

void print_ht(dht_table* ht, unsigned int k) {
    if (ht->count < k) k = ht->count;
    for (int i = 0; i < k; i++) {
        printf("Slovo: %s, cetnost: %d\n", ht->entries[i]->key, ht->entries[i]->value);
    }
}

void solve_ht(char* filename, word_processor wp, unsigned int k) {
    dht_table* hash_table = dht_init_hash_table(127);
    FILE* input_file = open_input_file(filename);

    parse_words(input_file, (dht_table*)hash_table, wp);
    qsort(hash_table->entries, hash_table->size, sizeof(dht_entry*), dht_comp);
    
    print_ht(hash_table, k);
}

// void fill_array(char** arr, unsigned int size, dht_table* ht) {

// }

// void solve_arr(char* filename, word_processor wp, unsigned int k) {
//     dht_table* hash_table = dht_init_hash_table(127);
//     char** arr;
//     FILE* input_file = open_input_file(filename);

//     parse_words(input_file, (dht_table*)hash_table, wp); 
//     arr = malloc(hash_table->max_value * (sizeof(char*)));

// }

int main(int argc, char** argv) {
    if(argc < 4) {
        printf("Nedostatek parametru!");
        exit(EXIT_FAILURE);
    }

    int k_words;
    char* filename = NULL;
    word_processor wp = insert_into_ht;

    if (argv[1] && argv[2] && argv[3] && strcmp(argv[1], "-i") == 0) {
        filename = malloc(strlen(argv[2])*sizeof(char));
        strcpy(filename, argv[2]);

        k_words = atoi(argv[3]);
    } else {
        printf("Nevhodne, nebo chybejici parametry!\n");
        exit(EXIT_FAILURE);
    }

    solve_ht(filename, wp, k_words);
    getchar();
    return 0;
}