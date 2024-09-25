#include <stdio.h>
#include <stdlib.h>
#include "helper.h"
#define MAX_INPUT_LEN 4
#define INPUT_FILE "input"

int time_ms[MAX_INPUT_LEN] = {0};
int dist_mm[MAX_INPUT_LEN] = {0};

void get_race_data(char *lines[]);
int get_factor(int n);
int min_time(long dist, long time);

int part1(char *lines[], int line_count);
int part2(char *lines[], int line_count);


int main(void){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, INPUT_FILE);


  printf("Part 1 Result: %d\n", part1(lines, line_count));
  printf("Part 2 Result: %d\n", part2(lines, line_count));

  dispose_read_file(lines, line_count);

  return EXIT_SUCCESS;
}

int part1(char *lines[], int line_count){
  int i, min, max;
  long tally = 1;
  get_race_data(lines);

  for(i = 0; i < MAX_INPUT_LEN; i++){
    if (time_ms[i] == 0){
      break;
    }
    min = min_time(dist_mm[i], time_ms[i]);
    max = time_ms[i] - min;
    tally *= (max - min + 1);
  }
  return tally;
}

int part2(char *lines[], int line_count){
  int i, min, max;
  long tally = 1, race_time = 0, race_dist = 0;
  get_race_data(lines);

  for(i = 0; i < MAX_INPUT_LEN; i++){
    if (time_ms[i] == 0){
      break;
    }
    if (race_time > 0){
      race_time *= get_factor(time_ms[i]);
      race_dist *= get_factor(dist_mm[i]);
    }
    race_time += time_ms[i];
    race_dist += dist_mm[i];
  }
  min = min_time(race_dist, race_time);
  max = race_time - min;
  tally *= (max - min + 1);

  return tally;
}

int get_factor(int n){
  int factor = 1;
  while(n / factor > 0)
    factor *= 10;
  return factor;
}

int min_time(long dist, long time){
  int t = 0;
  while (t < time){
    if (t * (time - t) > dist)
      break;
    t++;
  }
  return t;
}

void get_race_data(char *lines[]){
  struct split_result split_time, split_distance;
  int i;

  split_time = split(lines[0], " ");
  for(i = 1; i < split_time.count; i++)
    time_ms[i - 1] = atoi(split_time.split[i]);


  split_distance = split(lines[1], " ");
   for(i = 1; i < split_distance.count; i++)
    dist_mm[i - 1] = atoi(split_distance.split[i]);

  dispose_split_result(split_time);
  dispose_split_result(split_distance);

}

