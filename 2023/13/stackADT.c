#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "stackADT.h"
#include "helper.h"
#define STACK_SIZE 100

struct stack_type{
  char *contents[100];
  int top;
};

Stack stack_create(int size){
  Stack stack = NULL;
  stack = m_alloc(stack, sizeof(struct stack_type), "Unable to allocate memory for stack");
  stack->top = 0;
  return stack;
}

void destroy_stack(Stack stack){
  for (int i = 0; i < stack->top; i++)
    free(stack->contents[i]);
  free(stack);
}

void stack_make_empty(Stack stack){
  stack->top = 0;
}

bool stack_is_empty(Stack stack){
  return stack->top == 0;
}

bool stack_is_full(Stack stack){
  return stack->top == STACK_SIZE;
}

int stack_length(Stack stack){
  return stack->top;
}

void stack_push(Stack stack, char *str){
  char *ptr = stack->contents[stack->top];
  ptr = NULL;
  ptr = m_alloc(ptr, strlen(str) + 1, "Unable to allocate memory for new stack content");
  strcpy(ptr, str);
  stack->contents[stack->top++] = ptr;
}

char *stack_pop(Stack stack){
  if (stack_is_empty(stack)){
    return NULL;
  }
  int str_size = strlen(stack_peek(stack)) + 1;
  char *str = NULL;
  str = m_alloc(str, str_size, "Unable to allocate memory stack string");
  strcpy(str, stack_peek(stack));
  stack->top--;
  return str;
}

char *stack_peek(Stack stack){
  if (stack_is_empty(stack)){
    return NULL;
  }
  return stack->contents[stack->top - 1];
}
