#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include "helper.h"

struct condition_record{
  char *cfg;
  int *nums;
  int num_len;
};

struct condition_record *records;
int records_len = 0;

struct node {
  char *cfg;
  int *nums;
  int nums_len;
  long result;
  struct node *next;
};

struct node *cache = NULL;

void add_to_cache(char *cfg, int *nums, int nums_len, long result){
  struct node *new_node = NULL;
  new_node = m_alloc(new_node, sizeof(struct node), "Unable to allocate memory for cache node");
  new_node->cfg = NULL;
  new_node->cfg = m_alloc(new_node->cfg, strlen(cfg) + 1, "Unable to allocate memory for cache cfg");
  strcpy(new_node->cfg, cfg);
  new_node->nums = NULL;
  new_node->nums = m_alloc(new_node->nums, sizeof(int) * nums_len, "Unable to allocate memory for cache nums");
  for(int i = 0; i < nums_len; i++)
    new_node->nums[i] = nums[i];
  new_node->nums_len = nums_len;
  new_node->result = result;
  new_node->next = NULL;

  if (cache == NULL){
    cache = new_node;
    return;
  }

  new_node->next = cache;
  cache = new_node;
}

void dispose_cache(){
  struct node *temp;

  while(cache != NULL){
    temp = cache;
    cache = cache->next;
    free(temp->cfg);
    free(temp->nums);
    free(temp);
  }
}

bool cache_compare(const struct node *item, const char *cfg, const int *nums, const int nums_len){
  if(item->nums_len != nums_len)
    return false;
  if(strcmp(item->cfg, cfg) != 0)
    return false;
  for (int i = 0; i < nums_len; i++)
    if (item->nums[i] != nums[i])
      return false;

  return true;
}

struct node *get_from_cache(const char* cfg, const int *nums, const int nums_len){
  struct node *cur, *prev;

  for (cur = cache, prev = NULL; cur != NULL && !cache_compare(cur, cfg, nums, nums_len); prev = cur, cur = cur->next);
  if (cur == NULL)
    return NULL;

  if (prev == NULL)
    return cur;

  // set the found item to the from of the linked list
  prev->next = cur->next;
  cur->next = cache;
  cache = cur;

  return cur;
}

long count(char *cfg, int nums[], int num_len){
  long result;
  struct node *cache_item;

  if (cfg[0] == '\0'){
    return num_len == 0 ? 1 : 0;
  }
  
  if (num_len == 0){
    return str_contains_char(cfg, '#') > 0 ? 0 : 1;
  }

  cache_item = get_from_cache(cfg, nums, num_len);
  if(cache_item != NULL)
    return cache_item->result;

  result = 0;

  if (str_contains_char(".?", cfg[0]) > 0)
    result += count(cfg + 1, nums, num_len);

  if (str_contains_char("#?", cfg[0]) > 0){
    int str_len = (int)strlen(cfg);
    if (nums[0] <= str_len){
      char slice[nums[0] + 1];
      strncpy(slice, cfg, nums[0]);
      slice[nums[0]] = '\0';
      if (str_contains_char(slice, '.') == 0){
        if (nums[0] == str_len){
          result += count(cfg + nums[0], nums + 1, num_len - 1);
        }
        else if (cfg[nums[0]] != '#'){
          result += count(cfg + nums[0] + 1, nums + 1, num_len - 1);
        }
      }
    }
  }

  add_to_cache(cfg, nums, num_len, result);
  return result;
}

void process_input(char *lines[], int n){
  struct split_result split1, split2;
  int i, j, len;
  char *cfg;
  int *nums;
  records = NULL;

  for (i = 0; i < n; i++){
    cfg = NULL;
    nums = NULL;

    split1 = split(lines[i], " ");
    len = strlen(split1.split[0]);
    cfg = m_alloc(cfg, len + 1, "Unable to allocate memory for record");
    strcpy(cfg, split1.split[0]);
    cfg[len] = '\0';

    split2 = split(split1.split[1], ",");
    nums = m_alloc(nums, sizeof(int) * (split2.count), "Unable to allocate memory for config");
    for (j = 0; j < split2.count; j++){
      nums[j] = atoi(split2.split[j]);
    }

    records = m_alloc(records, sizeof(struct condition_record) * (records_len + 1), "Unable to allocate memory for condition record");
    records[records_len].cfg = cfg;
    records[records_len].nums = nums;
    records[records_len].num_len = split2.count;
    records_len++;
  }
}

void expand_records(){
  char *r_ptr;
  int i, j, str_len, num_len;

  for(i = 0; i < records_len; i++){
    r_ptr = records[i].cfg;
    str_len = strlen(r_ptr);
    char orig_text[str_len + 1];
    strcpy(orig_text, r_ptr);
    orig_text[str_len] = '\0';

    records[i].cfg = m_alloc(records[i].cfg, (str_len * 5) + 5, "Unable to allocate memory for expanded record");
    r_ptr = records[i].cfg;

    for(j = 1; j < 5; j++){
      r_ptr += str_len;
      r_ptr[0] = '?';
      r_ptr += 1;
      strcpy(r_ptr, orig_text);
    }
    r_ptr[str_len] = '\0';
  
    num_len = records[i].num_len;
    records[i].nums = m_alloc(records[i].nums, sizeof(int) * num_len * 5, "Unable to allocate memory for expanded config");
    for(j = num_len; j < 5 * num_len; j++){
      records[i].nums[j] = records[i].nums[j - num_len];
    }
    records[i].num_len *= 5;
  }
}

void dispose_records(){
  int n = 0;

  while (n < records_len){
    free(records[n].cfg);
    free(records[n].nums);
    n++;
  }
  records = NULL;
  records_len = 0;
}

long part1(char *lines[], int line_count){
  long tally = 0, record_tally = 0;

  process_input(lines, line_count);

  for(int i = 0; i < line_count; i++){
    record_tally = count(records[i].cfg, records[i].nums, records[i].num_len);
    tally +=  record_tally;
    dispose_cache();
  }

  return tally;
}

long part2(int line_count){
  long tally = 0, record_tally = 0;

  expand_records();

  for(int i = 0; i < line_count; i++){
    record_tally = count(records[i].cfg, records[i].nums, records[i].num_len);
    tally +=  record_tally;
    dispose_cache();
  }

  dispose_records();

  return tally;
}

void solve_file(char *file_path){
  char *lines[INIT_LINES_SIZE];

  int line_count = read_file(lines, file_path);

  printf("Part 1 Result: %ld\n", part1(lines, line_count));
  printf("Part 2 Result: %ld\n", part2(line_count));

  dispose_read_file(lines, line_count);
}

int main(int argc, char *argv[]){
  for (int i = 1; i < argc; ++i) {
    solve_file(argv[i]);
  }
  return EXIT_SUCCESS;
}


