#include <stdio.h>
#include <stdlib.h>
#include "helper.h"

int part1(char *lines[], int line_count);
int part2(char *lines[], int line_count);

int main(void){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, "input");

  for(int j = 0; j < line_count; j++)
    printf("Line %2d: '%s'\n", j, lines[j]);

  printf("Part 1 Result: %d\n", part1(lines, line_count));

  line_count = read_file(lines, "input");
  
  printf("Part 2 Result: %d\n", part2(lines, line_count));

  dispose_read_file(lines, line_count);

  return EXIT_SUCCESS;
}

int part1(char *lines[], int line_count){
  struct split_result split1, split2;
  char **p;
  int tally = 0;

  for(p = lines; p < lines + line_count; p++){
    split1 = split(*p, ":");
    char *all_cards = split1.split[1];
    int card_tally = 0;

    split2 = split(all_cards, "|");
    struct split_result winners = split(split2.split[0], " ");
    struct split_result numbers = split(split2.split[1], " ");

    while (numbers.count > 0) {
      if (contains(winners.split, winners.count, numbers.split[--numbers.count])){
        if(card_tally == 0)
          card_tally = 1;
        else
          card_tally *= 2;
      }
    }
    dispose_split_result(split2);
    dispose_split_result(split1);
    tally += card_tally;
  }
  return tally;
}

int part2(char *lines[], int line_count){
  struct split_result split1, split2;
  int card[line_count], i = 0, idx = 0, tally = 0;

  while(i < line_count)
    card[i++] = 1;
  
  for(idx = 0; idx < line_count; idx++){
    split1 = split(lines[idx], ":");
    char *all_cards = split1.split[1];
    split2 = split(all_cards, "|");
    struct split_result winners = split(split2.split[0], " ");
    struct split_result numbers = split(split2.split[1], " ");
    i = 1;
    while (numbers.count > 0) {
      if (contains(winners.split, winners.count, numbers.split[--numbers.count])){
        card[idx + i++] += card[idx];
      }
    }
    dispose_split_result(split2);
    dispose_split_result(split1);
  }
  
  idx = 0;
  while(idx < line_count)
    tally += card[idx++];

  return tally;
}

