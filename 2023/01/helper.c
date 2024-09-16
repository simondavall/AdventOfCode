#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "helper.h"

// This function reads lines from a specified file into
// the array provided. After use the dispose method
// below should be called in order to free the memory
// allocated.
int read_file(char *str[], const char *filepath){
  FILE *fptr;
  int line_count = 0;
  void *str_ptr;
  char *line = NULL;
  size_t len = 0;
  ssize_t str_len;
  
  fptr = fopen(filepath, "rb");
  if (fptr == NULL){
    printf("Error opening file: %s", filepath);
    exit(EXIT_FAILURE);
  }

  while ((str_len = getline(&line, &len, fptr)) != -1) {
    line[str_len - 1] = '\0';
    if (strcmp(line, "\0") == 0) // ignore empty strings
      continue;
    str_ptr = malloc(str_len);
    if (str_ptr == NULL){
      printf("Error allocating memory during read line.\n");
      exit(EXIT_FAILURE);
    }
    str[line_count] = str_ptr;
    strncpy(str[line_count++], line, str_len);
  }

  fclose(fptr);
  if(line)
    free(line);
  return line_count;
}

// This function should be called in order to free memory
// allocatied during the read_file function above.
void dispose(char *str[], int count){
  for (int i = 0; i < count; i++) {
    free(str[i]);
  }
}
