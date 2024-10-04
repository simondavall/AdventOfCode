#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"
#define MAX_LEN 110
int max_height = 0;
int max_width = 0;

struct node {
  char *key;
  struct node *next;
};

struct node *cache[MAX_LEN][MAX_LEN];

void add_to_cache(char *str, int x, int y){
  struct node *new_node = NULL;
  new_node = m_alloc(new_node, sizeof(struct node), "Unable to allocate memory for cache node");
  new_node->key = NULL;
  new_node->key = m_alloc(new_node->key, strlen(str) + 1, "Unable to allocate memory for cache cfg");
  strcpy(new_node->key, str);
  new_node->next = NULL;

  if (cache[y][x] == NULL){
    cache[y][x] = new_node;
    return;
  }

  new_node->next = cache[y][x];
  cache[y][x] = new_node;
}

void dispose_cache(){
  struct node *temp;

  for(int i = 0; i < max_height; i++)
    for (int j = 0; j < max_width; j++){
      while(cache[i][j] != NULL){
        temp  = cache[i][j];
        cache[i][j] = cache[i][j]->next;
        free(temp->key);
        free(temp);
      }
    }
}

bool cache_compare(const struct node *item, const char *key){
  if(strcmp(item->key, key) != 0)
    return false;

  return true;
}

int find_in_cache(const char* key, int x, int y){
  int count = 1;
  struct node *cur;

  for (cur = cache[y][x]; cur != NULL && !cache_compare(cur, key); cur = cur->next) count++;
  if (cur == NULL)
    return 0;

  return count;
}

struct cell{
  char ch;
  bool seen;
};

enum compass{
  NORTH,
  EAST,
  SOUTH,
  WEST,
};

struct cell grid[MAX_LEN][MAX_LEN];

void reset_grid(){
  for (int i = 0; i < max_height; i++)
    for (int j = 0; j < max_width; j++)
      grid[i][j].seen = false;
}

bool move(int *x, int *y, enum compass direction){

  switch(direction){
    case NORTH:
      if (*y > 0){ *y -= 1; return true; } break;
    case EAST:
      if (*x < max_width - 1){ *x += 1; return true; } break;
    case SOUTH:
      if (*y < max_height - 1){ *y += 1; return true; } break;
    case WEST:
      if (*x > 0){ *x -= 1; return true; } break;
    default:
      printf("Unexpected direction: %d\n", direction);
      exit(EXIT_FAILURE);
      break;
  }

  return false;
}

bool in_bounds(int x, int y){
  return x >= 0 && y >= 0 && x < max_width && y < max_height;
}

void traverse(int x, int y, enum compass direction){

  if (in_bounds(x, y)){
    char key[10];
    sprintf(key, "%3.3d,%3.3d,%c", x, y, direction + '0');
    if (find_in_cache(key, x, y) > 0){
      return;
    }
    add_to_cache(key, x, y);
  }

  if (!move(&x, &y, direction))
    return;

  grid[y][x].seen = true;

  switch (grid[y][x].ch) {
    case '.':
      traverse(x, y, direction);
      break;
    case '/':
      switch(direction){
        case NORTH:
          traverse(x, y, EAST); break;
        case EAST:
          traverse(x, y, NORTH); break;
        case SOUTH:
          traverse(x, y, WEST); break;
        case WEST:
          traverse(x, y, SOUTH); break;
      }
      break;
    case '\\':
      switch(direction){
        case NORTH:
          traverse(x, y, WEST); break;
        case EAST:
          traverse(x, y, SOUTH); break;
        case SOUTH:
          traverse(x, y, EAST); break;
        case WEST:
          traverse(x, y, NORTH); break;
      }
     break;
    case '-':
      switch(direction){
        case EAST:
        case WEST:
          traverse(x, y, direction);
          break;
        case NORTH:
        case SOUTH:
          traverse(x, y, EAST);
          traverse(x, y, WEST);
          break;
      }
      break;
    case '|':
      switch(direction){
        case NORTH:
        case SOUTH:
          traverse(x, y, direction);
          break;
        case EAST :
        case WEST:
          traverse(x, y, NORTH);
          traverse(x, y, SOUTH);
          break;
      }
      break;
    }
}

long count_energized_cells(){
  long tally = 0;
  for (int i = 0; i < max_height; i++)
    for (int j = 0; j < max_width; j++)
      if (grid[i][j].seen) tally++;

  return tally;
}

long part1(char *lines[]){
  // set up grid
  for (int i = 0; i < max_height; i++){
    for (int j = 0; j < max_width; j++){
      grid[i][j].ch = lines[i][j];
      grid[i][j].seen = false;
    }
  }

  traverse(-1, 0, EAST);
  printf("Made it here\n");

  dispose_cache();
  return count_energized_cells();
}

void find_largest(int x, int y, enum compass direction, long *largest){
    long current;

    reset_grid();
    traverse(x, y, direction);
    dispose_cache();
    current = count_energized_cells();
    if (current > *largest)
      *largest = current;
}

long part2(){
  long largest = 0, current;

  for (int i = 0; i < max_width; i++){
    find_largest(i, -1, SOUTH, &largest);
    find_largest(i, max_height, NORTH, &largest);
    find_largest(-1, i, EAST, &largest);
    find_largest(max_width, i, WEST, &largest);
  }

  return largest;
}

void solve_file(char *file_path){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, file_path);

  max_height = line_count;
  max_width = strlen(lines[0]);

  printf("Part 1 Result: %ld\n", part1(lines));
  printf("Part 2 Result: %ld\n", part2());

  dispose_read_file(lines, line_count);
}

int main(int argc, char *argv[]){
  for (int i = 1; i < argc; ++i) {
    solve_file(argv[i]);
  }
  return EXIT_SUCCESS;
}
