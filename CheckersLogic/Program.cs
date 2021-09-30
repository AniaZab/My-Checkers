using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    class Program
    {
        public static void PrintPositions(Position[] positions)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                Console.WriteLine(positions[i]);
            }
        }
        public static void PrintPiecesOfTwoColorsFromTheBoard(CheckersBoard b1)
        {
            Position[] positionsOfWhitePices = CheckersBoard.ReturnPiecesOfOneColorFromTheBoard(b1, true);
            Position[] positionsOfBlackPices = CheckersBoard.ReturnPiecesOfOneColorFromTheBoard(b1, false);
            Console.WriteLine("\n\n\n\n\n\npositionsOfWhitePices are:\n");
            PrintPositions(positionsOfWhitePices);
            Console.WriteLine("\n\n\n\n\n\npositionsOfBlackPices are:\n");
            PrintPositions(positionsOfBlackPices);
        }
        public static void PrintPossiblePositions(Position[] possiblePositions)
        {
            foreach (var item in possiblePositions)
            {
                Console.WriteLine("nowa możliwa pozycja tej figury: "+item);
            }
        }
            

        public static void TestingMovePiece(CheckersBoard b1)
        {
            //MovePiece dla MEN
            Console.WriteLine(MovePiece.Move(b1, new Position(6, 4), new Position(7, 5)));
            Position endPosition = new Position(4, 2);
            Console.WriteLine(MovePiece.Move(b1, new Position(6, 4), endPosition));
            PrintPiecesOfTwoColorsFromTheBoard(b1);

            //MovePiece dla KING
            Position startPosition = new Position(5, 5);
            endPosition = new Position(7, 3);
            b1.ChangeManToKingOnTheBoard(startPosition);
            b1.SetPieceOnPosition(new Position(6, 4), b1.GetPieceFromPosition(new Position(6, 2)));
            b1.SetSquareEmpty(new Position(6, 2));
            Console.WriteLine(MovePiece.Move(b1, startPosition, endPosition));
            PrintPiecesOfTwoColorsFromTheBoard(b1);
        }
        public static void TestingMethodFindPossibleMoves(CheckersBoard b1)
        {
            Console.WriteLine("Men test- starting checkersBoard with extra white Men on [4,2] with NORMAL move:");
            Position[] menMoves = PossibleMoves.FindPossibleMoves(b1, new Position(4, 2), true, false, false);
            PrintPossiblePositions(menMoves);

            Console.WriteLine("\nMen test- starting checkersBoard with extra white Men on [4,2]  with CAPTURE move:");            
            b1.boardPieces[5, 3] = new CheckersPiece(CheckersPiece.Figure.Men, false, new Position(5, 3));

            menMoves = PossibleMoves.FindPossibleMoves(b1, new Position(4, 2), true, true, false);
            PrintPossiblePositions(menMoves);

            Console.WriteLine("\nMen test- starting checkersBoard with extra white Men on [5,3]  with NO move as 2 are blocking:");
            menMoves = PossibleMoves.FindPossibleMoves(b1, new Position(5, 3), false, false, true);
            if (menMoves.Length == 0)
                Console.WriteLine("nie ma możliwości ruchu");
            else
                PrintPossiblePositions(menMoves);

            Console.WriteLine("\nMen test- starting checkersBoard with extra white Men on [6,4]  AFTER CAPTURE move:");
            b1.SetSquareEmpty(new Position(4, 2)); // biały sie przeniósł na inne pole
            b1.boardPieces[6, 4] = new CheckersPiece(CheckersPiece.Figure.Men, true, new Position(6, 4)); // nowe miejsce białego
            b1.SetSquareEmpty(new Position(4, 6)); // usuwam by sprawdzić
            menMoves = PossibleMoves.FindPossibleMoves(b1, new Position(6, 4), true, true, true);
            PrintPossiblePositions(menMoves);
        }
        static void PrintBoard(CheckersBoard b1)
        {
            Console.WriteLine("\n\n\n\n\n\n");
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.WriteLine(b1.boardPieces[i, j]);
                }
            }
        }
        static void Main(string[] args)
        {
            CheckersBoard b1 = new CheckersBoard();
            TestingMethodFindPossibleMoves(b1);
            b1.boardPieces[5, 3] = new CheckersPiece(CheckersPiece.Figure.Men, false, new Position(5, 3));
            b1.SetSquareEmpty(new Position(4, 2)); // biały sie przeniósł na inne pole
            b1.boardPieces[6, 4] = new CheckersPiece(CheckersPiece.Figure.Men, true, new Position(6, 4)); // nowe miejsce białego
            b1.SetSquareEmpty(new Position(4, 6)); // usuwam by sprawdzić
            

            PrintBoard(b1);
            PrintPiecesOfTwoColorsFromTheBoard(b1);
            // sprawdzanie czy są mozliwe bicia dla jakielwolwiek figury danego koloru
            Position[] positionsOfWhitePices = CheckersBoard.ReturnPiecesOfOneColorFromTheBoard(b1, true);
            Position[] positionsOfBlackPices = CheckersBoard.ReturnPiecesOfOneColorFromTheBoard(b1, false);

            Console.WriteLine("Checking if there is possibile capture for white pieces: " + PossibleMoves.IsCaptureExistingForAnyPiece(b1,positionsOfWhitePices));
            Console.WriteLine("Checking if there is possibile capture for black pieces: " + PossibleMoves.IsCaptureExistingForAnyPiece(b1, positionsOfBlackPices));
            //TESTOWANIE POSSIBLEMOVES
                //TestingPossibleMoves(b1, positionsOfWhitePices, positionsOfBlackPices);
            //TESTOWANIE MOVEPIECE
            TestingMovePiece(b1);
            Console.ReadKey();
        }
    }
}
