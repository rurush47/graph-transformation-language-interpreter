using System;
using System.Linq;
using Microsoft.Msagl.Drawing;

namespace GraphTransformationLanguage
{
    [Serializable]
    public class Rule
    {
        public string Name { get; private set; }
        public Graph RightSide { get; private set; }
        public Graph LeftSide { get; private set; }

        public Rule()
        {
            
        }

        public void SetRule(string name, Graph leftSide, Graph rightSide)
        {
            Name = name;
            LeftSide = leftSide;
            RightSide = rightSide;
        }

        public void IsValid()
        {
            try
            {
                IsGraphValid(LeftSide);
            }
            catch (Exception e)
            {
                throw new Exception("Left side exception: " + e);
            }
            try
            {
                IsGraphValid(RightSide);
            }
            catch (Exception e)
            {
                throw new Exception("Right side exception: " + e);
            }
        }

        private void IsGraphValid(Graph graph)
        {
            int nodesCount = graph.NodeCount;

            if (!graph.Nodes.Any())
            {
                throw new Exception("No nodes in this side of the rule.");
            }

            if (graph.Edges.Any(e => e.Target == e.Source))
            {
                throw new Exception("Edge connecting node to self!");
            }

            foreach (Node n in graph.Nodes)
            {
                if (nodesCount > 1 &&
                    !graph.Edges.Any(e => e.Source == n.Id || e.Target == n.Id))
                {
                    throw new Exception("Node is not connected with the graph!");
                }

                foreach (Node n2 in graph.Nodes)
                {
                    if (n.Id != n2.Id)
                    {
                        if (graph.Edges.Count(e =>
                        (e.Source == n.Id && e.Target == n2.Id) ||
                        (e.Source == n2.Id && e.Target == n.Id)) > 1)
                        {
                            throw new Exception("There is two ways connection between nodes!");
                        }
                    }
                }
            }

            return;
        }
    }
}
