CC = gcc
CFLAGS = -Wall
EXEC = wordfreq
CFILES = wordfreq.c ../hashtable/src/hashtable.c ../hashtable/src/hash.c ../prime/prime.c 

all: $(EXEC)

$(EXEC): $(CFILES)
	gcc $(CFLAGS) -o $(EXEC) $(CFILES)

run: $(EXEC)
	./$(EXEC) -i input-l.txt 15

clean:
	rm -f *.o $(EXEC)