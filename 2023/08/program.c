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
  char node[LOCATION_LEN];
  char left[LOCATION_LEN];
  char right[LOCATION_LEN];
};

struct element *map[91][91][91]; 

void populate_map(char *line);
void set_location(char element[], char *str);
void set_next_location(char current_location[], const char *instructions, int *ins_ptr);
bool is_finished(char loc[]);
bool is_finished2(char location[]);
int asc_compare(const void *a, const void *b);
void dispose();
int part1(char *lines[], int line_count);
long part2(char *lines[], int line_count);

int main(void){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, INPUT_FILE);

  printf("Part 1 Result: %d\n", part1(lines, line_count));
  printf("Part 2 Result: %ld\n", part2(lines, line_count));

  dispose_read_file(lines, line_count);

  return EXIT_SUCCESS;
}

int part1(char *lines[], int line_count){
  struct split_result split1, split2;
  int i, inst_ptr = 0, steps = 0;
  char current_location[3];
  
  char instructions[strlen(lines[0] + 1)];
  strcpy(instructions, lines[0]);
  instructions[strlen(lines[0])] = '\0';

  for(i = 1; i < line_count;i++){
    char line[strlen(lines[i]) + 1];
    strcpy(line, lines[i]);
    line[strlen(lines[i])] = '\0';
    populate_map(line);
  }

  set_location(current_location, INIT_LOCATION);
  
  while (!is_finished(current_location)){
    set_next_location(current_location, instructions, &inst_ptr);
    steps++;
  }

  dispose();

  return steps;
}

long part2(char *lines[], int line_count){
  struct split_result split1, split2;
  int i, j, inst_ptr = 0, path_count = 0;
  long steps = 0;
  
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
  for (i = 'A'; i <= 'Z'; i++){
    for (j = 'A'; j <= 'Z'; j++){
      if (map[i][j]['A'] != NULL){
        current_location = m_alloc(current_location, (LOCATION_LEN) * (path_count + 1), "Unable to allocate current location");
        if (path_count == 0){
          set_location(current_location, map[i][j]['A']->node);
        }
        else{
          p = current_location + path_count * (LOCATION_LEN);
          set_location(p, map[i][j]['A']->node);
        }
        path_count++;
      }
    }
  }

  int freq[path_count], current = 0;
  while (current < path_count){
    if (is_finished2(current_location)){
      freq[current++] = steps;
      if (current == path_count)
        break;
      steps = 0;
      current_location += LOCATION_LEN;
    }
    set_next_location(current_location, instructions, &inst_ptr);
    steps++;
  }

  // note: sorting asc helps a little
  qsort(freq, path_count, sizeof(int), asc_compare);

  long shared = freq[0], first, second;
  for(i = 0; i < path_count; i++){
    first = shared;
    second = freq[i];
    
    while (first != second){
      if (first < second)
        first += shared;
      else
        second += freq[i];
    }
    shared = first;
    // printf("Shared number: %ld\n", shared);
  }

  dispose();

  return shared;
}

int asc_compare(const void *a, const void *b){
  return *(int *)a - *(int *)b;
}

bool is_finished(char location[]){
  return location[0] + location[1] + location[2] == FINISH_LOCATION;
}

bool is_finished2(char location[]){
  char *p = location + 2;
  if (*p != 'Z')
    return false;
  p += LOCATION_LEN;
  return true;
}

void add_element(struct element *element, char *location){
  map[location[0]][location[1]][location[2]] = element;
}

struct element *get_element(char *location){
  return map[location[0]][location[1]][location[2]];
}

void set_next_location(char current_location[], const char *instructions, int *ins_ptr){
  char instruction = instructions[(*ins_ptr)++];

  struct element *element = get_element(current_location);
  set_location(current_location, instruction == 'L' ? element->left : element->right);

  if (instructions[*ins_ptr] == '\0')
    *ins_ptr = 0;
}

void set_location(char element[], char *str){
  element[0] = str[0];
  element[1] = str[1];
  element[2] = str[2];
}

void populate_map(char *line){
  struct split_result split1, split2;
  char location[LOCATION_LEN + 1];

  split1 = split(line, "=");
  strcpy(location, trim(split1.split[0]));
  location[LOCATION_LEN] = '\0';
  split2 = split(split1.split[1], ",");
  
  struct element *element = NULL;
  element = m_alloc(element, sizeof(struct element), "Unable to allocate memory to element");
  set_location(element->node, location);
  set_location(element->left, trim_char(split2.split[0], '('));
  set_location(element->right, trim_char(split2.split[1], ')'));
  add_element(element, location);

  dispose_split_result(split2);
  dispose_split_result(split1);
}

void dispose(){
  int i, j, k;

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
