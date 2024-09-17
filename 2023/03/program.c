#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"

int part1(char *lines[], int line_count);
int part2(char *lines[], int line_count);

int max_row_index = 0;
int max_col_index = 0;

int main(void){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, "input");

  max_row_index = line_count - 1;
  max_col_index = strlen(lines[0]) - 1;

  for(int j = 0; j < line_count; j++)
    printf("Line %2d: '%s'\n", j, lines[j]);

  printf("Part 1 Result: %d\n", part1(lines, line_count));
  printf("Part 2 Result: %d\n", part2(lines, line_count));

  dispose_read_file(lines, line_count);

  return EXIT_SUCCESS;
}

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

void check_part(char *grid[], int row, int *start, int *len, int *tally, int *number){
  if (is_engine_part(grid, row, *start, *len)){
    printf("Engine part found: %d\n", *number);
    *tally += *number;
  }
  *number = 0;
  *start = -1;
  *len = 0;
}

int part1(char *lines[], int line_count){
  int i, j, tally = 0, num_pos = 0, number = 0, start = -1, len = 0;
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
          // printf("Checking number: %d with detail: i: %d, start: %d, len: %d \n", number, i, start, len);
          check_part(lines, i, &start, &len, &tally, &number);
        }
      }
    }
    if (number > 0){
      check_part(lines, i, &start, &len, &tally, &number);
    }
  }

  printf("Matrix rows: %d, cols: %d\n", rows, cols);

  return tally;
}

int part2(char *lines[], int line_count){

  return 0;
}

