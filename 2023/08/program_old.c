#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"
#define INPUT_FILE "input"
#define INIT_LOCATION "AAA"
#define FINISH_LOCATION ('Z' * 3)
#define LOCATION_LEN 3

struct element{
  char node[LOCATION_LEN + 1];
  char left[LOCATION_LEN + 1];
  char right[LOCATION_LEN + 1];
};

struct element *map[26][26][26]; 

void populate_map(char *line);
void set_next_location(char **current_location, const char *instructions, int *ins_ptr);
void set_next_location2(char **current_location, int path_count, const char *instructions, int *ins_ptr);
bool is_finished(char loc[]);
bool are_finished(char location[], int n);
void dispose(char *current_location);
int part1(char *lines[], int line_count);
int part2(char *lines[], int line_count);

int main(void){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, INPUT_FILE);


  printf("Part 1 Result: %d\n", part1(lines, line_count));
  // for(int j = 0; j < line_count; j++)
  //   printf("%s\n", lines[j]);
  //printf("Part 2 Result: %d\n", part2(lines, line_count));

  dispose_read_file(lines, line_count);

  return EXIT_SUCCESS;
}

int part1(char *lines[], int line_count){
  struct split_result split1, split2;
  int i, inst_ptr = 0, steps = 0;
  
  char instructions[strlen(lines[0] + 1)];
  strcpy(instructions, lines[0]);
  instructions[strlen(lines[0])] = '\0';

  for(i = 1; i < line_count;i++){
    char line[strlen(lines[i]) + 1];
    strcpy(line, lines[i]);
    line[strlen(lines[i])] = '\0';
    instructions[0] = 'L';
    populate_map(line);
  }

  char *current_location = NULL;
  current_location = m_alloc(current_location, 4, "Unable to allocate current location");
  strcpy(current_location, INIT_LOCATION);
  
  while (!is_finished(current_location)){
    set_next_location(&current_location, instructions, &inst_ptr);
    steps++;
  }

  dispose(current_location);

  return steps;
}

int part2(char *lines[], int line_count){
  struct split_result split1, split2;
  int i, j, inst_ptr = 0, steps = 0, path_count = 0;
  
  char instructions[strlen(lines[0] + 1)];
  strcpy(instructions, lines[0]);
  instructions[strlen(lines[0])] = '\0';

  for(i = 1; i < line_count;i++){
    char line[strlen(lines[i]) + 1];
    strcpy(line, lines[i]);
    line[strlen(lines[i])] = '\0';
    populate_map(line);
  }
  

  char *current_location = NULL, *p;
  for (i = 0; i < 26; i++){
    for (j = 0; j < 26; j++){
      if (map[i][j][0] != NULL){
        current_location = m_alloc(current_location, (LOCATION_LEN + 1) * (path_count + 1), "Unable to allocate current location");
        if (path_count == 0){
          // printf("Adding start location: %s\n", map[i][j][0]->node);
          strcpy(current_location, map[i][j][0]->node);
          current_location[LOCATION_LEN] = '\0';
          // printf("Added start location: %s\n", current_location);
        }
        else{
          // printf("Adding start location: %s\n", map[i][j][0]->node);
          p = current_location + path_count * (LOCATION_LEN + 1);
          strcpy(p , map[i][j][0]->node);
          p[LOCATION_LEN] = '\0';
          // printf("Added start location: %s\n", p);
          // printf("Initial start location still(?): %s\n", current_location);
        }
        path_count++;
      }
    }
  }

  // current_location = m_alloc(current_location, 4, "Unable to allocate current location");
  // strcpy(current_location, INIT_LOCATION);
  
  while (!are_finished(current_location, path_count)){
    set_next_location2(&current_location, path_count, instructions, &inst_ptr);
    steps++;
    if (steps % 1000000 == 0)
      printf("Step: %d\n", steps);
  }

  dispose(current_location);

  return steps;
}

bool is_finished(char loc[]){
  return loc[0] + loc[1] + loc[2] == FINISH_LOCATION;
}

bool are_finished(char location[], int n){
  char *p = location;
  for(int i = 0; i < n; i++){
    int idx = (i * (LOCATION_LEN + 1)) + 2;
    // printf("Checking finish: last char: %c\n", location[idx]);
    if (location[idx] != 'Z')
      return false;
  }
  return true;
}

void add_element(struct element *element, char *location){
  map[location[0] - 'A'][location[1] - 'A'][location[2] - 'A'] = element;
}

struct element *get_element(char *location){
  return map[location[0] - 'A'][location[1] - 'A'][location[2] - 'A'];
}

void set_next_location(char **current_location, const char *instructions, int *ins_ptr){
  char instruction = instructions[(*ins_ptr)++];

  struct element *element = get_element(*current_location);
  strcpy(*current_location, instruction == 'L' ? element->left : element->right);

  if (instructions[*ins_ptr] == '\0')
    *ins_ptr = 0;
}

void set_next_location2(char **current_location, int path_count, const char *instructions, int *ins_ptr){
  int i;
  char instruction = instructions[(*ins_ptr)++];

  char *p = *current_location;
  while(path_count-- > 0){
  // for(i = 0; i < path_count; i++){
    struct element *element = get_element(p);
    strcpy(p, instruction == 'L' ? element->left : element->right);
    // printf("Set next location: %s\n", p);
    p += (LOCATION_LEN + 1);
  }

  if (instructions[*ins_ptr] == '\0')
    *ins_ptr = 0;
}

void populate_map(char *line){
  struct split_result split1, split2;
  char loc[LOCATION_LEN + 1];

  split1 = split(line, "=");
  strcpy(loc, trim(split1.split[0]));
  loc[LOCATION_LEN] = '\0';
  // printf("Loc: %s - [%d, %d, %d]\n", loc, loc[0] - 'A', loc[1] - 'A', loc[2] - 'A');
  // loc1[loc[0] - 'A'] = loc[1] - 'A';
  // loc2[loc[1] - 'A'] = loc[2] - 'A';
  split2 = split(split1.split[1], ",");
  // printf("Left: %s\n", trim_char(split2.split[0], '('));
  // printf("Right: %s\n", trim_char(split2.split[1], ')'));
  struct element *element = NULL;
  element = m_alloc(element, sizeof(struct element), "Unable to allocate memory to element");
  strcpy(element->node, loc);
  strcpy(element->left, trim_char(split2.split[0], '('));
  strcpy(element->right, trim_char(split2.split[1], ')'));
  add_element(element, loc);
  // map[loc[0] - 'A'][loc[1] - 'A'][loc[2] - 'A'] = element;

  dispose_split_result(split2);
  dispose_split_result(split1);
}

void dispose(char *current_location){
  int i, j, k;

  free(current_location);
  struct element *temp;
  for (i = 0; i < 26; i++){
    for (j = 0; j < 26; j++){
      for (k = 0; k < 26; k++){
        if (map[i][j][k] != NULL){
          free(map[i][j][k]);
        }
      }
    }
  }
}
