#nullable enable

using FluentAssertions;
using StateSmith.Output;
using StateSmith.SmGraph;
using System.IO;
using System.Text;
using Xunit;

namespace StateSmithTest.Output.SmDescriberTest;

public class SmDescriberTests
{
    [Fact]
    public void Test()
    {
        var sb = new StringBuilder();
        SmGraphDescriber smDescriber = new(new StringWriter(sb));
        StateMachine sm = BuildTestSm();
        smDescriber.Describe(sm);

        bool debugShowFullText = false;
        if (debugShowFullText)
            sb.ToString().Should().Be("");

        sb.ToString().ShouldBeShowDiff("""
            Vertex: ROOT
            -----------------------------------------
            - Type: StateMachine
            - Diagram Id: 123

            ### Behaviors
                enter / { sm_enter(); }

                ev1 / { sm_ev1_stuff(); }


            Vertex: S1
            -----------------------------------------
            - Parent: ROOT
            - Type: State
            - Diagram Id: 456

            ### Behaviors
                enter / { s1_enter(); }

                ev1 / { s1_ev1_stuff(); }

                =========== from ancestor ROOT ===========

                ev1 / { sm_ev1_stuff(); }

            """);
    }

    private static StateMachine BuildTestSm()
    {
        var sm = new StateMachine("MySm")
        {
            DiagramId = "123"
        };
        sm.AddEnterAction("sm_enter();");
        sm.AddBehavior(new Behavior(trigger: "ev1", actionCode: "sm_ev1_stuff();"));

        //---------------------------

        var s1 = sm.AddChild(new State("S1")
        {
            DiagramId = "456",
        });
        s1.AddEnterAction("s1_enter();");
        s1.AddBehavior(new Behavior(trigger: "ev1", actionCode: "s1_ev1_stuff();"));
        return sm;
    }
}
