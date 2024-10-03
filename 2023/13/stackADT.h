#ifndef STACKADT_H
#define STACKADT_H

#include <stdbool.h>

typedef struct stack_type *Stack;

Stack stack_create(int size);
void destroy_stack(Stack s);
void stack_make_empty(Stack s);
bool stack_is_empty(Stack s);
bool stack_is_full(Stack s);
int stack_length(Stack s);

void stack_push(Stack s, char *str);
char *stack_pop(Stack s);
char *stack_peek(Stack s);

#endif // !DEBUG
