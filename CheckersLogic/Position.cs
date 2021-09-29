using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    public class Position
    {
        public int xPosition { get; set; }
        public int yPosition { get; set; }

        public Position(int x, int y)
        {
            xPosition = x;
            yPosition = y;
        }

        //Determines whether two object instances are equal.
        public override bool Equals(object obj)
        {
            Position other = (Position)obj;

            return xPosition == other.xPosition && yPosition == other.yPosition;
        }

        // Method provides this hash code for algorithms that need quick checks of object equality
        public override int GetHashCode()
        {
            return xPosition * 8 + yPosition;
        }

        public override string ToString()
        {
            return $"xPosition:{{{xPosition}}}, yPosition:{{{yPosition}}}";
        }


        public bool IsPositionValid() //returns true if [0,0] <= [x,y] <= [7,7] (given position is on board)
        {
            return (xPosition <= 7 && yPosition <= 7) && (xPosition >= 0 && yPosition >= 0);
        }

    }

}