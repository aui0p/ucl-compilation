CC = gcc
CFLAGS = -Wall
EXEC = sntc
CFILES = sentence_sort.c ../ds/da/dyn_a.c

all: $(EXEC)

$(EXEC): $(CFILES)
	gcc $(CFLAGS) -o $(EXEC) $(CFILES)

run: $(EXEC)
	./$(EXEC) -i input.txt

clean:
	rm -f *.o $(EXEC)