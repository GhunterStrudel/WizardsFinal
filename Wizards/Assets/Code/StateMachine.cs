using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StateMachine
{
    public enum Command
    {
        HealthLow,
        HealthMediumLow,
        HealthMediumHigh,
        HealthHigh
    };

    class StateTransition
    {
        readonly ElementType CurrentState;
        readonly Command Command;

        public StateTransition(ElementType currentState, Command command)
        {
            CurrentState = currentState;
            Command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
        }
    }

    Dictionary<StateTransition, ElementType> transitions;
    public ElementType CurrentState { get; private set; }

    public StateMachine(ElementType type)
    {
        CurrentState = type;
        transitions = new Dictionary<StateTransition, ElementType>
        {
            { new StateTransition(ElementType.Light, Command.HealthLow), ElementType.Fire },
            { new StateTransition(ElementType.Light, Command.HealthMediumLow), ElementType.Air },
            { new StateTransition(ElementType.Light, Command.HealthMediumHigh), ElementType.Ice },
            { new StateTransition(ElementType.Light, Command.HealthHigh), ElementType.Earth },

            { new StateTransition(ElementType.Dark, Command.HealthLow), ElementType.Fire },
            { new StateTransition(ElementType.Dark, Command.HealthMediumLow), ElementType.Air },
            { new StateTransition(ElementType.Dark, Command.HealthMediumHigh), ElementType.Ice },
            { new StateTransition(ElementType.Dark, Command.HealthHigh), ElementType.Earth },

            { new StateTransition(ElementType.Fire, Command.HealthLow), ElementType.Light },
            { new StateTransition(ElementType.Fire, Command.HealthMediumLow), ElementType.Ice },
            { new StateTransition(ElementType.Fire, Command.HealthMediumHigh), ElementType.Earth },
            { new StateTransition(ElementType.Fire, Command.HealthHigh), ElementType.Dark },

            { new StateTransition(ElementType.Air, Command.HealthLow), ElementType.Light },
            { new StateTransition(ElementType.Air, Command.HealthMediumLow), ElementType.Fire },
            { new StateTransition(ElementType.Air, Command.HealthMediumHigh), ElementType.Ice },
            { new StateTransition(ElementType.Air, Command.HealthHigh), ElementType.Dark },

            { new StateTransition(ElementType.Ice, Command.HealthLow), ElementType.Light },
            { new StateTransition(ElementType.Ice, Command.HealthMediumLow), ElementType.Air },
            { new StateTransition(ElementType.Ice, Command.HealthMediumHigh), ElementType.Earth },
            { new StateTransition(ElementType.Ice, Command.HealthHigh), ElementType.Dark },

            { new StateTransition(ElementType.Earth, Command.HealthLow), ElementType.Light },
            { new StateTransition(ElementType.Earth, Command.HealthMediumLow), ElementType.Fire },
            { new StateTransition(ElementType.Earth, Command.HealthMediumHigh), ElementType.Air },
            { new StateTransition(ElementType.Earth, Command.HealthHigh), ElementType.Dark }
        };
    }

    public ElementType GetNext(Command command)
    {
        StateTransition transition = new StateTransition(CurrentState, command);
        ElementType nextState;
        if (!transitions.TryGetValue(transition, out nextState))
            throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
        return nextState;
    }

    public ElementType MoveNext(Command command)
    {
        CurrentState = GetNext(command);
        return CurrentState;
    }
}
