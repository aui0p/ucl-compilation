#include <stdlib.h>

typedef struct dht_entry {
    char* key;
    int value;
} dht_entry;

typedef struct dht_table {
    dht_entry** entries;
    int size;
    int count;
    unsigned int max_value;
} dht_table;

dht_table* dht_init_hash_table(const int base_size);
void dht_delete_hash_table(dht_table*);

void dht_insert(dht_table* ht, const char* key, const int value);
int dht_find(dht_table* ht, const char* key);
void dht_remove_entry(dht_table* ht, const char* key);