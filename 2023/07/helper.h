#ifndef HELPER_H
#define HELPER_H

#include <stdlib.h>
#define INIT_LINES_SIZE 1000

struct split_result{
  char **split;
  int count;
};

int read_file(char *str[], const char *filepath);
void dispose_read_file(char *str[], int count);

int contains(char *source[], int count, char *item);
int contains_char(char source[], int arr_len, char ch);
int str_contains_char(char *str, char c);

// Split functions
struct split_result split(char *str, const char *delim);
void dispose_split_result(struct split_result result);

  // Trim functions
char *ltrim(char *str);
char *rtrim(char *s);
char *trim(char *s);

// Malloc functions
void *m_alloc(void *ptr, size_t size, const char *err_msg);

#endif // !HELPER_H




