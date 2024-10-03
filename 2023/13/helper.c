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
  int str_len;
  
  fptr = fopen(filepath, "rb");
  if (fptr == NULL){
    printf("Error opening file: %s", filepath);
    exit(EXIT_FAILURE);
  }

  while ((str_len = (int)getline(&line, &len, fptr)) != -1) {
    line[str_len - 1] = '\0';
    // if (strcmp(line, "\0") == 0) // ignore empty strings
    //   continue;
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
int contains(char *source[], int arr_len, char *item){
  if (source == NULL)
    return 0;

  int found = 0;
  while(arr_len > 0)
    if(strcmp(item, source[--arr_len]) == 0)
      found++;

  return found;
}

/*******************************************************************/
/* contains_char  - Returns the number of occurances of a char     */
/*                  contained in the string array                  */
/*                  Returns zero if the char was not found.        */
/*******************************************************************/
int contains_char(char source[], int arr_len, char ch){
  if (source == NULL)
    return 0;
  
  int i, found = 0;
  for(i = 0; i < arr_len; i++)
    if(source[i] == ch) found++;

  return found;
}

/*******************************************************************/
/* str_contains_char  - Returns the number of occurances of a char */
/*                      contained in the string (\0 terminated)    */
/*                      Returns zero if the char was not found.    */
/*******************************************************************/
int str_contains_char(char *str, char ch){
  if (str == NULL)
    return 0;
  int found = 0;

  char *p = str;
  while(*p != '\0'){
    if (*p == ch) found++;
    p++;
  }

  return found;
}

int str_contains_str(char *str, char *find){
  printf("Check for NULLs\n");
  if (str == NULL || find == NULL)
    return 0;
  printf("Check for NULLs - complete\n");
  printf("Check lengths\n");
  size_t str_len = strlen(str);
  size_t find_len = strlen(find);
  if (str_len == 0 || find_len == 0 || find_len > str_len)
    return 0;
  printf("Check lengths - complete\n");
  int i, found = 0;
  char *p = str;
  for (p = str, i = 0; p < str + str_len; i++, p++){
    printf("Check char[%d]\n", i);
    if (p == find){
      printf("Found char, checking other chars.\n");
      // check other chars
    

    }
    printf("Move to next char in str.\n");
    p++;
  }

  return found;
}

// Split functions
//
// Splits a string by the specified delimitor.
// Returns an array of \0 terminated strings
//
// Use the dispose_split_result function after use, to deallocate memory.
struct split_result split(char *str, const char *delim){
  struct split_result *result = NULL;
  char *token, *item, **arr = NULL, **ins;
  int count = 0;

  token = strtok(str, delim);

  while(token != NULL){
    count++;

    item = NULL; // Needed
    item = m_alloc(item, strlen(token) + 1, "Unable to allocate memory to item in split");
    strcpy(item, token);
    item[strlen(token)] = '\0';
    
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

  if (str != NULL && strlen(str) > 0){
    cur = prev = str;
    while(isspace(*cur)) cur++;
    while(*cur != '\0') *prev++ = *cur++;
    *prev = *cur;
  }
  
  return str;
}

/************************************************************/
/* ltrim_char - Trims both specified char AND whitespace    */
/*              from the beginning of the string.           */
/************************************************************/
char *ltrim_char(char *str, char ch){
  char *prev, *cur;

  if (str != NULL && strlen(str) > 0){
    cur = prev = str;
    while(isspace(*cur) || *cur == ch ) cur++;
    while(*cur != '\0') *prev++ = *cur++;
    *prev = *cur;
  }
  
  return str;
}

char *rtrim(char *str)
{
  if (str != NULL && strlen(str) > 0){
    char* back = str + strlen(str) - 1;
    while(isspace(*back) && back >= str) back--;
    *(back+1) = '\0';
  }
  return str;
}

/************************************************************/
/* rtrim_char - Trims both specified char AND whitespace    */
/*              from the end of the string.                 */
/************************************************************/
char *rtrim_char(char *str, char ch)
{
  if (str != NULL && strlen(str) > 0){
    char* back = str + strlen(str) - 1;
    while((isspace(*back) || *back == ch) && back >= str) back--;
    *(back+1) = '\0';
  }
  return str;
}

char *trim(char *s)
{
    return rtrim(ltrim(s)); 
}

/**************************************************************/
/* trim_char - Trims both specified char AND whitespace       */
/*             from the beginning and the end of the string.  */
/**************************************************************/
char *trim_char(char *str, char ch)
{
    return rtrim_char(ltrim_char(str, ch), ch); 
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
