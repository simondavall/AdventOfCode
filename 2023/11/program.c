#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"

// Note: have changed galaxy to planet for more convenience 
struct planet{
  int x;
  int y;
};

int max_width = 0, max_height = 0;

struct planet *galaxy = NULL;
int galaxy_len = 0;

void add_planet_to_galaxy(int x, int y){
  struct planet new_planet;
  galaxy = m_alloc(galaxy, sizeof(struct planet) * (galaxy_len + 1), "Unable to allocate memory to new planet");
  new_planet.x = x;
  new_planet.y = y;

  galaxy[galaxy_len++] = new_planet;
}

void find_planets(char *map[]){
  int x, y;

  for(x = 0; x < max_height; x++){
    for(y = 0; y < max_height; y++){
      if (map[y][x] == '#')
        add_planet_to_galaxy(x, y);
    }
  }
}

void expand_galaxy_in_x(int x, int factor){
  for(int i = 0; i < galaxy_len; i++)
    if(galaxy[i].x > x)
      galaxy[i].x += (factor - 1);
}

void expand_galaxy_in_y(int y, int factor){
  for(int i = 0; i < galaxy_len; i++)
    if(galaxy[i].y > y)
      galaxy[i].y += (factor - 1);
}

void expand_galaxy(char *map[], int factor){
  int x, y;
  bool is_empty;
  for(x = max_width - 1; x >= 0; x--){
    is_empty = true;
    for(y = 0; y < max_height; y++){
      if (map[y][x] == '#'){
        is_empty = false;
        break;
      }
    }
    if(!is_empty)
      continue;
    expand_galaxy_in_x(x, factor);
  }
    
  for(y = max_height - 1; y >= 0; y--){
    is_empty = true;
    for(x = 0; x < max_width; x++){
      if (map[y][x] == '#'){
        is_empty = false;
        break;
      }
    }
    if(!is_empty)
      continue;
    expand_galaxy_in_y(y, factor);
  }
}

long calc_distances(){
  int i, j, dx, dy;
  long tally = 0;

  for (i = 0; i < galaxy_len - 1; i++){
    for (j = i + 1; j < galaxy_len; j++){
      dx = abs(galaxy[i].x - galaxy[j].x);
      dy = abs(galaxy[i].y - galaxy[j].y);
      tally += dx + dy;
    }
  }
  return tally;
}

long part1(char *map[]){
  long result = 0;

  // find the galaxies # into an array
  find_planets(map);
  // find the empty rows
  // adjust the galaxies array accordingly
  expand_galaxy(map, 2);
  // calc distances between all points and tally
  result = calc_distances();

  return result;
}

long part2(char *map[]){
  long result;

  free(galaxy);
  galaxy_len = 0;
  galaxy = NULL;

  find_planets(map);
  expand_galaxy(map, 1000000);
  result = calc_distances();
  
  return result;
}

void solve_file(char *file_path){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, file_path);

  max_width = strlen(lines[0]);
  max_height = line_count;

  galaxy_len = 0;

  // for(int j = 0; j < line_count; j++)
  //   printf("Line %2d: '%s'\n", j, lines[j]);

  printf("Part 1 Result: %ld\n", part1(lines));
  printf("Part 2 Result: %ld\n", part2(lines));

  free(galaxy);
  dispose_read_file(lines, line_count);
}

int main(int argc, char *argv[]){
  for (int i = 1; i < argc; ++i) {
    solve_file(argv[i]);
  }
  return EXIT_SUCCESS;
}
