// Autogenerated with StateSmith
#pragma once
#include <stdint.h>

enum Ex2_EventId
{
    Ex2_EventId_DO = 0, // The `do` event is special. State event handlers do not consume this event (ancestors all get it too) unless a transition occurs.
    Ex2_EventId_EV2 = 1,
    Ex2_EventId_MYEV1 = 2,
};

enum
{
    Ex2_EventIdCount = 3
};

enum Ex2_StateId
{
    Ex2_StateId_ROOT = 0,
    Ex2_StateId_STATE_1 = 1,
    Ex2_StateId_STATE_2 = 2,
};

enum
{
    Ex2_StateIdCount = 3
};

typedef struct Ex2 Ex2;
typedef void (*Ex2_Func)(Ex2* sm);

struct Ex2
{
    // Used internally by state machine. Feel free to inspect, but don't modify.
    enum Ex2_StateId state_id;
    
    // Used internally by state machine. Don't modify.
    Ex2_Func ancestor_event_handler;
    
    // Used internally by state machine. Don't modify.
    Ex2_Func current_event_handlers[Ex2_EventIdCount];
    
    // Used internally by state machine. Don't modify.
    Ex2_Func current_state_exit_handler;
};

// State machine constructor. Must be called before start or dispatch event functions. Not thread safe.
void Ex2_ctor(Ex2* self);

// Starts the state machine. Must be called before dispatching events. Not thread safe.
void Ex2_start(Ex2* self);

// Dispatches an event to the state machine. Not thread safe.
void Ex2_dispatch_event(Ex2* self, enum Ex2_EventId event_id);

// Converts a state id to a string. Thread safe.
const char* Ex2_state_id_to_string(const enum Ex2_StateId id);

// Converts an event id to a string. Thread safe.
const char* Ex2_event_id_to_string(const enum Ex2_EventId id);