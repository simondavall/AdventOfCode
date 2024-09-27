#include <stdio.h>
#include <stdlib.h>
#include "helper.h"
#define INPUT_FILE "sample"

long part1(char *lines[], int line_count){

  return 0;
}

long part2(char *lines[], int line_count){

  return 0;
}

void solve_file(char *file_path){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, file_path);

  for(int j = 0; j < line_count; j++)
    printf("Line %2d: '%s'\n", j, lines[j]);

  printf("Part 1 Result: %ld\n", part1(lines, line_count));
  printf("Part 2 Result: %ld\n", part2(lines, line_count));

  dispose_read_file(lines, line_count);
}

int main(int argc, char *argv[]){
  for (int i = 1; i < argc; ++i) {
    solve_file(argv[i]);
  }
  return EXIT_SUCCESS;
}
