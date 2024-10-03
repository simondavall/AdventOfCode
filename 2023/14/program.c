#include <complex.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"

int line_count;
int width;
int cache_buffer_size = 0;

struct node {
  char *content;
  struct node *next;
};

struct node *cache = NULL;

char *create_cache_key(char *grid[]){
  int i, j, buf_len = 0;

  if (cache_buffer_size == 0){
    for (i = 0; i < line_count; i++)
      for (j = 0; j < width; j++)
        if (grid[i][j] == 'O') cache_buffer_size += 4;
  }
  char buffer[cache_buffer_size + 1];
  for (i = 0; i < line_count; i++){
    for (j = 0; j < width; j++){
      if (grid[i][j] == 'O'){
        buffer[buf_len] = i + '0';
        buffer[buf_len + 1] = ',';
        buffer[buf_len + 2] = j + '0';
        buffer[buf_len + 3] = ',';
        buf_len += 4;
      }
    }
  }
  buffer[cache_buffer_size] = '\0';
  char *cache_key = NULL;
  cache_key = m_alloc(cache_key, cache_buffer_size + 1, "Unable to allocate memory for cache key");
  strcpy(cache_key, buffer);
  return cache_key;
}

void add_to_cache(char *str){
  struct node *new_node = NULL;
  new_node = m_alloc(new_node, sizeof(struct node), "Unable to allocate memory for cache node");
  new_node->content = NULL;
  new_node->content = m_alloc(new_node->content, strlen(str) + 1, "Unable to allocate memory for cache cfg");
  strcpy(new_node->content, str);
  new_node->next = NULL;

  if (cache == NULL){
    cache = new_node;
    return;
  }

  new_node->next = cache;
  cache = new_node;
}

void dispose_cache(){
  struct node *temp;

  while(cache != NULL){
    temp = cache;
    cache = cache->next;
    free(temp->content);
    free(temp);
  }
}

bool cache_compare(const struct node *item, const char *str){
  if(strcmp(item->content, str) != 0)
    return false;

  return true;
}

int find_in_cache(const char* str){
  int count = 1;
  struct node *cur;

  for (cur = cache; cur != NULL && !cache_compare(cur, str); cur = cur->next) count++;
  if (cur == NULL)
    return 0;

  return count;
}

void print_cache(){
  struct node *ptr = cache;

  while(ptr != NULL){
    printf("%s\n", ptr->content);
    ptr = ptr->next;
  }
}

void print_grid(char *grid[]){
  for(int i = 0; i< line_count; i++)
    printf("%s\n", grid[i]);
}

void tilt_north(char *grid[]){
  int i, j;
  int next_free;

  for (i = 0; i < width; i++){
    next_free = 0;
    for (j = 0; j < line_count; j++){
      if (grid[j][i] == '#')
        next_free = j + 1;
      else if (grid[j][i] == 'O'){
        grid[j][i] = '.';
        grid[next_free++][i] = 'O';
      }
    }
  }
}

void tilt_west(char *grid[]){
  int i, j;
  int next_free;

  for (i = 0; i < line_count; i++){
    next_free = 0;
    for (j = 0; j < width; j++){
      if (grid[i][j] == '#')
        next_free = j + 1;
      else if (grid[i][j] == 'O'){
        grid[i][j] = '.';
        grid[i][next_free++] = 'O';
      }
    }
  }
}

void tilt_east(char *grid[]){
  int i, j;
  int next_free;

  for (i = 0; i < line_count; i++){
    next_free = width - 1;
    for (j = width - 1; j >= 0; j--){
      if (grid[i][j] == '#')
        next_free = j - 1;
      else if (grid[i][j] == 'O'){
        grid[i][j] = '.';
        grid[i][next_free--] = 'O';
      }
    }
  }
}

void tilt_south(char *grid[]){
  int i, j;
  int next_free;

  for (i = 0; i < width; i++){
    next_free = line_count - 1;
    for (j = line_count - 1; j >= 0; j--){
      if (grid[j][i] == '#')
        next_free = j - 1;
      else if (grid[j][i] == 'O'){
        grid[j][i] = '.';
        grid[next_free--][i] = 'O';
      }
    }
  }
}

void cycle(char *grid[]){
  tilt_north(grid);
  tilt_west(grid);
  tilt_south(grid);
  tilt_east(grid);
}

int total_load(char *grid[]){
  int i, j, tally = 0;

  for (i = 0; i < width; i++){
    for (j = 0; j < line_count; j++){
      if (grid[j][i] == 'O')
        tally += line_count - j;
    }
  }
  return tally;
}

long part1(char *lines[]){
  tilt_north(lines);
  return total_load(lines);
}

long part2(char *lines[]){
  char *key, cache_index;
  bool found_in_cache = false, use_cache = true;
  int step = 1;
  
  int cycles = 1000000000;
  for (int i = 0; i < cycles; i += step){
    cycle(lines);

    if (!found_in_cache & use_cache){
      key = create_cache_key(lines);
      cache_index = find_in_cache(key);
      if (cache_index > 0){
        int factor = (cycles - i - 1) / cache_index;
        i = factor * cache_index + i;
        found_in_cache = true;
      }
      else{
        add_to_cache(key);
      }
    }
  }

  dispose_cache();
  cache_buffer_size = 0;

  return total_load(lines);
}

void solve_file(char *file_path){
  char *lines[INIT_LINES_SIZE];

  line_count = read_file(lines, file_path);
  width = strlen(lines[0]);

  printf("Part 1 Result: %ld\n", part1(lines));
  dispose_read_file(lines, line_count);

  line_count = read_file(lines, file_path);

  printf("Part 2 Result: %ld\n", part2(lines));
  dispose_read_file(lines, line_count);
}

int main(int argc, char *argv[]){
  for (int i = 1; i < argc; ++i) {
    solve_file(argv[i]);
  }
  return EXIT_SUCCESS;
}
