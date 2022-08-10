CC = gcc
CFLAGS = -Wall
EXEC = hw2
CFILES = hw2.c

all: $(EXEC)

$(EXEC): $(CFILES)
	gcc $(CFLAGS) -o $(EXEC) $(CFILES)

run: $(EXEC)
	./$(EXEC) -nsn -f output.txt 10 10

clean:
	rm -f *.o $(EXEC)