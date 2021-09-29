using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    public class OperationsAfterMove
    {
        public static void RemoveCapturedPiece(CheckersBoard checkersBoard, Position startPosition, Position endPosition)
        {
            Position positionOfCapturedPiece = ReturnsPositionOfCapturedPiece(checkersBoard, startPosition, endPosition);
            if (positionOfCapturedPiece != endPosition)
                checkersBoard.SetSquareEmpty(positionOfCapturedPiece);
        }
        private static Position ReturnsPositionOfCapturedPiece(CheckersBoard checkersBoard, Position startPosition, Position endPosition)
        {
            int differenceX = endPosition.xPosition - startPosition.xPosition;
            int differenceY = endPosition.yPosition - startPosition.yPosition;
            int changeX = differenceX / Math.Abs(differenceX);
            int changeY = differenceY / Math.Abs(differenceY);
            int indexX = startPosition.xPosition + changeX;
            int indexY = startPosition.yPosition + changeY;
            Position output = new Position(indexX, indexY);
            while(checkersBoard.IsSquareEmpty(output) ||  checkersBoard.IsWhitePieceOnSquare(endPosition) == 
                checkersBoard.IsWhitePieceOnSquare(output))
            {
                indexX+=changeX;
                indexY+=changeY;
                output = new Position(indexX, indexY);
            }
            return output;
        }
        public static bool WasManChangedIntoKing(CheckersBoard checkersBoard, Position endposition)
        {
            if (MovePiece.IsItTheEndOfMove(checkersBoard, endposition))
            {
                if (IsManGetEnemysEnd(checkersBoard, endposition))
                {
                    checkersBoard.ChangeManToKingOnTheBoard(endposition);
                    return true;
                }
            }
            return false;
        }
        public static bool IsManGetEnemysEnd(CheckersBoard checkersBoard, Position position)
        {
            if (checkersBoard.GetPieceFromPosition(position).isWhite)
            {
                if(position.yPosition == 7)
                {
                    return true;
                }
            }
            else
            {
                if (position.yPosition == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
