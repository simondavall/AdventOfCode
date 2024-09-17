#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"

struct gear_part {
  struct gear_part *next;
  int row;
  int col;
  int count;
  int numbers[2];
} *parts_list = NULL;

void build_number(int *number, char digit);
void check_part(char *grid[], int row, int *start, int *len, long *tally, int *number);
void check_gear_part(char *grid[], int row, int *start, int *len, int *number);

long part1(char *lines[], int line_count);
long part2(char *lines[], int line_count);

int max_row_index = 0;
int max_col_index = 0;

int main(void){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, "input");

  max_row_index = line_count - 1;
  max_col_index = strlen(lines[0]) - 1;

  for(int j = 0; j < line_count; j++)
    printf("Line %2d: '%s'\n", j, lines[j]);

  printf("Part 1 Result: %ld\n", part1(lines, line_count));
  printf("Part 2 Result: %ld\n", part2(lines, line_count));

  dispose_read_file(lines, line_count);

  return EXIT_SUCCESS;
}

long part1(char *lines[], int line_count){
  int i, j, num_pos = 0, number = 0, start = -1, len = 0;
  long tally = 0;
  int rows = line_count;
  int cols = strlen(lines[0]);

  for(i = 0; i < rows; i++){
    for(j = 0; j < cols; j++){
      if (isdigit(lines[i][j])){
        if (start == -1)
          start = j;
        
        len++;
        build_number(&number, lines[i][j]);
      }
      else{
        if (number > 0){
          check_part(lines, i, &start, &len, &tally, &number);
        }
      }
    }
    if (number > 0){
      check_part(lines, i, &start, &len, &tally, &number);
    }
  }

  return tally;
}

long part2(char *lines[], int line_count){
  int i, j, num_pos = 0, number = 0, start = -1, len = 0;
  long tally = 0;
  int rows = line_count;
  int cols = strlen(lines[0]);

  for(i = 0; i < rows; i++){
    for(j = 0; j < cols; j++){
      if (isdigit(lines[i][j])){
        if (start == -1)
          start = j;
        len++;
        build_number(&number, lines[i][j]);
      }
      else{
        if (number > 0){
          check_gear_part(lines, i, &start, &len, &number);
        }
      }
    }
    if (number > 0){
      check_gear_part(lines, i, &start, &len, &number);
    }
  }

  struct gear_part *result, *temp;
  for (result = parts_list; result != NULL; result = result->next){
    if (result->count == 2){
      tally += result->numbers[0] * result->numbers[1];
    }
  }

  // dispose
  while(parts_list){
    temp = parts_list;
    parts_list = parts_list->next;
    free(temp);
  }

  return tally;
}

// Functions

void build_number(int *number, char digit){
  if (*number > 0)
    *number *= 10;

  *number += digit - '0';
}

bool is_symbol(char c){
  return !isdigit(c) && c != '.';
}

bool is_engine_part(char *grid[], int row, int col, int len){
  int i, j, prev, next;

  if (col > 0 && is_symbol(grid[row][col - 1]))
    return true;
  if ((col + len) < max_col_index && is_symbol(grid[row][col + len]))
    return true;

  prev = row - 1;
  for(i = col - 1; i <= col + len; i++){
    if (prev >= 0 && i >= 0 && i <= max_col_index){
      if (is_symbol(grid[prev][i]))
        return true;
    }
  }
  next = row + 1;
  for(i = col - 1; i <= col + len; i++){
    if (next <= max_row_index && i >= 0 && i <= max_col_index){
      if (is_symbol(grid[next][i]))
        return true;
    }
  }

  return false;
}

void check_part(char *grid[], int row, int *start, int *len, long *tally, int *number){
  if (is_engine_part(grid, row, *start, *len)){
    *tally += *number;
  }
  *number = 0;
  *start = -1;
  *len = 0;
}

struct gear_part *find_gear_part(int row, int col){
  struct gear_part *p;

  for (p = parts_list; p != NULL && !(p->row == row && p->col == col); p = p->next);

  if (p != NULL && p->row == row && p->col == col)
    return p;

  return NULL;
}

void add_to_list(int number, int row, int col){

  struct gear_part *gear_part_node = find_gear_part(row, col);
  if (gear_part_node == NULL){
    gear_part_node = m_alloc(gear_part_node, sizeof(struct gear_part), "Unable to allocate memory for gear part node");
    gear_part_node->col = col;
    gear_part_node->row = row;
    gear_part_node->count = 0;
    gear_part_node->numbers[0] = 0;
    gear_part_node->numbers[1] = 0;
    gear_part_node->next = parts_list;
    parts_list = gear_part_node;
  }

  gear_part_node->numbers[gear_part_node->count++] = number;
}

void register_gear_part(char *grid[], int row, int col, int len, int number){
  int i, j, prev, next;

  if (col > 0 && grid[row][col - 1] == '*')
    add_to_list(number, row, col - 1);
  if ((col + len) < max_col_index && grid[row][col + len] == '*')
    add_to_list(number, row, col + len);

  prev = row - 1;
  for(i = col - 1; i <= col + len; i++){
    if (prev >= 0 && i >= 0 && i <= max_col_index){
      if (grid[prev][i] == '*')
        add_to_list(number, prev, i);
    }
  }
  next = row + 1;
  for(i = col - 1; i <= col + len; i++){
    if (next <= max_row_index && i >= 0 && i <= max_col_index){
      if (grid[next][i] == '*')
        add_to_list(number, next, i);
    }
  }
}

void check_gear_part(char *grid[], int row, int *start, int *len, int *number){
  register_gear_part(grid, row, *start, *len, *number);
  *number = 0;
  *start = -1;
  *len = 0;
}
