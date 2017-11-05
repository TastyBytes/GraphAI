using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphAI
{
    public class BreadthFirstSearch
    {
        #region Node

        private class Node
        {
            internal Node( Node parent, IAction cause, IState state )
            {
                if( state == null )
                    throw new ArgumentNullException(nameof(state));

                this.Parent = parent;
                this.Cause = cause;
                this.State = state;
            }

            internal Node Parent { get; }
            internal IAction Cause { get; }
            internal IState State { get; }
            internal bool IsSeed => this.Cause == null;
        }

        #endregion

        #region Private Fields

        private readonly IProblemSpace problemSpace;
        private readonly List<Node> nodesToExtend;
        private readonly List<Node> nodesVisited;
        private Node goal;

        #endregion

        #region Constructor

        public BreadthFirstSearch( IProblemSpace problem, params IState[] initialStates )
        {
            if( (initialStates?.Length ?? 0) == 0 )
                throw new ArgumentException("There must be at least one initial (i.e. seed) state!");

            if( initialStates.Where(s => s == null).Count() != 0 )
                throw new ArgumentException("null initial state found!");

            this.problemSpace = problem ?? throw new ArgumentNullException(nameof(problem));
            this.nodesToExtend = new List<Node>();
            this.nodesVisited = new List<Node>();

            this.nodesToExtend.AddRange(initialStates.Select(s => new Node(null, null, s)));
        }

        #endregion

        #region Private Static Methods

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

        private static int IndexOf( List<Node> list, IState state )
        {
            for( int i = 0; i < list.Count; ++i )
            {
                if( list[i].State.Equals(state) )
                    return i;
            }
            return -1;
        }

        #endregion

        #region Public Members

        public Path PathToGoal => GetPath(this.goal);
        public Path[] PathsFound => this.nodesVisited.Select(n => GetPath(n)).ToArray();

        public bool TryIterate( Func<IState, bool> isGoal )
        {
            bool couldIterate;

            // pick node to extend
            Node node;
            if( this.nodesToExtend.Count == 0 )
            {
                couldIterate = false;
            }
            else
            {
                couldIterate = true;
                node = this.nodesToExtend[0];
                this.nodesToExtend.RemoveAt(0);
                this.nodesVisited.Add(node);

                // extend node
                var actions = this.problemSpace.Extend(node.State);

                // check results
                if( (actions?.Length ?? 0) != 0 )
                {
                    foreach( var a in actions )
                    {
                        if( a == null )
                            continue;

                        // get result of action
                        var state = a.GetResult();
                        if( state == null )
                            continue;

                        // skip if state already seen
                        // (we always keep the shortest path)
                        if( IndexOf(this.nodesToExtend, state) != -1
                         || IndexOf(this.nodesVisited, state) != -1 )
                            continue;

                        // see what we can reach from here
                        var n = new Node(node, a, state);
                        this.nodesToExtend.Add(n);

                        // is it our goal?
                        if( isGoal(state) )
                            this.goal = n;
                    }
                }
            }

            return couldIterate;
        }

        #endregion
    }
}
