#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"
#include "day05.h"

void add_to_list(struct node **list, long dest, long src, long range){
  struct node *new_node = NULL, *prev, *cur;

  new_node = m_alloc(new_node, sizeof(struct node), "Cannot allocate memory to new node");
  new_node->src = src;
  new_node->dest = dest;
  new_node->range = range;
  new_node->next = NULL;

  for (cur = *list, prev = NULL;
    cur != NULL && cur->src < new_node->src;
    prev = cur, cur = cur->next)
    ;

  if (prev == NULL){
    new_node->next = cur;
    *list = new_node;
  }
  else if (cur == NULL){
    prev->next = new_node;
  }
  else{
    prev->next = new_node;
    new_node->next = cur;
  }
}

void process_line(struct node **src_list, char *line){
  struct split_result items;

  items = split(line, " ");
  if (items.count < 3){
    printf("Too few items in split\n");
    exit(EXIT_FAILURE);
  }
  long dest = atol(items.split[0]);
  long src = atol(items.split[1]);
  long range = atol(items.split[2]);
  
  add_to_list(src_list, dest, src, range);
  dispose_split_result(items);
}

void print_list(struct node *list, const char *title){
  struct node *p = list;

  printf("%s\n", title);
  while (p){
    printf("Src: %ld, Dest: %ld, Range: %ld\n", p->src, p->dest, p->range);
    p = p->next;
  }
}

void destroy(struct node *list){
  struct node *p;

  while (list){
    p = list;
    list = list->next;
    free(p);
  }
}
