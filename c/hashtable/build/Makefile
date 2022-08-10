CC = gcc
CFLAGS = -Wall
EXEC = dht
CFILES = ../src/main.c ../src/hashtable.c ../src/hash.c ../../prime/prime.c

all: $(EXEC)

$(EXEC): $(CFILES)
	gcc $(CFLAGS) -o $(EXEC) $(CFILES)

run: $(EXEC)
	./$(EXEC)

clean:
	rm -f *.o $(EXEC)