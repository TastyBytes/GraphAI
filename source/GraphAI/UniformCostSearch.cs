using System;
using System.Collections.Generic;

namespace GraphAI
{
    public class UniformCostSearch : SearchAlgorithmBase<IState, IWeightedAction>
    {
        #region Private Fields

        private readonly List<Node> nodesNotVisited;
        private readonly List<int> currentCostToReach;

        #endregion

        #region Constructor

        public UniformCostSearch( IProblemSpace problem, IState initialState )
            : base(problem)
        {
            if( initialState == null )
                throw new ArgumentNullException(nameof(initialState));

            this.nodesNotVisited = new List<Node>();
            this.currentCostToReach = new List<int>();
            this.nodesNotVisited.Add(new Node(null, null, initialState));
            this.currentCostToReach.Add(this.GetCost(this.nodesNotVisited[0]));
        }

        #endregion

        #region Private Methods

        private int GetCost( Node node )
        {
            int sum = 0;
            while( node.Cause != null )
            {
                sum += node.Cause.Cost;
                node = node.Parent;
            }
            return sum;
        }

        #endregion

        #region SearchAlgorithmBase

        protected override Node SelectNextNodeToVisit()
        {
            if( this.nodesNotVisited.Count != 0 )
            {
                // find node with lowest cost
                int minCostIndex = 0;
                for( int i = 1; i < this.currentCostToReach.Count; ++i )
                {
                    if( this.currentCostToReach[i] < this.currentCostToReach[minCostIndex] )
                        minCostIndex = i;
                }

                // remove & return node
                var node = this.nodesNotVisited[minCostIndex];
                this.nodesNotVisited.RemoveAt(minCostIndex);
                this.currentCostToReach.RemoveAt(minCostIndex);
                return node;
            }
            else
            {
                return null;
            }
        }

        protected override void OnNeighborFound( Node node, bool visited )
        {
            int index = IndexOf(this.nodesNotVisited, node.State);
            bool waitingToBeVisited = index != -1;

            bool nodeIsKnown = visited || waitingToBeVisited;
            if( !nodeIsKnown )
            {
                this.nodesNotVisited.Add(node);
                this.currentCostToReach.Add(this.GetCost(node));
            }
            else if( waitingToBeVisited )
            {
                int currentPathCost = this.currentCostToReach[index];
                int newPathCost = GetCost(node);
                if( currentPathCost > newPathCost )
                {
                    this.nodesNotVisited[index] = node;
                    this.currentCostToReach[index] = newPathCost;
                }
            }
        }

        #endregion
    }
}
