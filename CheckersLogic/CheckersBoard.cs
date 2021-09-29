using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    // plansze numerujemy [0,7] x [0,7]
    public class CheckersBoard
    {
        public CheckersPiece[,] boardPieces { get; set; }

        public CheckersPiece GetPieceFromPosition(Position position)
        {
            return boardPieces[position.xPosition, position.yPosition];
        }

        public bool SetPieceOnPosition(Position position, CheckersPiece piece)
        {
            if (!position.IsPositionValid()) return false;

            boardPieces[position.xPosition, position.yPosition] = piece;
            return true;

        }

        public void SetSquareEmpty(Position position)
        {
            boardPieces[position.xPosition, position.yPosition] = null;
        }

        public bool IsSquareWhite(int row, int column) //nie wiem czy ta metoda jest wogóle w przypadku szachów potrzebna ponieważ nie ma żadnej mechaniki uwzględniającej interakcję z kolorem pola bardziej z kolorem bierki na polu(metody do tego napisałem poniżej) ~Bartek
        {
            if ((row + column) % 2 == 0)
            {
                return false;
            }
            return true;
        }

        public bool IsWhitePieceOnSquare(int row, int column)
        {
            if (boardPieces[row, column] == null) return false;
            return boardPieces[row, column].isWhite;
        }
        public bool IsWhitePieceOnSquare(Position position)
        {
            if (boardPieces[position.xPosition, position.yPosition] == null) return false;
            return boardPieces[position.xPosition, position.yPosition].isWhite;
        }
        public bool IsBlackPieceOnSquare(Position position)
        {
            if (boardPieces[position.xPosition, position.yPosition] == null) return false;
            return !boardPieces[position.xPosition, position.yPosition].isWhite;
        }

        public bool IsSquareEmpty(Position position)
        {
            return boardPieces[position.xPosition, position.yPosition] == null;
        }

        public static Position[] ReturnPiecesOfOneColorFromTheBoard(CheckersBoard checkersBoard, bool isWhite)
        {
            List<Position> piecesOfOneColor = new List<Position>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Position currentPosition = new Position(i, j);
                    if (!checkersBoard.IsSquareEmpty(currentPosition) && checkersBoard.IsWhitePieceOnSquare(currentPosition) == isWhite)
                        piecesOfOneColor.Add(currentPosition);
                }
            }
            return piecesOfOneColor.ToArray();
        }
        public void ChangeManToKingOnTheBoard(Position position)
        {
            CheckersPiece checkersPiece = boardPieces[position.xPosition, position.yPosition];
            checkersPiece.figure = CheckersPiece.Figure.King;
        }
        public CheckersBoard()
        {
            boardPieces = new CheckersPiece[8, 8];
            //Ustawienie początkowe białych men
            for (int i = 0; i < 3; i++)
            {
                int j = i % 2;
                int numberOfMenInLine = 0;
                while (numberOfMenInLine < 4)
                {
                    boardPieces[j, i] = new CheckersPiece(CheckersPiece.Figure.Men, true, new Position(j, i));
                    j += 2;
                    numberOfMenInLine++;
                }
            }
            //Ustawienie początkowe czarnych men
            for (int i = 5; i < 8; i++)
            {
                int j = i % 2;
                int numberOfMenInLine = 0;
                while (numberOfMenInLine < 4)
                {
                    boardPieces[j, i] = new CheckersPiece(CheckersPiece.Figure.Men, false, new Position(j, i));
                    j += 2;
                    numberOfMenInLine++;
                }
            }
        }
    }
}