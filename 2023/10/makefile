CC=gcc
CFLAGS=-Wall -Wextra -pedantic -std=c11
INPUTS=sample01 sample02 sample03 sample04 sample05 input

.PHONY: test
test: aoc $(INPUTS)
	./aoc $(INPUTS)

aoc: program.o helper.o
	$(CC) -o aoc program.o helper.o $(CFLAGS)

program.o: program.c helper.h
	$(CC) -c program.c $(CFLAGS)

helper.o: helper.c helper.h
	$(CC) -c helper.c $(CFLAGS)
