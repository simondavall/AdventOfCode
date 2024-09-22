#ifndef DAY05_H
#define DAY05_H

#include <stdlib.h>

struct node {
  long src;
  long dest;
  long range;
  struct node *next;
};

void process_line(struct node **src_list, char *line);
void add_to_list(struct node **list, long dest, long src, long range);
void destroy(struct node *list);
void print_list(struct node *list, const char *title);

#endif // !DAY05_H
#define DAY05_H


