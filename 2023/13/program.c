#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"
#include "stackADT.h"
#define MAX_LEN 20

void clear_grid(char grid[][MAX_LEN]){
  int i, j;

  for(i = 0; i < MAX_LEN; i++)
    for(j = 0; j < MAX_LEN; j++)
      grid[i][j] = '\0';
}

void transpose_grid(char grid[][MAX_LEN], int grid_len){
  int i, j;
  char ch;

  for(i = 0; i < grid_len; i++)
    for(j = i; j < MAX_LEN; j++){
      ch = grid[i][j];
      grid[i][j] = grid[j][i];
      grid[j][i] = ch;
    }
}

int horizontal_reflection(char grid[][MAX_LEN], int grid_len, int orig_rows){
  int count = 0, result = 0;
  Stack s = stack_create(grid_len);

  for (int i = 0; i < grid_len; i++){
    if (!stack_is_empty(s))
      if (strcmp(stack_peek(s), grid[i]) == 0){
        char *str = stack_pop(s);
        free(str);
        count++;
        if (stack_is_empty(s)){
          if (count == orig_rows){
            break;
          }
          else{
            destroy_stack(s);
            return count;
          }
          count = 0;
        }
      }
      else
        stack_push(s, grid[i]);  
    else
      stack_push(s, grid[i]);
  }

  stack_make_empty(s);
  count = 0;

  for (int i = grid_len - 1; i >= 0; i--)
    if (!stack_is_empty(s))
      if (strcmp(stack_peek(s), grid[i]) == 0){
        char *str = stack_pop(s);
        free(str); // not used, need to free
        count++;
        if (stack_is_empty(s)){
          if (orig_rows == count + i){
            break;
          }
          else{
            destroy_stack(s);
            return count + i;
          }
          count = 0;
        }
      }
      else
        stack_push(s, grid[i]);  
    else
      stack_push(s, grid[i]);

  destroy_stack(s);
  return result;
}

int vertical_reflection(char grid[][MAX_LEN], int grid_len, int orig_cols){
  int new_length = strlen(grid[0]);
  transpose_grid(grid, MAX_LEN);
  int result = horizontal_reflection(grid, new_length, orig_cols);
  transpose_grid(grid, MAX_LEN);
  return result;
}

void switch_mirror(char grid[][MAX_LEN], int col, int row){
  if (grid[col][row] == '.')
    grid[col][row] = '#';
  else
    grid[col][row] = '.';
}

int find_alternative_values(char grid[][MAX_LEN], int grid_len, int orig_rows, int orig_cols){
  int i, j, rows, cols, width = strlen(grid[0]);

  for (i = 0; i < grid_len; i++){
    for (j = 0; j < width; j++){
      switch_mirror(grid, i, j);
      rows = horizontal_reflection(grid, grid_len, orig_rows);
      cols = vertical_reflection(grid, grid_len, orig_cols);
      if (cols > 0 && rows > 0)
        continue;
      if (rows > 0 && rows != orig_rows)
        return rows * 100;
      if (cols > 0 && cols != orig_cols)
        return cols;
      switch_mirror(grid, i, j); // return grid to orig format
    }
  }
  return 0;
}

long part1(char *lines[], int line_count){
  int i = 0, j = 0, rows, cols;
  long tally = 0;
  char grid[MAX_LEN][MAX_LEN];

  clear_grid(grid);

  for (i = 0; i < line_count; i++){
    rows = cols = 0;
    if (strcmp(lines[i], "\0") != 0){
      strcpy(grid[j++], lines[i]);
      continue;
    }

    rows = horizontal_reflection(grid, j, 0);
    if (rows == 0)
      cols = vertical_reflection(grid, j, 0);

    tally += rows * 100 + cols;
    j = 0;
    clear_grid(grid);
  }
  return tally;
}

long part2(char *lines[], int line_count){
  int i = 0, j = 0, rows, cols;
  long tally = 0;
  char grid[MAX_LEN][MAX_LEN];

  clear_grid(grid);

  for (i = 0; i < line_count; i++){
    rows = cols = 0;
    if (strcmp(lines[i], "\0") != 0){
      strcpy(grid[j++], lines[i]);
      continue;
    }

    rows = horizontal_reflection(grid, j, 0);
    if (rows == 0)
      cols = vertical_reflection(grid, j, 0);

    tally += find_alternative_values(grid, j, rows, cols);
    j = 0;
    clear_grid(grid);
  }
  return tally;
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
