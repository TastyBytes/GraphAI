using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphAI
{
    public abstract class SearchAlgorithmBase<TState, TAction>
        where TState : IState
        where TAction : IAction
    {
        #region Node

        protected class Node
        {
            internal Node( Node parent, TAction cause, TState state )
            {
                if( state == null )
                    throw new ArgumentNullException(nameof(state));

                this.Parent = parent;
                this.Cause = cause;
                this.State = state;
            }

            internal Node Parent { get; }
            internal TAction Cause { get; }
            internal TState State { get; }
            internal bool IsSeed => this.Parent == null;
        }

        #endregion

        #region Non-Public Fields

        private readonly IProblemSpace problemSpace;
        private readonly List<Node> nodesVisited;
        private Node goal;

        #endregion

        #region Constructor

        public SearchAlgorithmBase( IProblemSpace problem )
        {
            this.problemSpace = problem ?? throw new ArgumentNullException(nameof(problem));
            this.nodesVisited = new List<Node>();
        }

        #endregion

        #region Non-Public Static Methods

        private static Path GetPath( Node node )
        {
            if( node == null )
                return null;

            var actions = new List<IAction>();
            var states = new List<IState>();

            while( true )
            {
                states.Insert(0, node.State);

                if( node.IsSeed )
                {
                    // the "first" cause is null
                    break;
                }
                else
                {
                    actions.Insert(0, node.Cause);
                    node = node.Parent;
                }
            }

            return new Path(actions.ToArray(), states.ToArray());
        }

        protected static int IndexOf( IReadOnlyList<Node> list, TState state )
        {
            for( int i = 0; i < list.Count; ++i )
            {
                if( list[i].State.Equals(state) )
                    return i;
            }
            return -1;
        }

        #endregion

        #region Protected Abstract Methods

        protected abstract Node SelectNextNodeToVisit();

        protected abstract void OnNeighborFound( Node node, bool visited );

        #endregion

        #region Public Members

        public Path PathToGoal => GetPath(this.goal);
        public Path[] PathsFound => this.nodesVisited.Select(n => GetPath(n)).ToArray();

        public bool TryIterate( Func<TState, bool> isGoal )
        {
            bool couldIterate;

            // pick node to extend
            var node = this.SelectNextNodeToVisit();
            if( node == null )
            {
                couldIterate = false;
            }
            else
            {
                couldIterate = true;

                // mark as visited
                this.nodesVisited.Add(node);

                // check if goal was reached
                // NOTE: Dijkstra and others depend on this NOT happening immediately when a new neighbor is found,
                //       because the cost of reaching it may change between now and when it is extended.
                if( isGoal(node.State) )
                    this.goal = node;

                // extend node
                var actions = this.problemSpace.Extend(node.State);

                // check results
                if( (actions?.Length ?? 0) != 0 )
                {
                    foreach( var a in actions )
                    {
                        if( !(a is TAction cause) )
                            continue;

                        // get result of action
                        var state = a.Result;
                        if( !(state is TState result) )
                            continue;

                        // make neighbor known to the algorithm
                        this.OnNeighborFound(
                            new Node(node, cause, result),
                            visited: IndexOf(this.nodesVisited, result) != -1);
                    }
                }
            }

            return couldIterate;
        }

        #endregion
    }
}
