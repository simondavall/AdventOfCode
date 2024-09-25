#include <ctype.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "helper.h"

// This function reads lines from a specified file into
// the array provided. After use the dispose_read_file function
// to free the memory allocated.
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
void dispose_read_file(char *str[], int count){
  for (int i = 0; i < count; i++) {
    free(str[i]);
  }
}

/*******************************************************************/
/* contains  - Returns the number of occurances of a string (item) */
/*             contained in the specified string array (source)    */
/*             If zero is returned the string was not found        */
/*******************************************************************/
int contains(char *source[], int count, char *item){
  if (source == NULL)
    return 0;

  int found = 0;
  while(count > 0)
    if(strcmp(item, source[--count]) == 0)
      found++;

  return found;
}

// Split functions
//
// Splits a string by the specified delimitor.
// Use the dispose_split_result function after use, to deallocate memory.
struct split_result split(char *str, const char *delim){
  struct split_result *result = NULL;
  char *token, *item, **arr = NULL, **ins;
  int count = 0;

  // todo: revisit this as the original string is changed during this
  // function call.
  token = strtok(str, delim);

  while(token != NULL){
    count++;

    item = NULL; // Needed
    // todo: This memory is not deallocated
    item = m_alloc(item, strlen(token) + 1, "Unable to allocate memory to item in split");
    strcpy(item, token);
    
    arr = m_alloc(arr, sizeof(size_t) * count, "Unable to allocate memory to array during split");
    ins = arr + (count - 1);
    *ins = item;
    token = strtok(NULL, delim);
  }
 
  result = m_alloc(result, sizeof(struct split_result), "Unable to allocate memory for split result");
  result->split = arr;
  result->count = count;

  return *result;
}

void dispose_split_result(struct split_result result){
  char **p, *str;
  p = result.split;
  while(p < result.split + result.count){
    str = *p;
    p++;
    free(str);
  }
  free(result.split);
}

// Trim functions
char *ltrim(char *str)
{
  char *prev, *cur;

  if (str != NULL){
    cur = prev = str;
    while(isspace(*cur)) cur++;
    while(*cur != '\0') *prev++ = *cur++;
    *prev = *cur;
  }
  
  return str;
}

char *rtrim(char *s)
{
  if (s != NULL){
    char* back = s + strlen(s);
    while(isspace(*--back));
    *(back+1) = '\0';

  }
  return s;
}

char *trim(char *s)
{
    return rtrim(ltrim(s)); 
}

void *m_alloc(void *ptr, size_t size, const char *err_msg){
  if (ptr == NULL){
    ptr = malloc(size);
  }
  else{
    ptr = realloc(ptr, size);
  }

  if (ptr == NULL){
    printf("%s", err_msg);
    exit(EXIT_FAILURE);
  }
 
  return ptr;
}
