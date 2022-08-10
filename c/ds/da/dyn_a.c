#include "dyn_a.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>

static char* cpy(const char *value) {
    char *value_copy = malloc(sizeof(char*));
    memcpy(value_copy, value, sizeof(char*));

    return value_copy;
}

static bool in_bounds(const unsigned size, const unsigned index) {
    if (size >= 0 && index < size)
        return true;

    printf("Index [%d] je mimo rozsah pole!\n", index);
    return false;
}

dyn_a* dyn_a_init_dynamic_array(unsigned int base_size) {
    dyn_a *da = malloc(sizeof(dyn_a));
    da->items = calloc(base_size, sizeof(char *));
    da->capacity = base_size;

    return da;
}

static void dyn_a_delete_items(dyn_a* da) {
    for (size_t i = 0; i < da->size; i++) {
        if (da->items[i] == NULL) continue;
        //free(da->items[i]);
    }

    //free(da->items);
}

void dyn_a_delete_array(dyn_a* da) {
    dyn_a_delete_items(da);
    free(da);
} 

void dyn_a_add(dyn_a* da, const char* value) {
    if (da->size >= da->capacity) {
        char** newItems = realloc(da->items, (da->capacity <<= 1) * sizeof(char*));
        //dyn_a_delete_items(da);
        //free(da->items);
        da->items = newItems;
    }

    void *copy_value = cpy(value);
    da->items[da->size++] = copy_value;
}

void* dyn_a_update(dyn_a* da, const char* value, const unsigned int index) {
    if (!in_bounds(da->size, index)) return NULL;

    free(da->items[index]);
    void *copy_value = cpy(value);
    da->items[index] = copy_value;
}

char* dyn_a_get(dyn_a* da, const unsigned int index) {
    if (!in_bounds(da->size, index)) return NULL;

    return da->items[index];
}

void dyn_a_remove(dyn_a* da, const unsigned int index) {
    if (!in_bounds(da->size, index)) return;

    for (int i = index; i < da->size; i++) {
        da->items[i] = da->items[i+1];
    }

    da->size--;

    free(da->items[da->size]);
}
