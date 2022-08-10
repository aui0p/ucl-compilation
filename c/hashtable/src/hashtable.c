#include "hashtable.h"
#include "hash.h"
#include "../../prime/prime.h"

#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <math.h>

static dht_entry DHT_DELETED_MARK = { NULL, -1 };
#define BASE_SIZE_PRIME 127

static dht_entry* dht_create_entry(const char* k, const int v )
{
    dht_entry* i = (dht_entry*)malloc(sizeof(dht_entry));
    i->key = strdup(k);
    i->value = v;
    return i;
}

dht_table* dht_init_hash_table(const int base_size)
{
    dht_table* dht = (dht_table*)malloc(sizeof(dht_table));
    if (!dht) {
        printf("Nedostatek pameti!");
        exit(EXIT_FAILURE);
    }
    
    dht->size = next_consecutive_prime(base_size);
    dht->count = 0;
    dht->max_value = 0;
    dht->entries = calloc((size_t)dht->size, sizeof(dht_entry*));
    if (!dht->entries) {
        printf("Nedostatek pameti!");
        exit(EXIT_FAILURE);
    }

    return dht;
}

static void dht_delete_entry(dht_entry* e) {
    free(e->key);
    free(e);
}

void dht_delete_hash_table(dht_table* t) {
    for (size_t i = 0; i < t->size; i++) {
        dht_entry* it = t->entries[i];

        if (it != NULL && it != &DHT_DELETED_MARK) {
            dht_delete_entry(it);
        }
    }
    
    free(t->entries);
    free(t);
}

static int dht_load(dht_table* ht) {
    return ((double)ht->count / ht->size) * 100;
}

static void dht_resize(dht_table* ht, const int base_size) {
    dht_table* new_ht = dht_init_hash_table(base_size);

   for (int i = 0; i < ht->size; i++) {
        dht_entry* e = ht->entries[i];
        if (e != NULL && e != &DHT_DELETED_MARK) {
            dht_insert(new_ht, e->key, e->value);
        }
    }

    ht->count = new_ht->count;

    const int tmp_size = ht->size;
    ht->size = new_ht->size;
    new_ht->size = tmp_size;

    dht_entry** tmp_entries = ht->entries;
    ht->entries = new_ht->entries;
    new_ht->entries = tmp_entries;

    dht_delete_hash_table(new_ht);
}

static void dht_grow(dht_table* ht) {
    const int base_size = ht->size*2;
    dht_resize(ht, base_size);
}

static void dht_shrink(dht_table* ht) {
    const int base_size = ht->size/2;
    dht_resize(ht, base_size);
}

static void update_max(dht_table* ht, unsigned int v) {
    if (ht->max_value < v) ht->max_value = v;
}

void dht_insert(dht_table* ht, const char* k, const int v) {
    const int load = dht_load(ht);
    if (load > 70) dht_grow(ht);

    dht_entry* e = dht_create_entry(k, v);
    int idx = dht_get_hash_index(e->key, ht->size, 0);
    dht_entry* item = ht->entries[idx];
    
    int n_collisions = 1;
    while (item != NULL && item != &DHT_DELETED_MARK) {
        if (strcmp(item->key, k) == 0) {
            dht_delete_entry(item);
            ht->entries[idx] = e;

            update_max(ht, e->value);
            return;
        }

        idx = dht_get_hash_index(e->key, ht->size, n_collisions);
        item = ht->entries[idx];
        n_collisions++;
    }
    ht->entries[idx] = e;
    ht->count++;
    update_max(ht, e->value);
}

int dht_find(dht_table* ht, const char* k) {
    int idx = dht_get_hash_index(k, ht->size, 0);
    dht_entry* e = ht->entries[idx];

    int n_collisions = 1;
    while (e != NULL && e != &DHT_DELETED_MARK) {
        if (strcmp(e->key, k) == 0) return e->value;

        idx = dht_get_hash_index(k, ht->size, n_collisions);
        e = ht->entries[idx];
        ++n_collisions;
    }
    return -1;
}

// TODO get entry

void dht_remove_entry(dht_table* ht, const char* k) {
    const int load = dht_load(ht);
    if (load < 15) dht_shrink(ht);

    int idx = dht_get_hash_index(k, ht->size, 0);
    dht_entry* e = ht->entries[idx];

    int n_collisions = 1;
    while (e != NULL) {
        if ((strcmp(e->key, k) == 0) && (e != &DHT_DELETED_MARK)) {
            dht_delete_entry(e);
            ht->entries[idx] = &DHT_DELETED_MARK;
            ht->count--;
            return;
        }
        idx = dht_get_hash_index(k, ht->size, n_collisions);
        e = ht->entries[idx];
        ++n_collisions;
    }
    ht->count--;
}
