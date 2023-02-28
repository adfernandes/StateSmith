// Generated state machine
public class TestsMySm1
{
public enum EventId
{
    DO = 0, // The `do` event is special. State event handlers do not consume this event (ancestors all get it too) unless a transition occurs.
}

public const int EventIdCount = 1;

public enum StateId
{
    ROOT = 0,
    S1 = 1,
    S2 = 2,
}

public const int StateIdCount = 3;

// event handler type
private delegate void Func(TestsMySm1 sm);

    // Used internally by state machine. Feel free to inspect, but don't modify.
    public StateId state_id;
    
    // Used internally by state machine. Don't modify.
    private Func? ancestor_event_handler;
    
    // Used internally by state machine. Don't modify.
    private readonly Func?[] current_event_handlers = new Func[EventIdCount];
    
    // Used internally by state machine. Don't modify.
    private Func? current_state_exit_handler;

// State machine variables. Can be used for inputs, outputs, user variables...
public struct Vars
{
    public bool b;
    //>>>>>ECHO://This is super cool!
    //>>>>>ECHO:byte x;
}
    
    // Variables. Can be used for inputs, outputs, user variables...
    public Vars vars;

// State machine constructor. Must be called before start or dispatch event functions. Not thread safe.
public TestsMySm1()
{
}

// Starts the state machine. Must be called before dispatching events. Not thread safe.
public void start()
{
    ROOT_enter(this);
    // ROOT behavior
    // uml: TransitionTo(ROOT.InitialState)
    {
        // Step 1: Exit states until we reach `ROOT` state (Least Common Ancestor for transition). Already at LCA, no exiting required.
        
        // Step 2: Transition action: ``.
        
        // Step 3: Enter/move towards transition target `ROOT.InitialState`.
        // ROOT.InitialState is a pseudo state and cannot have an `enter` trigger.
        
        // ROOT.InitialState behavior
        // uml: / { self->vars.b = false; } TransitionTo(S1)
        {
            // Step 1: Exit states until we reach `ROOT` state (Least Common Ancestor for transition). Already at LCA, no exiting required.
            
            // Step 2: Transition action: `self->vars.b = false;`.
            //>>>>>ECHO:self->vars.b = false;
            
            // Step 3: Enter/move towards transition target `S1`.
            S1_enter(this);
            
            // Step 4: complete transition. Ends event dispatch. No other behaviors are checked.
            this.state_id = StateId.S1;
            // No ancestor handles event. Can skip nulling `ancestor_event_handler`.
            return;
        } // end of behavior for ROOT.InitialState
    } // end of behavior for ROOT
}

// Dispatches an event to the state machine. Not thread safe.
public void dispatch_event(EventId event_id)
{
    Func? behavior_func = this.current_event_handlers[(int)event_id];
    
    while (behavior_func != null)
    {
        this.ancestor_event_handler = null;
        behavior_func(this);
        behavior_func = this.ancestor_event_handler;
    }
}

// This function is used when StateSmith doesn't know what the active leaf state is at
// compile time due to sub states or when multiple states need to be exited.
private static void exit_up_to_state_handler(TestsMySm1 sm, Func desired_state_exit_handler)
{
    while (sm.current_state_exit_handler != desired_state_exit_handler)
    {
        sm.current_state_exit_handler!(sm);
    }
}


////////////////////////////////////////////////////////////////////////////////
// event handlers for state ROOT
////////////////////////////////////////////////////////////////////////////////

private static void ROOT_enter(TestsMySm1 sm)
{
    // setup trigger/event handlers
    sm.current_state_exit_handler = ROOT_exit;
}

private static readonly Func ROOT_exit = (TestsMySm1 sm) =>
{
    // State machine root is a special case. It cannot be exited.
};


////////////////////////////////////////////////////////////////////////////////
// event handlers for state S1
////////////////////////////////////////////////////////////////////////////////

private static void S1_enter(TestsMySm1 sm)
{
    // setup trigger/event handlers
    sm.current_state_exit_handler = S1_exit;
    sm.current_event_handlers[(int)EventId.DO] = S1_do;
}

private static readonly Func S1_exit = (TestsMySm1 sm) =>
{
    // adjust function pointers for this state's exit
    sm.current_state_exit_handler = ROOT_exit;
    sm.current_event_handlers[(int)EventId.DO] = null;  // no ancestor listens to this event
};

private static readonly Func S1_do = (TestsMySm1 sm) =>
{
    // No ancestor state handles `do` event.
    
    // S1 behavior
    // uml: do TransitionTo(S2)
    {
        // Step 1: Exit states until we reach `ROOT` state (Least Common Ancestor for transition).
        S1_exit(sm);
        
        // Step 2: Transition action: ``.
        
        // Step 3: Enter/move towards transition target `S2`.
        S2_enter(sm);
        
        // Step 4: complete transition. Ends event dispatch. No other behaviors are checked.
        sm.state_id = StateId.S2;
        // No ancestor handles event. Can skip nulling `ancestor_event_handler`.
        return;
    } // end of behavior for S1
};


////////////////////////////////////////////////////////////////////////////////
// event handlers for state S2
////////////////////////////////////////////////////////////////////////////////

private static void S2_enter(TestsMySm1 sm)
{
    // setup trigger/event handlers
    sm.current_state_exit_handler = S2_exit;
}

private static readonly Func S2_exit = (TestsMySm1 sm) =>
{
    // adjust function pointers for this state's exit
    sm.current_state_exit_handler = ROOT_exit;
};

    public bool ____GilNoEmit_echoStringBool(string toEcho) { return true; }
}
