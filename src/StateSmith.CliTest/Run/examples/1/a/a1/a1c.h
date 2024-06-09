// Autogenerated with StateSmith 0.10.0-alpha+ab8c57ce711a699bdd49c7944b94ef4dfbf80060.
// Algorithm: Balanced1. See https://github.com/StateSmith/StateSmith/wiki/Algorithms

#pragma once
#include <stdint.h>

typedef enum a1c_EventId
{
    a1c_EventId_DO = 0, // The `do` event is special. State event handlers do not consume this event (ancestors all get it too) unless a transition occurs.
    a1c_EventId_MY_EVENT_1 = 1,
    a1c_EventId_MY_EVENT_2 = 2,
} a1c_EventId;

enum
{
    a1c_EventIdCount = 3
};

typedef enum a1c_StateId
{
    a1c_StateId_ROOT = 0,
    a1c_StateId_STATE_1 = 1,
    a1c_StateId_STATE_2 = 2,
} a1c_StateId;

enum
{
    a1c_StateIdCount = 3
};


// Generated state machine
// forward declaration
typedef struct a1c a1c;

// event handler type
typedef void (*a1c_Func)(a1c* sm);

// State machine constructor. Must be called before start or dispatch event functions. Not thread safe.
void a1c_ctor(a1c* sm);

// Starts the state machine. Must be called before dispatching events. Not thread safe.
void a1c_start(a1c* sm);

// Dispatches an event to the state machine. Not thread safe.
void a1c_dispatch_event(a1c* sm, a1c_EventId event_id);

// Thread safe.
char const * a1c_state_id_to_string(a1c_StateId id);

// Thread safe.
char const * a1c_event_id_to_string(a1c_EventId id);

// Generated state machine
struct a1c
{
    // Used internally by state machine. Feel free to inspect, but don't modify.
    a1c_StateId state_id;
    
    // Used internally by state machine. Don't modify.
    a1c_Func ancestor_event_handler;
    
    // Used internally by state machine. Don't modify.
    a1c_Func current_event_handlers[a1c_EventIdCount];
    
    // Used internally by state machine. Don't modify.
    a1c_Func current_state_exit_handler;
};

