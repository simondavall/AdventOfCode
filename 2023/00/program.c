#include <stdio.h>
#include <stdlib.h>
#include "helper.h"

int main(void){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, "sample");

  for(int j = 0; j < line_count; j++)
    printf("Line %2d: '%s'\n", j, lines[j]);

  dispose(lines);

  return EXIT_SUCCESS;
}


