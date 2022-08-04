#include <stdio.h>
#include <stdbool.h>
#include <math.h>


int main(int argc, char** argv)
{
    // POINTERS
    // every 1 byte in memory has an address
    int a; // Compiler allocates a size of a data type in the memory (32 bits -> 4 bytes) 
    // Every computer has a lookup table with all memory informations (a is at 0x202 for example)
    // TODO this is in CPU register maybe?? When talking about stack memory

    int* p; // Pointer is ALSO stored in memory!! And also takes the data type-size of memory!!!

    p = &a; // & - reference operator -> accessing an address of a

    *p = 10; // * - dereference operator -> accessing a VALUE of p (actually of a)

    // printf(p) will print AN ADDRESS of A, because it is a pointer
    // printf(&p) will print AN ADDRESS of P, because it is a reference to a pointer
    // printf(*p) will print A VALUE of A, because it is a pointer dereference
    // With int a; it will actually print a garbage value, because a is unitialized and the system
    // just gave it a random memory address which can contain some (even previously used) random stuff

    // POINTER ARITHMETIC
    // print(p + 1) -> it adds a new BYTE to the pointer (1 unit of the size of data type)
    // e.g. (p + sizeof(int))
    // e.g. if p = 2002 then p + 1 = 2006
    // this can return a corrupted memory

    // INTERESTINGLY a[5] == 5[a] works! Thanks to pointer arithmetic definition
    // e.g. *(a + 5) == *(5 + a) -> adding is commutative

    // POINTER TYPES
    // int* -> int
    // char* -> char
    // Why strong types? -> we use dereferencing to access / modify the value and the datatypes
    // are of different byte sizes

    // INT A -------------
    // BYTE 3           BYTE 2          BYTE 1          BYTE 0
    // 00000000         00000000        00000100        00000001
    // ^
    // The left most is a SIGN bit (positive or negative)
    // Rest 31 bits for the value
    // The POINTER to this int is and address of BYTE 0
    // print(*p) => "we need to access 4 bytes starting from address X" -> because it is an INT

    // We can use typecast to cast pointer types
    // e.g. int *p = &A, char *c
    // c = (char*)p

    // print(*c) will print 1 -> sizeof(char) = 1 byte and the 1st byte of A is 00000001 (1 in decimal)

    //VOID POINTER
    void *v;
    // we CANNOT dereference *v !!!
    // v = p
    // v = c
    // Are both OK since void is a "generic" pointer
    // we CANNOT do pointer arithmetic with void pointer (it has no size -> what to add??) !!!

    // POINTER TO POINTER
    // Storing a memory address of another pointer
    // int **q = &p (a POINTER to a POINTER)
    // dereferencing -> print( *(*q))
    // int ***r = &q -> r cannot store &p !!

    // POINTER as a function argument
    // f(int a) -> a is passed by VALUE (it is copied) -> therefore mutation of a is forgotten when fce returns
    // f(int *a) -> a is passed by REFERENCE (pointer) -> mutation of a will be propagated (*a = 10)

    // STACK, STATIC/GLOBAL, CODE(TEXT) memory segments are FIXED (given from the OS) when the program starts !!
    // Only Heap can be dynamically grown / shrank (tho the size of Stack can be increased by setting a OS command)


    // Every function gets a chunk of Stack based on it parameters, return address, etc. called a STACK FRAME
    // When main() gets invoked, it also gets a stack frame

    // POINTERS AND ARRAYS
    // int A[5] -> 5 consecutive chunks of memory (arrays are contiguous) !!!
    // A = pointer to the FIRST ELEMENT
    // print(A) -> address, print(*A) -> value of 1st element (index 0 - A[0], ith value - A[i] or *(A + i))
    // 5 x 4 bytes = 20 bytes
    // So with pointer arithmetic we can get ELEMENTS of the ARRAY
    // int *p = A, p + 1 -> 2nd array element
    //              p + 2 -> 3rd array element
    // BUT we CANNOT do pointer arithmetic on an array !!! A + 1 -> compile error

    // ARRAY AS A FUNCTION ARGUMENT
    // fce(int A[]) -> A is basically an int (pointer) so only FIRST 8 BYTES are copied
    // Compiler basically creates a pointer and copies the address of the first element

    // !!! This means we need to pass the size to the arrays when we want to work with it !!!

    // STRINGS
    // Arrays of chars -> size of array >= no. of chars in string + 1
    // --> "John", size >= 5
    // The +1 memory slot is for ASCII NULL CHARACTER '\0' to determine the END of the string
    // -> STRING IN C HAS TO BE NULL-TERMINATED, it is stored implicitly
    // e.g. char str[20] = "John" -> strlen(str) = 4, sizeof(str) = 5 - because of null termination

    // char c[20] = "hello" => this is compiled (and stored) on the stack in the array's address space
    // char* c = "Hello" => this will be compiled as a CONST STRING LITERAL, most likely stored in the text memory chunk
    //  ==> meaning it cannot be modified !!!
    // c[0] = 'K' -> AccessViolation error


    // CHARACTER ARRAYS AND POINTERS
    // We can iterate with pointer arithmetic untill we reach '\0' e.g. a null pointer
    // fce(char* c -> this is a pointer to a string array) {
    // fce(char c[] -> this is equivalent)
    //     while (*c != '\0') {
    //         printf(*c);
    //     }
    //     c++;
    // }
    // to enforce NON-MODIFIABLE string (e.g. for reading) just use fce(CONST char* c){}

    // MULTI-DIMENSIONAL ARRAYS
    int B[2][3];
    // Consecutive memory blocks of 1-D arrays
    // int *p = B will be compilation error -> B is of type pointer to int[3] array, not pointer to int  
    // int (*p)[3] = B is correct
    
    // B or &B[0] returns address to the start of the first array
    // *B or B[0] returns pointer to the first element of the first array -> &B[0][0]
    // B + 1 moves to the seconds inner array
    // *(B + 1) or B[1] returns a pointer to the first element of the second inner array
    // *(B + 1) + 2 returns a pointer to the 3rd element of the second inner array => B[1]+2 or &B[1][2]
    // *(*B + 1) returns a value of the second element of the first inner array
    // *(*(B + 1)) returns a value of the first element of the second inner array

    // B[i][j] = *(B[i] + j)
    //         = *(*(B + i) + j)

    return 0;
}
