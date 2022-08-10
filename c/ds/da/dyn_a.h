#include <stdbool.h>

typedef struct dyn_a {
    char** items;
    unsigned int size;
    unsigned int capacity;
} dyn_a;

dyn_a* dyn_a_init_dynamic_array(unsigned int base_size);
void dyn_a_delete_array(dyn_a* da);

void dyn_a_add(dyn_a* da, const char* value);
void* dyn_a_update(dyn_a* da, const char* value, unsigned int index);
char* dyn_a_get(dyn_a* da, const unsigned int index);
void dyn_a_remove(dyn_a* da, const unsigned int index);