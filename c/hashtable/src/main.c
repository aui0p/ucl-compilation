#include <stdio.h>
#include <stdlib.h>

#include "hashtable.h"

int comp(const void* a, const void* b) {
    dht_entry* e1 = *(dht_entry**)a;
    dht_entry* e2 = *(dht_entry**)b;
    if(!e1 && !e2) return 0;
    if(!e1) return 1;
    if(!e2) return -1;
    
    return e2->value - e1->value;
}

int main(int argc, char** argv) {
    dht_table* dht = dht_init_hash_table(3);
    dht_insert(dht, "slovo 1", 5);

    dht_insert(dht, "slovo 2", 3);
    dht_insert(dht, "slovo 3", 2);
    dht_insert(dht, "slovo 4", 12);
    dht_insert(dht, "slovo 5", 5);
    dht_insert(dht, "slovo 1", 7);
    dht_insert(dht, "slovo 1", 5);
    dht_insert(dht, "slovo 1", 3);
    dht_insert(dht, "slovo 6", 10);
    dht_insert(dht, "slovo 7", 1);
    dht_remove_entry(dht, "slovo 2");

    dht_insert(dht, "slovo 2", 3);

    printf("size %d, count %d\n", dht->size, dht->count);
    int max = INT32_MAX;
    for (size_t i = 0; i < dht->size; i++)
    {
        dht_entry* e = dht->entries[i];
        if(!e) continue;
        printf("key: %s, val: %d\n", e->key, e->value);
    }
    qsort(dht->entries, dht->size, sizeof(dht_entry*), comp);
    // printf("%d\n", dht_get_entry(dht, "slovo 1"));
    // printf("%d\n", dht_get_entry(dht, "slovo 2"));
    // printf("%d\n", dht_get_entry(dht, "slovo 3"));
    // printf("%d\n", dht_get_entry(dht, "slovo 4"));
    // printf("%d\n", dht_get_entry(dht, "slovo 5"));
    printf("------- after sort -------\n");
    for (size_t i = 0; i < dht->size; i++)
    {
        dht_entry* e = dht->entries[i];
        if(!e) { printf("null\n"); continue; };
        printf("klic: %s, val: %d\n", e->key, e->value);
    }


    dht_delete_hash_table(dht);

    return 0;
}