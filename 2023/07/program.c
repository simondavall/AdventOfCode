#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "helper.h"
#define INPUT_FILE "input"
#define HAND_LEN 5
#define MAX_HANDS 1000

struct hand{
  char cards[HAND_LEN + 1];
  int rank;
  int bid;
};

struct hand hands[MAX_HANDS];

enum hand_rank{
  FIVE_OF_A_KIND,
  FOUR_OF_A_KIND,
  FULL_HOUSE,
  THREE_OF_A_KIND,
  TWO_PAIRS,
  ONE_PAIR,
  HIGH_CARD,
};


int cardRank[100] = { [50] = 2, 3, 4, 5, 6, 7, 8, 9, [65] = 14, [74] = 11, [75] = 13, [81] = 12, [84] = 10 };
int cardRank2[100] = { [50] = 2, 3, 4, 5, 6, 7, 8, 9, [65] = 14, [74] = 1, [75] = 13, [81] = 12, [84] = 10 };

int get_rank(char *cards);
int get_rank2(char *cards);
int hand_comparer(const void *hand1, const void *hand2);
int hand_comparer2(const void *hand1, const void *hand2);
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
  struct split_result split1;
  struct hand *hand; 
  int i;
 
  for (i = 0; i < line_count; i++) {
    char line[strlen(lines[i])];
    strcpy(line, lines[i]);
    split1 = split(line, " ");
    if (split1.count != 2){
      printf("Incorrect read from file. Count read: %d\n", split1.count);
      exit(EXIT_FAILURE);
    }
    hand = NULL;
    hand = m_alloc(hand, sizeof(struct hand), "Unable to allocate memory for hand");
    strcpy(hand->cards, split1.split[0]);
    hand->cards[HAND_LEN] = '\0';
    hand->rank = get_rank(hand->cards);
    hand->bid = atoi(split1.split[1]);

    dispose_split_result(split1);
    hands[i] = *hand;
    free(hand);
  }

  printf("\n");
  qsort(hands, MAX_HANDS, sizeof(struct hand), hand_comparer);
  long tally = 0;
  int winnings = 0;

  for (int i = 0; i < MAX_HANDS; i++){
    winnings = hands[i].bid * (i + 1);
    tally += winnings;
  }

  return tally;
}

int part2(char *lines[], int line_count){
  struct split_result split1;
  struct hand *hand; 
  int i;
 
  for (i = 0; i < line_count; i++) {
    char line[strlen(lines[i])];
    strcpy(line, lines[i]);
    split1 = split(line, " ");
    if (split1.count != 2){
      printf("Incorrect read from file. Count read: %d\n", split1.count);
      exit(EXIT_FAILURE);
    }
    hand = NULL;
    hand = m_alloc(hand, sizeof(struct hand), "Unable to allocate memory for hand");
    strcpy(hand->cards, split1.split[0]);
    hand->cards[HAND_LEN] = '\0';
    hand->rank = get_rank2(hand->cards);
    hand->bid = atoi(split1.split[1]);

    dispose_split_result(split1);
    hands[i] = *hand;
    free(hand);
  }

  qsort(hands, MAX_HANDS, sizeof(struct hand), hand_comparer2);
  long tally = 0;
  int winnings = 0;

  for (int i = 0; i < MAX_HANDS; i++){
    winnings = hands[i].bid * (i + 1);
    tally += winnings;
  }

  return tally;
}

int hand_comparer(const void *hand1, const void *hand2){
  struct hand h1 = *(struct hand *)hand1;
  struct hand h2 = *(struct hand *)hand2;

  if (h1.rank != h2.rank)
    return h2.rank - h1.rank;

  for (int i = 0; i < HAND_LEN; i++)
    if (h1.cards[i] != h2.cards[i])
      return cardRank[h1.cards[i]] - cardRank[h2.cards[i]];

  return 0;
}

int hand_comparer2(const void *hand1, const void *hand2){
  struct hand h1 = *(struct hand *)hand1;
  struct hand h2 = *(struct hand *)hand2;

  if (h1.rank != h2.rank)
    return h2.rank - h1.rank;

  for (int i = 0; i < HAND_LEN; i++)
    if (h1.cards[i] != h2.cards[i])
      return cardRank2[h1.cards[i]] - cardRank2[h2.cards[i]];

  return 0;
}

int get_rank(char *cards){
  char seen[HAND_LEN] = {0}, card;
  int counts[HAND_LEN] = {0};
  int i, j, seen_id = 0, count, different_cards = 0;

  for (i = 0; i < HAND_LEN; i++){
    char card = cards[i];
    if (contains_char(seen, HAND_LEN, card))
      continue;
    different_cards++;
    count = 0;
    seen[seen_id++] = card;
    for(j = i; j < HAND_LEN; j++)
      if (cards[j] == card) count++;

    if (count >= 4) return 5 - count; // found four and five card hands
    counts[i] = count;
  }

  if (different_cards == 5)
    return HIGH_CARD;
  if (different_cards == 4)
    return ONE_PAIR;
  if (different_cards == 2)
    return FULL_HOUSE;

  for(i = 0; i < HAND_LEN; i++){
    if (counts[i] == 3)
      return THREE_OF_A_KIND;
    if (counts[i] == 2)
      return TWO_PAIRS;
  }

  printf("Error: hand not valued. Card: %s, Diff: %d\n", cards, different_cards);
  exit(EXIT_FAILURE);
}

int get_rank2(char *cards){
  char seen[HAND_LEN] = {0}, card;
  int counts[HAND_LEN] = {0};
  int i, j, seen_id = 0, count, different_cards = 0;

  // sort out jacks
  int jacks = 0;
  for(j = 0; j < HAND_LEN; j++)
    if (cards[j] == 'J') jacks++;
  if (jacks > 0)
    seen[seen_id++] = 'J';

  for (i = 0; i < HAND_LEN; i++){
    char card = cards[i];
    if (contains_char(seen, HAND_LEN, card))
      continue;
    different_cards++;
    count = 0;
    seen[seen_id++] = card;
    for(j = i; j < HAND_LEN; j++)
      if (cards[j] == card) count++;

    if (count + jacks >= 4) return 5 - (count + jacks); // found four and five card hands
    counts[i] = count;
  }

  if (different_cards == 0)
    return FIVE_OF_A_KIND;
  if (different_cards == 5)
    return HIGH_CARD;
  if (different_cards == 4)
    return ONE_PAIR;
  if (different_cards == 2)
    return FULL_HOUSE;

  switch (jacks) {
    case 0:
      for(i = 0; i < HAND_LEN; i++){
        if (counts[i] == 3)
          return THREE_OF_A_KIND;
        if (counts[i] == 2)
          return TWO_PAIRS;
      }
      break;
    case 1:
      if (different_cards == 3){
        return THREE_OF_A_KIND;
      }
      if (different_cards == 4){
        return ONE_PAIR;
      }
      break;
    case 2:
      if (different_cards == 3){
        return THREE_OF_A_KIND;
      }
      break;
    default:
      printf("This case is not handled. Jacks count: %d, diff: %d\n", jacks, different_cards);
      break;
  }

  printf("Error: hand not valued. Card: %s, Diff: %d\n", cards, different_cards);
  exit(EXIT_FAILURE);
}
