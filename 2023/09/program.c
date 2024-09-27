#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"

struct node{
  int data;
  int level;
  struct node *next;
};

struct node *list = NULL;

void destroy_list(struct node *list);
int create_tree(char *lines);
int get_forecast_value(int level);
int get_postcast_value(int level);

long part1(char *lines[], int line_count){
  long result = 0;
  int i, level = 0;

  for(i = 0; i < line_count; i++){
    level = create_tree(lines[i]);

    result += get_forecast_value(level);

    destroy_list(list);
    list = NULL;
  }
  return result;
}

long part2(char *lines[], int line_count){
  long result = 0;
  int i, level = 0;

  for(i = 0; i < line_count; i++){
    level = create_tree(lines[i]);

    result += get_postcast_value(level);

    destroy_list(list);
    list = NULL;
  }

  return result;
}

void solve_file(char *file_path){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, file_path);

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

void print_list(struct node *list){
  struct node *ptr = list;

  while(ptr){
    printf("%d:%d ", ptr->level, ptr->data);
    ptr = ptr->next;
  }
  printf("\b\n");
}

struct node *create_node(int n, int level){
  struct node *new_node = NULL;
  new_node = m_alloc(new_node, sizeof(struct node), "Unable to allocate memory to new node");
  new_node->data = n;
  new_node->level = level;
  new_node->next = NULL;

  return new_node;
}

void destroy_list(struct node *list){
  struct node *ptr;

  while (list){
    ptr = list;
    list = list->next;
    free(ptr);
  }
}

void add_to_list(int n, int level){
  struct node *ptr, *new_node;
  new_node = create_node(n, level);

  if (list == NULL){
    list = new_node;
    return;
  }

  ptr = list;
  while(ptr->next != NULL) ptr = ptr->next;
  ptr->next = new_node;
}

void add_new_level(int level){
  int diff;
  struct node *ptr = list;

  while(ptr != NULL && ptr->level < level - 1) ptr = ptr->next;
  
  while(ptr->next != NULL && ptr->next->level < level){
    diff = ptr->next->data - ptr->data;
    add_to_list(diff, level);
    ptr = ptr->next;
  }
}

bool is_zeroed(struct node *list, int level){
  struct node *ptr = list;

  while (ptr != NULL && ptr->level < level) ptr = ptr->next;
  
  while(ptr != NULL && ptr->level == level){
    if(ptr->data != 0)
      return false;
    ptr = ptr->next;
  }

  return true;
}

int get_forecast_value(int level){
  struct node *ptr = list;
  int n = 0, value = 0;;

  while (n < level){
    while(ptr->next != NULL && ptr->next->level < n + 1) ptr = ptr->next;
    value += ptr->data;
    n++;
  }
  return value;
}

int get_postcast_value(int level){
  struct node *ptr;
  int value = 0;

  while (level >= 0){
    ptr = list;
    while (ptr != NULL && ptr->level < level) ptr = ptr->next;
    value = ptr->data - value;
    level--;
  }
  
  return value;
}

int create_tree(char *lines){
  struct split_result split1;
  int level = 0, i;

  char current_line[strlen(lines + 1)];
  strcpy(current_line, lines);
  current_line[strlen(lines)] = '\0';

  split1 = split(current_line, " ");
  for (i = 0; i < split1.count; i++){
    add_to_list(atoi(split1.split[i]), level);
  }

  while (!is_zeroed(list, level)){
    add_new_level(++level);
  }

  return level;
}
