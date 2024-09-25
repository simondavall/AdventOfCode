#include <stdio.h>
#include <stdlib.h>
#include "helper.h"
#define INPUT_FILE "sample"

int part1(char *lines[], int line_count);
int part2(char *lines[], int line_count);

int main(void){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, INPUT_FILE);

  for(int j = 0; j < line_count; j++)
    printf("Line %2d: '%s'\n", j, lines[j]);

  printf("Part 1 Result: %d\n", part1(lines, line_count));
  printf("Part 2 Result: %d\n", part2(lines, line_count));

  dispose_read_file(lines, line_count);

  return EXIT_SUCCESS;
}

int part1(char *lines[], int line_count){

  return 0;
}

int part2(char *lines[], int line_count){

  return 0;
}

