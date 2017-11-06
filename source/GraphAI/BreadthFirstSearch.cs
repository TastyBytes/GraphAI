using System;
using System.Collections.Generic;

namespace GraphAI
{
    public class BreadthFirstSearch : SearchAlgorithmBase<IState, IAction>
    {
        #region Private Fields

        private readonly List<Node> nodesNotVisited;

        #endregion

        #region Constructor

        public BreadthFirstSearch( IProblemSpace problem, IState initialState )
            : base(problem)
        {
            if( initialState == null )
                throw new ArgumentNullException(nameof(initialState));

            this.nodesNotVisited = new List<Node>();
            this.nodesNotVisited.Add(new Node(null, null, initialState));
        }

        #endregion

        #region SearchAlgorithmBase

        protected override Node SelectNextNodeToVisit()
        {
            if( this.nodesNotVisited.Count != 0 )
            {
                var node = this.nodesNotVisited[0];
                this.nodesNotVisited.RemoveAt(0);
                return node;
            }
            else
            {
                return null;
            }
        }

        protected override void OnNeighborFound( Node node, bool visited )
        {
            if( !visited )
                this.nodesNotVisited.Add(node);
        }

        #endregion
    }
}
