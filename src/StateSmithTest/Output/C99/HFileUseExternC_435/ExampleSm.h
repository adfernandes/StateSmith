// Autogenerated with StateSmith 0.17.2+ed91eced77d7b24791b16b68b32d1fb19e0d5d12.
// Algorithm: Balanced2. See https://github.com/StateSmith/StateSmith/wiki/Algorithms

#pragma once  // You can also specify normal include guard. See https://github.com/StateSmith/StateSmith/blob/main/docs/settings.md
#include <stdint.h>


#ifdef __cplusplus
extern "C" {
#endif

typedef enum ExampleSm_EventId
{
    ExampleSm_EventId_EV1 = 0
} ExampleSm_EventId;

enum
{
    ExampleSm_EventIdCount = 1
};

typedef enum ExampleSm_StateId
{
    ExampleSm_StateId_ROOT = 0,
    ExampleSm_StateId_S1 = 1
} ExampleSm_StateId;

enum
{
    ExampleSm_StateIdCount = 2
};


// Generated state machine
// forward declaration
typedef struct ExampleSm ExampleSm;

// State machine constructor. Must be called before start or dispatch event functions. Not thread safe.
void ExampleSm_ctor(ExampleSm* sm);

// Starts the state machine. Must be called before dispatching events. Not thread safe.
void ExampleSm_start(ExampleSm* sm);

// Dispatches an event to the state machine. Not thread safe.
// Note! This function assumes that the `event_id` parameter is valid.
void ExampleSm_dispatch_event(ExampleSm* sm, ExampleSm_EventId event_id);

// Thread safe.
char const * ExampleSm_state_id_to_string(ExampleSm_StateId id);

// Thread safe.
char const * ExampleSm_event_id_to_string(ExampleSm_EventId id);

// Generated state machine
struct ExampleSm
{
    // Used internally by state machine. Feel free to inspect, but don't modify.
    ExampleSm_StateId state_id;
};


#ifdef __cplusplus
}  // extern "C"
#endif
