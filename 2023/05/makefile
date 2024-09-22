CC=gcc
CFLAGS=-Wall -Wextra -Wpedantic 

aoc: program.o helper.o day05.o
	$(CC) -o aoc program.o helper.o day05.o $(CFLAGS)

program.o: program.c helper.h day05.h
	$(CC) -c program.c $(CFLAGS)

helper.o: helper.c helper.h
	$(CC) -c helper.c $(CFLAGS)

day05.o: day05.c day05.h
	$(CC) -c day05.c $(CFLAGS)
