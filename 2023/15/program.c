#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"

struct node{
  char *label;
  int focal_length;
  struct node *next;
};

struct node *hash_table[256] = { NULL };

void dispose_hash_table(){
  struct node *ptr;

  for(int i = 0; i < 256; i++){
    while(hash_table[i] != NULL){
      ptr = hash_table[i];
      hash_table[i] = hash_table[i]->next;
      free(ptr->label);
      free(ptr);
    }
  }
}

void print_hash_table(){
  struct node *p;

  for (int i = 0; i < 256; i++){
    if (hash_table[i] != NULL){
      printf("Box %3d:", i);
      p = hash_table[i];
      while(p != NULL){
        printf(" [%s %d]", p->label, p->focal_length);
        p = p->next;
      }
      printf("\n");
    }
  }
}

void remove_lens_from_box(char *label, int box_id){
  struct node *cur, *prev;

  for (cur = hash_table[box_id], prev = NULL; cur != NULL && strcmp(cur->label, label) != 0; prev = cur, cur = cur->next);

  if(cur == NULL)
    return;

  if (prev == NULL)
    hash_table[box_id] = cur->next;
  else{
    prev->next = cur->next;
  }
  free(cur);
}

struct node *find_lens_in_box(char *label, int box_id){
  struct node *cur;

  for (cur = hash_table[box_id]; cur != NULL && strcmp(cur->label, label); cur = cur->next);

  return cur;
}

void add_lens_to_box(char *label, int focal_length, int box_id){
  struct node *new_node, *cur, *prev;
  
  new_node = find_lens_in_box(label, box_id);
  if (new_node != NULL)
    new_node->focal_length = focal_length;
  else{
    new_node = m_alloc(new_node, sizeof(struct node), "Unable to allocate memory for new node");
    new_node->label = NULL;
    new_node->label = m_alloc(new_node->label, sizeof(strlen(label) + 1), "Unable to allocate memory for node label");
    strcpy(new_node->label, label);
    new_node->focal_length = focal_length;
    new_node->next = NULL;

    for (cur = hash_table[box_id], prev = NULL; cur != NULL; prev = cur, cur = cur->next);

    if(prev == NULL)
      hash_table[box_id] = new_node;
    else
      prev->next = new_node;
  }
}

int calc_total(){
  int counter, tally = 0;
  struct node *p;

  for (int i = 0; i < 256; i++){
    counter = 0;
    p = hash_table[i];
    while(p != NULL){
      counter++;
      tally += (i + 1) * counter * p->focal_length;
      p = p->next;
    }
  }
  return tally;
}

int hash(char *step){
  int current_value = 0;
  for(int i = 0; i < (int)strlen(step); i++){
    current_value += step[i];
    current_value *= 17;
    current_value %= 256;
  }
  return current_value;
}

long part1(char *steps[], int line_count){
  long tally = 0;
  for (int i = 0; i < line_count; i++){
    tally += hash(steps[i]);
  }
  return tally;
}

long part2(char *steps[], int line_count){
  struct split_result split1;
  int i, hash_value; 
  long tally = 0;

  for (i = 0; i < 256; i++)
    hash_table[i] = NULL;

  for (i = 0; i < line_count; i++){
    split1 = split(steps[i], "=");
    if (split1.count > 1){
      hash_value = hash(split1.split[0]);
      add_lens_to_box(split1.split[0], atoi(split1.split[1]), hash_value);
      dispose_split_result(split1);
      continue;
    }

    split1 = split(steps[i], "-");
    if (split1.count > 0){
      hash_value = hash(split1.split[0]);
      remove_lens_from_box(split1.split[0], hash_value);
      dispose_split_result(split1);
      continue;
    }

    // should not reach here
    printf("Unexpected step value: %s\n", steps[i]);
    exit(EXIT_FAILURE);
  }

  tally = calc_total();

  dispose_hash_table();

  return tally;
}

void solve_file(char *file_path){
  struct split_result split1;
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, file_path);
  
  split1 = split(lines[0], ",");
  dispose_read_file(lines, line_count);
  
  line_count = split1.count;
  char *steps[line_count];
  for(int i = 0; i < line_count; i++){
    steps[i] = split1.split[i];
  }

  printf("Part 1 Result: %ld\n", part1(steps, line_count));
  printf("Part 2 Result: %ld\n", part2(steps, line_count));

  dispose_split_result(split1);
}

int main(int argc, char *argv[]){
  for (int i = 1; i < argc; ++i) {
    solve_file(argv[i]);
  }
  return EXIT_SUCCESS;
}
