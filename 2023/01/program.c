#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"
#define MAX_LINE_BUFFER 100

const char *digits[][2] = {
  { "one",    "1" }, 
  { "two",    "2" },
  { "three",  "3" },
  { "four",   "4" },
  { "five",   "5" },
  { "six",    "6" },
  { "seven",  "7" },
  { "eight",  "8" },
  { "nine",   "9" },
};

char *prepare_str(char *str);
void str_replace(char *orig, const char *rep, const char *with);
int part1(char *lines[], int line_count);
int part2(char *lines[], int line_count);

int main(void){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, "input");

  printf("Part 1 Result: %d\n", part1(lines, line_count));

  line_count = read_file(lines, "input");
  printf("Part 2 Result: %d\n", part2(lines, line_count));

  dispose(lines, line_count);

  return EXIT_SUCCESS;
}

int part1(char *lines[], int line_count){
  int i, j, first, last, tally = 0;

  for(i = 0; i < line_count; i++){
    j = 0;
    while(lines[i][j] != '\0'){
      if (isdigit(lines[i][j])){
        first = lines[i][j] - '0';
        break;
      }
      j++;
    }

    j = strlen(lines[i]);
    while (j >= 0){
      if (isdigit(lines[i][j])){
        last = lines[i][j] - '0';
        break;
      }
      j--;
    }

    tally += first * 10;
    tally += last;
  }

  return tally;
}

int part2(char *lines[], int line_count){
  int i, j, first_digit, last_digit, tally = 0;
  char line_buffer[MAX_LINE_BUFFER];

  for(i = 0; i < line_count; i++){
    // printf("Orig string: %s\n", lines[i]);
    strcpy(line_buffer, prepare_str(lines[i]));
    // printf("Prepared string: %s\n", line_buffer);

    j = 0;
    while(line_buffer[j] != '\0'){
      if (isdigit(line_buffer[j])){
        first_digit = line_buffer[j] - '0';
        break;
      }
      j++;
    }

    j = strlen(line_buffer);
    while (j >= 0){
      if (isdigit(line_buffer[j])){
        last_digit = line_buffer[j] - '0';
        break;
      }
      j--;
    }

    // printf("Digits: %d%d\n", first_digit, last_digit);
    tally += first_digit * 10;
    tally += last_digit;
  }

  return tally;
}

char *prepare_str(char *str){
  int i, j, len;
  char temp[MAX_LINE_BUFFER];
  char *ch_ptr, *orig;
  bool replacement_occurred = false;

  do {
    len = strlen(str);
    replacement_occurred = false;
    for(j = 3; j <= len; j++){
      orig = str;
      strncpy(temp, orig, j);
      temp[j] = '\0';
      for (i = 0; i < 9; i++){
        ch_ptr = strstr(temp, digits[i][0]);
        if (ch_ptr != NULL){
          str_replace(temp, digits[i][0], digits[i][1]);
          orig += j;
          strcat(temp, orig);
          strcpy(str, temp);
          replacement_occurred = true;
          break;
        }
      }
      if(replacement_occurred)
        break;
    }
  } while(replacement_occurred);
  return str;
}

void str_replace(char *orig, const char *rep, const char *with) {
    char *orig_ptr = orig; // pointer to orig str
    char *ins;    // the next insert point
    char tmp[MAX_LINE_BUFFER];    // varies
    int len_rep;  // length of rep (the string to remove)
    int len_with; // length of with (the string to replace rep with)
    int len_front; // distance between rep and end of last rep
    int count;    // number of replacements

    // sanity checks and initialization
    if (!orig || !rep)
        return;
    len_rep = strlen(rep);
    if (len_rep == 0)
        return; // empty rep causes infinite loop during count
    if (!with)
        with = "";
    len_with = strlen(with);
       
    // first time through the loop, all the variable are set correctly
    // from here on,
    //    tmp points to the end of the result string
    //    ins points to the next occurrence of rep in orig
    //    orig points to the remainder of orig after "end of rep"
    if ((ins = strstr(orig, rep)) != NULL) {
        len_front = ins - orig;
        strncpy(tmp, orig, len_front);
        tmp[len_front] = '\0';
        strcat(tmp, with);
        orig_ptr += len_front + len_rep; // move to next "end of rep"
        strcat(tmp, orig_ptr);
        strcpy(orig, tmp);
    }
}
