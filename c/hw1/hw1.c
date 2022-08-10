#include <stdio.h>
#include <stdlib.h>
#include <math.h>

void get_number(int* num) {
    printf("Zadejte druhe cislo:\n");
    scanf("%i", num);
}

double add(double a, double b) {
    return a + b;
}

double substract(double a, double b) {
    return a - b;
}

double multiply(double a, double b) {
    return a * b;
}

double divide(double a, double b) {
    if (a == 0 || b == 0) {
        printf("Pokus o deleni nulou!");
        exit(1);
    }
    return a / b;
}

int main(int argc, char** argv) {
    char operand;
    double result;
    
    printf("%s", "Zadejte prvni cislo:\n");
    scanf("%lf", &result);

     while (1) {
        int number;
        printf("%s", "Zadejte operaci:\n");
        scanf("%s", &operand);

        switch (operand) {
            case '+':
                get_number(&number);
                result = add(result, number);
                break;

            case '-':
                get_number(&number);
                result = substract(result, number);
                break;

            case '*':
                get_number(&number);
                result = multiply(result, number);
                break;

            case '/':
                get_number(&number);
                result = divide(result, number);
                break;

            case 's':
                result = sin(result);
                break;

            case 'c':
                result = cos(result);
                break;

            case 't':
                result = tan(result);
                break;

            case 'o':
                result = sqrt(result);

            case 'q':
            case 'Q':
                return 0;
                break;

            default:
                printf("Nepodporovana operace!\n");
                return -1;
                break;
        }

        printf("Vysledek je: %f \n", result);
    }
    return 0;
}
