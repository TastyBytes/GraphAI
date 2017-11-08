using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GraphAI
{
    public class Path
    {
        public Path( IAction[] actions, IState[] states )
        {
            if( (states?.Length ?? 0) == 0 )
                throw new ArgumentException("No states!");

            if( (actions?.Length ?? 0) == 0 )
            {
                if( states.Length != 1 )
                    throw new ArgumentException("No actions!");
                else
                    actions = new IAction[0];
            }

            if( actions.Where(a => a == null).Count() != 0 )
                throw new ArgumentException("null action found!");

            if( states.Where(a => a == null).Count() != 0 )
                throw new ArgumentException("null state found!");

            this.Actions = new ReadOnlyCollection<IAction>((IAction[])actions.Clone());
            this.States = new ReadOnlyCollection<IState>((IState[])states.Clone());
        }

        public ReadOnlyCollection<IAction> Actions { get; }
        public ReadOnlyCollection<IState> States { get; }

        public IState Start => this.States[0];
        public IState End => this.States[this.States.Count - 1];
        public int Length => this.States.Count - 1;

        public IEnumerable<object> GetStatesAndActions()
        {
            for( int i = 0; i < this.States.Count; ++i )
            {
                if( i == 0 )
                {
                    yield return this.States[0];
                }
                else
                {
                    yield return this.Actions[i - 1];
                    yield return this.States[i];
                }
            }
        }
    }
}
