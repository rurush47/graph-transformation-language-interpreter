using Microsoft.Msagl.Drawing;

namespace GraphTransformationLanguage
{
    public class Rule
    {
        public string Name { get; private set; }
        public Graph RightSide { get; private set; }
        public Graph LeftSide { get; private set; }
        
        public void SetRule(string name, Graph leftSide, Graph rightSide)
        {
            Name = name;
            LeftSide = leftSide;
            RightSide = rightSide;
        }
    }
}
