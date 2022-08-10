CC = gcc
CFLAGS = -Wall
EXEC = hw1
CFILES = hw1.c

all: $(EXEC)

$(EXEC): $(CFILES)
	gcc $(CFLAGS) -o $(EXEC) $(CFILES)

run: $(EXEC)
	./$(EXEC)

clean:
	rm -f *.o $(EXEC)