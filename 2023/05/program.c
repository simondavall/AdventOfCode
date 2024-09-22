#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"
#include "day05.h"

struct node *s_s_src = NULL;
struct node *s_f_src = NULL;
struct node *f_w_src = NULL;
struct node *w_l_src = NULL;
struct node *l_t_src = NULL;
struct node *t_h_src = NULL;
struct node *h_l_src = NULL;

char *read_from_file(const char *filepath);
void destroy_lists();
long walk_mappings(long seed, long range);
long part1(const char *);
long part2(const char *);

FILE *fptr;
ssize_t read;
size_t len;
char *line = NULL;

int main(void){
  char *filepath = "input";

  printf("Part 1 Result: %ld\n", part1(filepath));
  printf("Part 2 Result: %ld\n", part2(filepath));

  return EXIT_SUCCESS;
}

long part1(const char *filepath){
  struct split_result split1, seeds;
  char *first_line = read_from_file(filepath);
  split1 = split(first_line, ":");
  seeds = split(ltrim(split1.split[1]), " ");
  
  long result = 0, i, location;
 
  for(i = 0; i < seeds.count; i++){
    long seed = atol(seeds.split[i]);
    location = walk_mappings(seed, 1);
    if (location < result || result == 0)
        result = location;
  } 

  dispose_split_result(seeds);
  dispose_split_result(split1);
  destroy_lists();
  return result;
}

long part2(const char *filepath){
  struct split_result split1, seeds;
  char *first_line = read_from_file(filepath);
  split1 = split(first_line, ":");
  seeds = split(ltrim(split1.split[1]), " ");

  long result = 0, location;
  int i = 0, iterations_remaining = seeds.count / 2;
  while(iterations_remaining-- > 0){
    long seed = atol(seeds.split[i]);
    long range = atol(seeds.split[i+1]);
    location = walk_mappings(seed, range);
    if (location < result || result == 0)
      result = location;

    i += 2;
  }

  dispose_split_result(seeds);
  dispose_split_result(split1);
  destroy_lists();
  return result;
}

struct dest_range {
  long dest;
  long range;
  struct dest_range *next;
};

void destroy_dest_list(struct dest_range *list){
  struct dest_range *p;

  while(list){
    p = list;
    list = list->next;
    free(p);
  }
}

struct dest_range *add_range_to_list(struct dest_range *list, struct dest_range *item){
  struct dest_range *cur, *prev;
  if (list == NULL)
    return item;

  for (cur = list, prev = NULL; cur != NULL && cur->dest < item->dest; prev = cur, cur = cur->next);

  if (prev == NULL){
    item->next = cur;
    list = item;
  } 
  else if (cur == NULL)
    prev->next = item;
  else{
    prev->next = item;
    item->next = cur;
  }

  return list;
}

struct dest_range *get_destintation(struct dest_range *sources, struct node *list){
  struct dest_range *destinations = NULL, *src_list = sources;
  struct node *cur;
  long src, range;

  while (src_list) {
    src = src_list->dest;
    range = src_list->range;
    while (range > 0){
      struct dest_range *dr = NULL;
      dr = m_alloc(dr, sizeof(struct dest_range), "Unable to allocate destination range");
      dr->next = NULL;

      for (cur = list;
        cur != NULL && cur->src + cur->range <= src;
        cur = cur->next);

      if(cur == NULL){
        dr->dest = src;
        dr->range = range;
      }
      else if (src < cur->src) {
        dr->dest = src;
        if (src + range <= cur->src)
          dr->range = range;
        else
          dr->range = cur->src - src;
      }
      else{
        dr->dest = cur->dest + (src - cur->src);
        if (range + src <= cur->range + cur->src)
          dr->range = range;
        else
          dr->range = cur->range + cur->src - src;
      }
      destinations = add_range_to_list(destinations, dr);
      // printf("Consumed: %ld\n", dr->range); // this indicates the number of iterations short cutted
      range -= dr->range;
      src += dr->range;
    }
    src_list = src_list->next;
  }
  return destinations;
}

long walk_mappings(long seed, long range){
  struct dest_range *init = NULL, *soil, *fert, *water, *light, *temp, *humidity, *location;

  init = m_alloc(init, sizeof(struct dest_range), "Unable to allocate initial dest range");
  init->next = NULL;
  init->dest = seed;
  init->range = range;

  soil = get_destintation(init, s_s_src);
  destroy_dest_list(init);
  fert = get_destintation(soil, s_f_src);
  destroy_dest_list(soil);
  water = get_destintation(fert, f_w_src);
  destroy_dest_list(fert);
  light = get_destintation(water, w_l_src);
  destroy_dest_list(water);
  temp = get_destintation(light, l_t_src);
  destroy_dest_list(light);
  humidity = get_destintation(temp, t_h_src);
  destroy_dest_list(temp);
  location = get_destintation(humidity, h_l_src);
  destroy_dest_list(humidity);

  long loc = location->dest;
  destroy_dest_list(location);
  printf("Found location: %ld\n", loc);
  return loc;
}

void read_mappings(struct node **src_list){
  read = getline(&line, &len, fptr);
  while (strcmp(line, "\n") != 0){
    process_line(src_list, line);
    read = getline(&line, &len, fptr);
  }
}

char *read_from_file(const char *filepath){
  char *first_line = NULL;

  fptr = fopen(filepath, "r");
  read = getline(&line, &len, fptr);

  first_line = m_alloc(first_line, strlen(line) + 1, "Unable to allocate memory for fisrt line");
  strcpy(first_line, line);
  printf("First line: %s\n", first_line);
  while ((read = getline(&line, &len, fptr)) != -1) {
    while (strcmp(line, "\n") != 0){
      switch (line[0]){
        case 's':
          if (line[1] == 'e'){
            read_mappings(&s_s_src);
          }
          else{
            read_mappings(&s_f_src);
          }
          break;
        case 'f':
          read_mappings(&f_w_src);
          break; 
        case 'w':
          read_mappings(&w_l_src);
          break; 
        case 'l':
          read_mappings(&l_t_src);
          break; 
        case 't':
          read_mappings(&t_h_src);
          break; 
        case 'h':
          read_mappings(&h_l_src);
          break; 
      }
      read = getline(&line, &len, fptr);
    }
  }

  fclose(fptr);
  return first_line;
}

void destroy_lists(){
  destroy(s_s_src);
  destroy(s_f_src);
  destroy(f_w_src);
  destroy(w_l_src);
  destroy(l_t_src);
  destroy(t_h_src);
  destroy(h_l_src);

  s_s_src = NULL;
  s_f_src = NULL;
  f_w_src = NULL;
  w_l_src = NULL;
  l_t_src = NULL;
  t_h_src = NULL;
  h_l_src = NULL;
}
