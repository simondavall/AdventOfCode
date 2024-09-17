#include <ctype.h>
#include <stdatomic.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include "helper.h"

#define MAX_RED 12
#define MAX_GREEN 13
#define MAX_BLUE 14

int part1(char *lines[], int line_count);
int part2(char *lines[], int line_count);

int main(void){
  int n, i;
  char **p;
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, "input");
  printf("Part 1 Result: %d\n", part1(lines, line_count));

  line_count = read_file(lines, "input");
  printf("Part 2 Result: %d\n", part2(lines, line_count));

  dispose_read_file(lines, line_count);

  return EXIT_SUCCESS;
}

bool is_size_valid(int size, char *colour){
  switch(tolower(colour[0])){
    case 'r':
      return size <= MAX_RED;
    case 'b':
      return size <= MAX_BLUE;
    case 'g':
      return size <= MAX_GREEN;
    default:
      printf("Found invalid colour: %s\n", colour);
      exit(EXIT_FAILURE);
  }
}

int part1(char *lines[], int line_count){
  struct split_result split1, split2, split3, split4;
  int i, j, k, m, number;
  char *colour, *game_cubes, *cube_item, *cube_group;
  long tally = 0;
  bool game_valid;

  for(i = 0; i < line_count; i++){
    game_valid = true;
    split1 = split(lines[i], ":");
    game_cubes = split1.split[1];
    split2 = split(game_cubes, ";");

    j = 0;
    while(j < split2.count && game_valid){
      cube_group = split2.split[j++];
      split3 = split(cube_group, ",");
      k = 0;
      while(k < split3.count && game_valid){
        cube_item = ltrim(split3.split[k++]);
        split4 = split(cube_item, " ");
        m = 0;
        while(m++ < split4.count){
          number = atoi(split4.split[0]);
          colour = split4.split[1];
          if(!is_size_valid(number, colour)){
            game_valid = false;
            break;
          }
        }
        dispose_split_result(split4);
      }
      dispose_split_result(split3);
    }
    dispose_split_result(split2);

    if (game_valid){
      split2 = split(split1.split[0], " ");
      tally += atoi(split2.split[1]);
      dispose_split_result(split2);
    }
    dispose_split_result(split1);
  }

  return tally;
}

int part2(char *lines[], int line_count){
  struct split_result split1, split2, split3, split4;
  int i, j, k, m, number, max_red, max_green, max_blue;
  char *colour, *game_cubes, *cube_item, *cube_group;
  long tally = 0;

  for(i = 0; i < line_count; i++){
    max_red = max_blue = max_green = 0;
    split1 = split(lines[i], ":");
    game_cubes = split1.split[1];
    split2 = split(game_cubes, ";");
    j = 0;
    while(j < split2.count){
      cube_group = split2.split[j++];
      split3 = split(cube_group, ",");
      k = 0;
      while(k < split3.count){
        cube_item = ltrim(split3.split[k++]);
        split4 = split(cube_item, " ");
        m = 0;
        while(m++ < split4.count){
          number = atoi(split4.split[0]);
          colour = split4.split[1];
          // printf("cube %d, colour: %s", number, colour);
          switch(colour[0]){
            case 'r':
              if(number > max_red) max_red = number;
              break;
            case 'g':
              if (number > max_green) max_green = number;
              break;
            case 'b':
              if (number > max_blue) max_blue = number;
              break;
            default:
              printf("Found invalid colour: %s\n", colour);
              exit(EXIT_FAILURE);
          }

        }
        dispose_split_result(split4);
      }
      dispose_split_result(split3);
    }
    dispose_split_result(split2);
    dispose_split_result(split1);

    tally += max_red * max_green * max_blue;
  }

  return tally;
}

