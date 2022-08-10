#include <stdlib.h>
#include <math.h>
#include <string.h>

// TODO would larger primes help pathology (check school mat)?
static const int DHT_PRIME1 = 193;
static const int DHT_PRIME2 = 389;


int dht_hash(const char* s, const int prime, const int dht_num_entries) {
    long hash = 0;
    const int s_size = strlen(s);
    
    for (size_t i = 0; i < s_size; i++) {
        hash += (long)(pow(prime, s_size - (i+1)) * s[i]);
    }
    
    hash = hash % dht_num_entries;
    return abs((int)hash);
}

// double hashing
int dht_get_hash_index(const char* s, const int dht_num_entries, const int n_collisions) {
    const int hash_a = dht_hash(s, DHT_PRIME1, dht_num_entries);
    // TODO defer creating the second hash until needed
    const int hash_b = dht_hash(s, DHT_PRIME2, dht_num_entries);
    return (hash_a + (n_collisions * (hash_b + 1))) % dht_num_entries;
}