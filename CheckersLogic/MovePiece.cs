using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    public class MovePiece
    {
        static List<Position> ListOfPositionsWhichCanMoveNextTime = new List<Position>();
        public static bool IsAfterCapture(Position startposition)
        {
            if (ListOfPositionsWhichCanMoveNextTime.Contains(startposition))
                return true;
            return false;
        }
        public static bool IsFirstMove(Position startposition)
        {
            if (ListOfPositionsWhichCanMoveNextTime.Contains(startposition))
                return false;
            return true;
        }
        public static void ChangeListOfPositionsWhichCanMoveNextTime(CheckersBoard checkersBoard, Position endposition)
        {
            if (!IsItTheEndOfMove(checkersBoard, endposition) && 
                !ListOfPositionsWhichCanMoveNextTime.Contains(endposition))
            {
                ListOfPositionsWhichCanMoveNextTime.Add(endposition);
            }
            else
            {
                if (ListOfPositionsWhichCanMoveNextTime.Contains(endposition))
                {
                    ListOfPositionsWhichCanMoveNextTime.Remove(endposition);
                }
            }
        }
        public static bool Move(CheckersBoard checkersBoard, Position startposition,
            Position endposition)
        {
            if(ListOfPositionsWhichCanMoveNextTime.Count > 0)
                startposition = ListOfPositionsWhichCanMoveNextTime.ToArray()[0];
            Position[] possibleStartingPositions = CheckersBoard.ReturnPiecesOfOneColorFromTheBoard
                (checkersBoard, checkersBoard.GetPieceFromPosition(startposition).isWhite);
            Position[] availablePositionsToMoveFrom;
            Position[] possibleMoves;
            bool isCaptureExisting = PossibleMoves.IsCaptureExistingForAnyPiece(
                checkersBoard, possibleStartingPositions);
            if (isCaptureExisting) 
                availablePositionsToMoveFrom = PossibleMoves.ReturnPiecesWhoCanCapture
                    (checkersBoard, possibleStartingPositions);
            else
                availablePositionsToMoveFrom = possibleStartingPositions;
            if (!availablePositionsToMoveFrom.Contains(startposition)) 
                return false;
            possibleMoves = PossibleMoves.FindPossibleMoves(checkersBoard, 
                startposition, IsFirstMove(startposition), isCaptureExisting, IsAfterCapture(startposition));
            if (!possibleMoves.Contains(endposition)) return false; 
            CheckersPiece PieceWhichIsMoving = checkersBoard.GetPieceFromPosition(startposition);
            checkersBoard.SetPieceOnPosition(endposition, PieceWhichIsMoving);
            checkersBoard.SetSquareEmpty(startposition);
            if (isCaptureExisting) OperationsAfterMove.RemoveCapturedPiece(checkersBoard, startposition, endposition);
            OperationsAfterMove.WasManChangedIntoKing(checkersBoard, endposition);
            ChangeListOfPositionsWhichCanMoveNextTime(checkersBoard, endposition);
            return true;
        }
        public static bool IsItTheEndOfMove(CheckersBoard checkersBoard, Position endposition)
        {
            Position[] positionsOfTheColor = CheckersBoard.ReturnPiecesOfOneColorFromTheBoard(checkersBoard,
                checkersBoard.GetPieceFromPosition(endposition).isWhite);
            Position[] positionsWhichCanCapture = PossibleMoves.ReturnPiecesWhoCanCapture(
                checkersBoard, positionsOfTheColor);
            for (int i = 0; i < positionsWhichCanCapture.Length; i++)
            {
                if(positionsWhichCanCapture[i] == endposition)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
