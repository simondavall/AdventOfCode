#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include "helper.h"
#define INPUT_FILE "sample"

struct tile{
  int x;
  int y;
  char ch;
};

enum direction {
  INVALID,
  NORTH,
  EAST,
  SOUTH,
  WEST,
};

int map_width, map_height;
int start_x = 0, start_y = 0;

struct tile *tunnel = NULL;
int tunnel_len = 0;

void print_array(){
  int i = 0;

  while(i < tunnel_len){
    printf("ch:%c, x:%d, y:%d\n", tunnel[i].ch, tunnel[i].x, tunnel[i].y);
    i++;
  }
}
enum direction dir_reverse(enum direction direction){
  switch (direction) {
    case NORTH:   return SOUTH; break;
    case EAST:    return WEST; break;
    case SOUTH:   return NORTH; break;
    case WEST:    return EAST; break;
  
    default:
    printf("Unknown direction value found: %d\n", direction);
    exit(EXIT_FAILURE);
  }
}

void add_tile_to_tunnel(char ch, int x, int y){
  struct tile t;
  tunnel = m_alloc(tunnel, sizeof(struct tile) * (tunnel_len + 1), "Unable to allocate memory for tile");
  t.x = x;
  t.y = y;
  t.ch = ch;
  tunnel[tunnel_len++] = t;
}

enum direction map_char(char ch, enum direction from){
  switch(ch){
    case '|':
      if (from == SOUTH) return NORTH;
      if (from == NORTH) return SOUTH;
      break;
    case '-':
      if (from == EAST) return WEST;
      if (from == WEST) return EAST;
      break;
    case 'F':
      if (from == SOUTH) return EAST;
      if (from == EAST) return SOUTH;
      break;
    case '7':
      if (from == SOUTH) return WEST;
      if (from == WEST) return SOUTH;
      break;
    case 'J':
      if (from == NORTH) return WEST;
      if (from == WEST) return NORTH ;
      break;
    case 'L':
      if (from == NORTH) return EAST;
      if (from == EAST) return NORTH ;
      break;

    default:
      return INVALID;
  }

  return INVALID;
}

void find_start(char *map[], int *x, int *y){
  for(*x = 0;*x < map_width; (*x)++){
    for(*y = 0;*y < map_height; (*y)++){
      if (map[*y][*x] == 'S'){
        add_tile_to_tunnel('S', *x, *y);
        return;
      }
    }
  }
}

bool is_valid_neighbour(char *map[], int x, int y, enum direction from){
  enum direction to = map_char(map[y][x], from);
  return to != INVALID;
}

enum direction *find_valid_neighbour(char *map[], int x, int y){
  enum direction *valid_neighbours = NULL;
  valid_neighbours = m_alloc(valid_neighbours, sizeof(enum direction) * 4, "Unable to allocate memory to valid neighbours");
  int found = 0;

  if (x > 0 && is_valid_neighbour(map, x - 1, y, EAST))
    valid_neighbours[found++] = WEST;
  if (y > 0 && is_valid_neighbour(map, x, y - 1, SOUTH))
    valid_neighbours[found++] = NORTH ;
  if (x < map_width && is_valid_neighbour(map, x + 1, y, WEST))
    valid_neighbours[found++] = EAST;
  if (y < map_height && is_valid_neighbour(map, x, y + 1, NORTH))
    valid_neighbours[found++] = SOUTH;

  if (found != 2)
    valid_neighbours[0] = INVALID;

  return valid_neighbours;
}

void transpose(int *x, int *y, enum direction to){
  switch (to) {
    case NORTH:   *y -= 1;  break;
    case EAST:    *x += 1;  break;
    case SOUTH:   *y += 1;  break;
    case WEST:    *x -= 1;  break;
    default:                break;  
  }
}

int traverse_tunnel(char *map[], int x, int y, enum direction to){
  int steps = 0, ch = '\0';
  while (ch != 'S') {
    transpose(&x,&y, to);
    ch = map[y][x];
    if (ch != 'S')
      add_tile_to_tunnel(ch, x, y);
    to = map_char(ch, dir_reverse(to));
    steps++;
  }
  return steps;
}

int get_tunnel_tile(int x, int y){
  for (int i = 0; i < tunnel_len; i++){
    if (tunnel[i].x == x && tunnel[i].y == y)
      return i;
  }
  return -1;
}

bool check_range_x(int lower, int upper, int y){
  int count = 0, t_id, i;
  
  for(i = lower; i < upper; i++){
    // printf("Checking against x: %d, y: %d\n", i, y);
    t_id = get_tunnel_tile(i, y);
    // printf("Tunnel id: %d\n", t_id);
    if (t_id == -1){
      // printf("Is not part of tunnel, or self. Move next 'x'\n");
      continue;
    }
    char ch = tunnel[t_id].ch;
    // printf("Checking against ch: %c\n", ch);
    if (ch == '|'){
       printf("Found a tunnel boundary | at x:%d, y:%d\n", i, y);
      // printf("Found a tunnel boundary\n");
      count++;
    }
  }

  if (count % 2 == 1)
    printf("Found inside on x from lower:%d to upper:%d\n", lower, upper);

  return count % 2 == 1;
}

bool check_range_y(int lower, int upper, int x){
  int count = 0, t_id, i;
  
  for(i = lower; i < upper; i++){
    // printf("Checking against x: %d, y: %d\n", i, y);
    t_id = get_tunnel_tile(x, i);
    // printf("Tunnel id: %d\n", t_id);
    if (t_id == -1){
      // printf("Is not part of tunnel, or self. Move next 'x'\n");
      continue;
    }
    char ch = tunnel[t_id].ch;
    // printf("Checking against ch: %c\n", ch);
    if (ch == '-'){
       printf("Found a tunnel boundary - at x:%d, y:%d\n", x, i);
      count++;
    }
  }
  if (count % 2 == 1)
    printf("Found inside on y from lower:%d to upper:%d\n", lower, upper);


  return count % 2 == 1;
}

enum direction get_lateral_direction(char ch){
  switch (ch) {
    case 'J': return WEST; break;
    case 'F': return EAST; break;
    case 'L': return EAST; break;
    case '7': return WEST; break;
    default:  return INVALID; break;
  }
}

enum direction get_vertical_direction(char ch){
  switch (ch) {
    case 'J': return NORTH; break;
    case 'F': return SOUTH; break;
    case 'L': return NORTH; break;
    case '7': return SOUTH; break;
    default:  return INVALID; break;
  }

}
bool check_north_south(int lower, int upper, int x){
  int i, t_id, count, pd_count = 0;
  struct tile tile;
  enum direction pd[2] = {0};

  count = 0;
  for (i = lower; i < upper; i++) {
    t_id = get_tunnel_tile(x, i);
    if (t_id < 0)
      continue;
    tile = tunnel[t_id];
    if (tile.ch == '-'){
      count++;
      continue;
    }
    if (tile.ch == '|')
      continue;

    pd[pd_count++] = get_lateral_direction(tile.ch);
    if (pd_count > 1){
      if (pd[0] != pd[1])
        count++;
      pd_count = 0;
    }
  }
  
  return count % 2 == 1;
}

bool check_east_west(int lower, int upper, int y){
  int i, t_id, count, pd_count = 0;
  struct tile tile;
  enum direction pd[2] = {0};

  count = 0;
  for (i = lower; i < upper; i++) {
    t_id = get_tunnel_tile(i, y);
    if (t_id < 0)
      continue;
    tile = tunnel[t_id];
    if (tile.ch == '|'){
      count++;
      continue;
    }
    if (tile.ch == '-')
      continue;

    pd[pd_count++] = get_vertical_direction(tile.ch);
    if (pd_count > 1){
      if (pd[0] != pd[1])
        count++;
      pd_count = 0;
    }
  }
  
  return count % 2 == 1;
}

bool is_inside_tunnel(int x, int y){
  // printf("Checking x:%d, y:%d\n", x, y);
  if (get_tunnel_tile(x, y) > -1)
    return false;

  bool to_north = check_north_south(0, y, x);
  bool to_east = check_east_west(x + 1, map_width, y);
  bool to_south = check_north_south(y + 1, map_height, x);
  bool to_west = check_east_west(0, x, y);
   
  return to_north && to_east && to_south && to_west;
}

void set_correct_start_char(enum direction *neighbours){
  char ch;
  switch (neighbours[0]) {
    case NORTH:
      if (neighbours[1] == EAST) ch = 'L';
      else if (neighbours[1] == SOUTH) ch = '|';
      else if (neighbours[1] == WEST) ch = 'J';
      break;
    case EAST:
      if (neighbours[1] == NORTH) ch = 'L';
      else if (neighbours[1] == SOUTH) ch = 'F';
      else if (neighbours[1] == WEST) ch = '-';
      break;
    case SOUTH:
      if (neighbours[1] == NORTH) ch = '|';
      else if (neighbours[1] == EAST) ch = 'F';
      else if (neighbours[1] == WEST) ch = '7';
      break;
    case WEST:
      if (neighbours[1] == NORTH) ch = 'J';
      else if (neighbours[1] == EAST) ch = '-';
      else if (neighbours[1] == SOUTH) ch = '7';
      break;
     default:
      printf("Found invalid neighbour: %d\n", neighbours[0]);
      exit(EXIT_FAILURE);
  }
  tunnel[0].ch = ch;
}

long part1(char *map[]){
  enum direction *valid_neighbours;

  find_start(map, &start_x, &start_y);
  valid_neighbours = find_valid_neighbour(map, start_x, start_y);
  set_correct_start_char(valid_neighbours);
  int steps = traverse_tunnel(map, start_x, start_y, valid_neighbours[0]);

  return steps / 2;
}

long part2(){
  int x, y, inside = 0;

  for(x = 0; x < map_width; x++){
    for(y = 0; y < map_height; y++){
      if (is_inside_tunnel(x, y))
        inside++;
    }
  }

  return inside;
}

void solve_file(char *file_path){
  char *map[INIT_LINES_SIZE];

  int line_count = read_file(map, file_path);

  map_width = strlen(map[0]);
  map_height = line_count;

  tunnel_len = 0;
  //tunnel = NULL;

  // for(int j = 0; j < line_count; j++)
  //   printf("%s\n", map[j]);

  printf("Part 1 Result: %ld\n", part1(map));
  printf("Part 2 Result: %ld\n", part2());

  free(tunnel);
  dispose_read_file(map, line_count);
}

int main(int argc, char *argv[]){
  for (int i = 1; i < argc; ++i) {
    solve_file(argv[i]);
  }
  return EXIT_SUCCESS;
}
