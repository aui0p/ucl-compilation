#include <stdio.h>
#include <stdlib.h>

typedef struct str {
    int* vals;
} st;

typedef struct str_p {
    struct str str;
} st_p;


int main(void) {
    st* str = (st*)malloc(sizeof(st));
    str->vals = (int*)calloc(10, sizeof(int));
    for (size_t i = 0; i < 10; i++)
    {
        // Am I actually retarded?
        int val = str->vals[i];
        int* ptr = &str->vals[i];
        printf("value: %d\n", val);
        printf("address: %d\n", ptr);
    }
    free(str->vals);
    free(str);
    
    scanf("%s");
    
}