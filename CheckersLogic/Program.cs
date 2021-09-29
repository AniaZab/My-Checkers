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
        public static void TestingPossibleMoves(CheckersBoard b1, Position[] positionsOfWhitePices, Position[] positionsOfBlackPices)
        {
            // wejściowe pozycje, które mogą być rozważane
            Position[] possiblePositions = positionsOfWhitePices;
            // pozycje, z których mozna się ruszyć
            Position[] availablePositionsToMoveFrom;
            Position[] possibleMoves;
            bool isFirstMove = true;
            bool isAfterCapture = false;
            // unity pyta czy mozna zbić jakimś pionkiem danego koloru?
            bool isCaptureExisting = PossibleMoves.IsCaptureExistingForAnyPiece(b1, possiblePositions);
            // w zalezności od odpowiedzi unity robi:
            if (isCaptureExisting)
                availablePositionsToMoveFrom = PossibleMoves.ReturnPiecesWhoCanCapture(b1, possiblePositions);
            else
                availablePositionsToMoveFrom = possiblePositions;
            
            Console.WriteLine("availablePositionsToMoveFrom are:\n");
            PrintPositions(availablePositionsToMoveFrom);
            //unity wywołuje dla wybranej przez uzytkownika pozycji następująca funkcję (UWAGA! pozycja musi być wybrana z availablePositionsToMoveFrom):
            Position positionChosenByUserToMoveFrom = new Position(6, 4);
            Console.WriteLine("user is chosing from availablePositionsToMoveFrom a position to move from: " + positionChosenByUserToMoveFrom);
            possibleMoves = PossibleMoves.FindPossibleMoves(b1, positionChosenByUserToMoveFrom, isFirstMove, isCaptureExisting, isAfterCapture);
            Console.WriteLine("user can choose from possibleMoves a position to move to: ");
            PrintPositions(possibleMoves);
            //tu mały test gdy nie ma figury na tym polu
            Position testPositionChosenByUserToMoveFrom = new Position(4, 2);
            PossibleMoves.FindPossibleMoves(b1, testPositionChosenByUserToMoveFrom, isFirstMove, isCaptureExisting, isAfterCapture);

            // unity zmienia parametry wejściowe:
            isFirstMove = false;
            if (isCaptureExisting)
                isAfterCapture = true;
            isCaptureExisting = false;
            // musi być wybrana z possibleMoves
            Console.WriteLine("user is chosing from possibleMoves a position to move to: ");
            Position positionChosenByUserToMoveTo = possibleMoves[0];
            Console.WriteLine(positionChosenByUserToMoveTo);

            // zostaje wykonany ruch
            CheckersPiece PieceWhichIsMoving = b1.GetPieceFromPosition(positionChosenByUserToMoveFrom);
            b1.SetPieceOnPosition(positionChosenByUserToMoveTo, PieceWhichIsMoving); // nowe miejsce
            b1.SetSquareEmpty(positionChosenByUserToMoveFrom); // biały sie przeniósł na inne pole, więc go stad usuwamy
            b1.SetSquareEmpty(new Position(5, 5)); // TO DO - funkcja do usuwania zbitego Piece

            // unity pyta czy to koniec? i wywołuje funkcje dla nowej wybranej przez użytkownika pozycji: (powinna być tutaj pętla normalnie)
            possibleMoves = PossibleMoves.FindPossibleMoves(b1, positionChosenByUserToMoveTo, isFirstMove, isCaptureExisting, isAfterCapture);
            if (possibleMoves.Length == 0)
            {
                Console.WriteLine("Koniec ruchu");
            }
            else
            {
                // user wybiera z possibleMoves nową pozycję na którą chce dac swoją figurę np.:
                positionChosenByUserToMoveTo = possibleMoves[0];
                Console.WriteLine("Nowa pozycja to: " + positionChosenByUserToMoveTo);
            }
            positionsOfBlackPices = CheckersBoard.ReturnPiecesOfOneColorFromTheBoard(b1, false);
            PrintPiecesOfTwoColorsFromTheBoard(b1);


            //sprawdzenie PossibleMoves dla KING
            isFirstMove = true;
            isAfterCapture = false;
            //ustawienie królowej:
            b1.ChangeManToKingOnTheBoard(new Position(3, 7));
            possiblePositions = positionsOfBlackPices;
            isCaptureExisting = PossibleMoves.IsCaptureExistingForAnyPiece(b1, possiblePositions);
            if (isCaptureExisting)
            {
                //unity wywołuje funkcje:
                availablePositionsToMoveFrom = PossibleMoves.ReturnPiecesWhoCanCapture(b1, possiblePositions);
            }
            else
            {
                availablePositionsToMoveFrom = possiblePositions;
            }
            Console.WriteLine("availablePositionsToMoveFrom are:\n");
            PrintPositions(availablePositionsToMoveFrom); //poprawnie
            //unity wywołuje dla wybranej przez uzytkownika pozycji następująca funkcję (UWAGA! pozycja musi być wybrana z availablePositionsToMoveFrom):
            positionChosenByUserToMoveFrom = new Position(3, 7);
            Console.WriteLine("user is chosing from availablePositionsToMoveFrom a position to move from: " + positionChosenByUserToMoveFrom);
            possibleMoves = PossibleMoves.FindPossibleMoves(b1, positionChosenByUserToMoveFrom, isFirstMove, isCaptureExisting, isAfterCapture);
            Console.WriteLine("user can choose from possibleMoves a position to move to: ");
            PrintPositions(possibleMoves);
            isFirstMove = false;
            if (isCaptureExisting)
                isAfterCapture = true;
            isCaptureExisting = false;
            // musi być wybrana z possibleMoves
            Console.WriteLine("user is chosing from possibleMoves a position to move to: ");
            positionChosenByUserToMoveTo = possibleMoves[1];
            Console.WriteLine(positionChosenByUserToMoveTo);

            // zostaje wykonany ruch
            PieceWhichIsMoving = b1.GetPieceFromPosition(positionChosenByUserToMoveFrom);
            b1.SetPieceOnPosition(positionChosenByUserToMoveTo, PieceWhichIsMoving); // nowe miejsce
            b1.SetSquareEmpty(positionChosenByUserToMoveFrom); // biały sie przeniósł na inne pole, więc go stad usuwamy
            b1.SetSquareEmpty(new Position(4, 6)); // TO DO - funkcja do usuwania zbitego Piece

            // unity pyta czy to koniec? i wywołuje funkcje dla nowej wybranej przez użytkownika pozycji: (powinna być tutaj pętla normalnie)
            possibleMoves = PossibleMoves.FindPossibleMoves(b1, positionChosenByUserToMoveTo, isFirstMove, isCaptureExisting, isAfterCapture);
            if (possibleMoves.Length == 0)
            {
                Console.WriteLine("Koniec ruchu");
            }
            else
            {
                // user wybiera z possibleMoves nową pozycję na którą chce dac swoją figurę np.:
                positionChosenByUserToMoveTo = possibleMoves[0];
                Console.WriteLine("Nowa pozycja to: " + positionChosenByUserToMoveTo);
            }
            PrintPiecesOfTwoColorsFromTheBoard(b1);
        }
        public static void TestingMovePiece(CheckersBoard b1)
        {
            //MovePiece dla MEN
            Console.WriteLine(MovePiece.Move(b1, new Position(6, 4), new Position(7, 5)));
            Position endPosition = new Position(4, 2);
            Console.WriteLine(MovePiece.Move(b1, new Position(6, 4), endPosition));
            while (!MovePiece.IsItTheEndOfMove(b1, endPosition))
            {
                // daję unity endPosition jako startPosition
                // unity podaje nowe endPosition
            }
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
        static void Main(string[] args)
        {
            CheckersBoard b1 = new CheckersBoard();
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        Console.WriteLine(b1.boardPieces[i, j]);
            //    }
            //}
            Console.WriteLine("Men test- starting checkersBoard with extra white Men on [4,2] with NORMAL move:");
            Position[] menMoves = PossibleMoves.FindPossibleMoves(b1, new Position(4, 2), true, false, false);
            foreach (var item in menMoves)
            {
                Console.WriteLine("nowa możliwa pozycja tej figury: "+item);
            }

            Console.WriteLine("\nMen test- starting checkersBoard with extra white Men on [4,2]  with CAPTURE move:");            
            b1.boardPieces[5, 3] = new CheckersPiece(CheckersPiece.Figure.Men, false, new Position(5, 3));

            menMoves = PossibleMoves.FindPossibleMoves(b1, new Position(4, 2), true, true, false);
            foreach (var item in menMoves)
            {
                Console.WriteLine("nowa możliwa pozycja tej figury: " + item);
            }

            Console.WriteLine("\nMen test- starting checkersBoard with extra white Men on [5,3]  with NO move as 2 are blocking:");
            menMoves = PossibleMoves.FindPossibleMoves(b1, new Position(5, 3), false, false, true);
            if (menMoves.Length == 0)
                Console.WriteLine("nie ma możliwości ruchu");
            else
            {
                foreach (var item in menMoves)
                    Console.WriteLine("nowa możliwa pozycja tej figury: " + item);
            }

            Console.WriteLine("\nMen test- starting checkersBoard with extra white Men on [6,4]  AFTER CAPTURE move:");

            b1.SetSquareEmpty(new Position(4, 2)); // biały sie przeniósł na inne pole
            b1.boardPieces[6, 4] = new CheckersPiece(CheckersPiece.Figure.Men, true, new Position(6, 4)); // nowe miejsce białego
            b1.SetSquareEmpty(new Position(4, 6)); // usuwam by sprawdzić
            menMoves = PossibleMoves.FindPossibleMoves(b1, new Position(6, 4), true, true, true);
            foreach (var item in menMoves)
            {
                Console.WriteLine("nowa możliwa pozycja tej figury: " + item);
            }

            //string[] DirectionsString = { "North-West", "North-East", "South-East", "South-West" };
            //Console.WriteLine(PossibleMoves.ReturnForbiddenDirection("South-West", DirectionsString));

            Console.WriteLine("\n\n\n\n\n\n");
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.WriteLine(b1.boardPieces[i, j]);
                }
            }
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
