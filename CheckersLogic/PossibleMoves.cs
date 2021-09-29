using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    public class PossibleMoves
    {
        private static bool IsCaptureExisting = false; // czy istnieje bicie dla danego koloru
        private static bool IsFirstMove; // pierwszy ruch tym pionkiem w tym ruchu, może powinno to być dla każdej figury?
        static string[] DirectionsString = { "North-West", "North-East", "South-East", "South-West" };

        public static Position[] FindPossibleMoves(CheckersBoard checkersBoard, Position position, bool isFirstMove, 
            bool isCaptureExisting, bool isAfterCapture)
        {
            if (checkersBoard.IsSquareEmpty(position))
            {
                return new Position[0];
            }
            IsFirstMove = isFirstMove;
            IsCaptureExisting = isCaptureExisting;
            if (!isAfterCapture && !isFirstMove) //jesli to nie jest 1. ruch i ten ruch nie był biciem
            {
                return new Position[0];
            }
            if (isAfterCapture)
            {
                if (IsCaptureExistingForAnyPiece(checkersBoard, new Position[1] { position }))
                {
                    IsCaptureExisting = true;
                }
                else
                    return new Position[0];
            }
            CheckersPiece.Figure pieceType = checkersBoard.GetPieceFromPosition(position).figure;
            
            switch (pieceType)
            {
                case CheckersPiece.Figure.Men:
                    return PossibleMovesMan(checkersBoard, position);

                case CheckersPiece.Figure.King:
                    return PossibleMovesKing(checkersBoard, position);

                default:
                    throw new Exception("something went wrong and PossibleMoves.Positions() propably received invalid arguments?");
            }
        }
        public static bool IsCaptureExistingForAnyPiece(CheckersBoard checkersBoard, Position[] position)
        {
            for (int i = 0; i < position.Length; i++)
            {
                CheckersPiece.Figure pieceType = checkersBoard.GetPieceFromPosition(position[i]).figure;
                var directions = new Dictionary<string, Position>()
                {
                    ["North-West"] = new Position(position[i].xPosition - 1, position[i].yPosition + 1),
                    ["North-East"] = new Position(position[i].xPosition + 1, position[i].yPosition + 1),
                    ["South-West"] = new Position(position[i].xPosition - 1, position[i].yPosition - 1),
                    ["South-East"] = new Position(position[i].xPosition + 1, position[i].yPosition - 1),
                };
                bool isCaptureAvailable = false;
                if (CheckersPiece.Figure.Men == pieceType)
                {
                    isCaptureAvailable = IsCaptureAvailableForMen(checkersBoard, position[i], directions);
                }
                else if (CheckersPiece.Figure.King == pieceType)
                {
                    isCaptureAvailable = IsCaptureAvailableForKing(checkersBoard, position[i]);
                }
                if (isCaptureAvailable)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsCaptureExistingAfterKingCapture(CheckersBoard checkersBoard, Position positionBeforeCapture, Position[] positionsAfterCapture, string forbiddenDirection)
        {
            for (int i = 0; i < positionsAfterCapture.Length; i++)
            {
                bool isCaptureAvailable = IsCaptureAvailableForKingAfterKingCapture(checkersBoard, positionBeforeCapture, positionsAfterCapture[i], forbiddenDirection);
                if (isCaptureAvailable)
                {
                    return true;
                }
            }
            return false;
        }
        public static Position[] ReturnPiecesWhoCanCapture(CheckersBoard checkersBoard, Position[] position)
        {
            List<Position> piecesWhoCanCapture = new List<Position>();
            for (int i = 0; i < position.Length; i++)
            {
                CheckersPiece.Figure pieceType = checkersBoard.GetPieceFromPosition(position[i]).figure;
                var directions = new Dictionary<string, Position>()
                {
                    ["North-West"] = new Position(position[i].xPosition - 1, position[i].yPosition + 1),
                    ["North-East"] = new Position(position[i].xPosition + 1, position[i].yPosition + 1),
                    ["South-West"] = new Position(position[i].xPosition - 1, position[i].yPosition - 1),
                    ["South-East"] = new Position(position[i].xPosition + 1, position[i].yPosition - 1),
                };
                bool isCaptureAvailable = false;
                if (CheckersPiece.Figure.Men == pieceType)
                {
                    isCaptureAvailable = IsCaptureAvailableForMen(checkersBoard, position[i], directions);
                }
                else if(CheckersPiece.Figure.King == pieceType)
                {
                    isCaptureAvailable = IsCaptureAvailableForKing(checkersBoard, position[i]);
                }
                if (isCaptureAvailable)
                {
                    piecesWhoCanCapture.Add(position[i]);
                }
            }
            return piecesWhoCanCapture.ToArray();
        }
        public static List<Position> ReturnPiecesWhoCanCaptureAfterKingCapture(CheckersBoard checkersBoard, Position positionBeforeCapture,
            Position[] positionsAfterCapture, string forbiddenDirection)
        {
            List<Position> piecesWhoCanCapture = new List<Position>();
            for (int i = 0; i < positionsAfterCapture.Length; i++)
            {
                bool isCaptureAvailable = IsCaptureAvailableForKingAfterKingCapture(checkersBoard, positionBeforeCapture,
                    positionsAfterCapture[i], forbiddenDirection);

                if (isCaptureAvailable)
                {
                    piecesWhoCanCapture.Add(positionsAfterCapture[i]);
                }
            }
            return piecesWhoCanCapture;
        }
        private static Position[] PossibleMovesMan(CheckersBoard checkersBoard, Position position)
        {
            var directions = new Dictionary<string, Position>()
            {
                ["North-West"] = new Position(position.xPosition - 1, position.yPosition + 1),
                ["North-East"] = new Position(position.xPosition + 1, position.yPosition + 1),
                ["South-West"] = new Position(position.xPosition - 1, position.yPosition - 1),
                ["South-East"] = new Position(position.xPosition + 1, position.yPosition - 1),
            };
            if (IsCaptureExisting)
            {
                return ExploreManCaptures(checkersBoard, position, directions);
            }
            // można wykonać zwykły ruch jeśli przed chwilą nie było nim bicia
            else if (!IsCaptureExisting && IsFirstMove)
            {
                var northWestMoves = ExploreManMoves(checkersBoard, position, "North-West", directions);
                var northEastMoves = ExploreManMoves(checkersBoard, position, "North-East", directions);
                var southWestMoves = ExploreManMoves(checkersBoard, position, "South-West", directions);
                var southEastMoves = ExploreManMoves(checkersBoard, position, "South-East", directions);
                return northWestMoves.Concat(northEastMoves).Concat(southWestMoves).Concat(southEastMoves).ToArray();
            }
            // gdy był już ruch i nie można nim bić
            return new Position[0];
        }
        private static Position[] PossibleMovesKing(CheckersBoard checkersBoard, Position position)
        {
            if (IsCaptureExisting)
            {
                return ExploreKingCaptures(checkersBoard, position).ToArray();
            }
            else if (!IsCaptureExisting && IsFirstMove)// zwykłe ruchy dla królowej
            {
                return ExploreKingMoves(checkersBoard, position).ToArray();
            }
            // gdy był już ruch i nie można nim bić
            return new Position[0];
        }
        private static bool IsCaptureAvailableForMen(CheckersBoard checkersBoard, Position position,
            Dictionary<string, Position> directions)
        {
            Position[] currentPosition = { directions["North-West"], directions["North-East"], 
                directions["South-West"], directions["South-East"] };
            for (int i = 0; i < currentPosition.Length; i++)
            {
                if (currentPosition[i].IsPositionValid() && !checkersBoard.IsSquareEmpty(currentPosition[i])&&
                    (checkersBoard.IsWhitePieceOnSquare(position) != checkersBoard.IsWhitePieceOnSquare(currentPosition[i])))
                {
                    int xChange = currentPosition[i].xPosition - position.xPosition;
                    int yChange = currentPosition[i].yPosition - position.yPosition;
                    Position afterSmashingPosition = new Position(currentPosition[i].xPosition + xChange, 
                        currentPosition[i].yPosition + yChange);
                    if (afterSmashingPosition.IsPositionValid() && 
                        checkersBoard.IsSquareEmpty(afterSmashingPosition))
                        return true;
                }
            }
            return false;
        }
        private static bool IsCaptureAvailableForKing(CheckersBoard checkersBoard, Position position)
        {
            for (int i = 0; i < DirectionsString.Length; i++)
            {
                int counter = 1;
                while (true)
                {
                    var directions = new Dictionary<string, Position>()
                    {
                        ["North-West"] = new Position(position.xPosition - counter, position.yPosition + counter),
                        ["North-East"] = new Position(position.xPosition + counter, position.yPosition + counter),
                        ["South-East"] = new Position(position.xPosition + counter, position.yPosition - counter),
                        ["South-West"] = new Position(position.xPosition - counter, position.yPosition - counter),
                    };
                    Position currentPosition = directions[DirectionsString[i]];
                    counter += 1;
                    if (currentPosition.IsPositionValid() && !checkersBoard.IsSquareEmpty(currentPosition) &&
                        (checkersBoard.IsWhitePieceOnSquare(position) != checkersBoard.IsWhitePieceOnSquare(currentPosition)))
                    {
                        int xChange = currentPosition.xPosition - position.xPosition;
                        int yChange = currentPosition.yPosition - position.yPosition;
                        Position afterSmashingPosition = new Position(currentPosition.xPosition + xChange,
                            currentPosition.yPosition + yChange);
                        if (afterSmashingPosition.IsPositionValid() &&
                            checkersBoard.IsSquareEmpty(afterSmashingPosition))
                            return true;
                    }
                    if (!currentPosition.IsPositionValid() ||
                        checkersBoard.IsWhitePieceOnSquare(position) == checkersBoard.IsWhitePieceOnSquare(currentPosition))
                        break;
                }
            }
            return false;
        }
        private static bool IsCaptureAvailableForKingAfterKingCapture(CheckersBoard checkersBoard, Position positionBeforeCapture, Position positionAfterCapture, string forbiddenDirection)
        {
            for (int i = 0; i < DirectionsString.Length; i++)
            {
                if(forbiddenDirection == DirectionsString[i])
                {
                    int counter = 1;
                    while (true)
                    {
                        var directions = new Dictionary<string, Position>()
                        {
                            ["North-West"] = new Position(positionAfterCapture.xPosition - counter, positionAfterCapture.yPosition + counter),
                            ["North-East"] = new Position(positionAfterCapture.xPosition + counter, positionAfterCapture.yPosition + counter),
                            ["South-East"] = new Position(positionAfterCapture.xPosition + counter, positionAfterCapture.yPosition - counter),
                            ["South-West"] = new Position(positionAfterCapture.xPosition - counter, positionAfterCapture.yPosition - counter),
                        };
                        Position currentPosition = directions[DirectionsString[i]];
                        counter += 1;
                        if (currentPosition.IsPositionValid() && !checkersBoard.IsSquareEmpty(currentPosition) &&
                            (checkersBoard.IsWhitePieceOnSquare(positionBeforeCapture) != checkersBoard.IsWhitePieceOnSquare(currentPosition)))
                        {
                            Position afterSmashingPosition = new Position(2*currentPosition.xPosition - positionAfterCapture.xPosition,
                                2*currentPosition.yPosition - positionAfterCapture.yPosition);
                            if (afterSmashingPosition.IsPositionValid() &&
                                checkersBoard.IsSquareEmpty(afterSmashingPosition)) return true;
                        }
                        if (!currentPosition.IsPositionValid() ||
                            checkersBoard.IsWhitePieceOnSquare(positionBeforeCapture) == checkersBoard.IsWhitePieceOnSquare(currentPosition))
                            break;
                    }
                }
            }
            return false;
        }
        private static Position[] ExploreManCaptures(CheckersBoard checkersBoard, Position position, 
            Dictionary<string, Position> directions)
        {
            List<Position> captureMoves = new List<Position>();

            Position[] currentPosition = { directions["North-West"], directions["North-East"],
                directions["South-West"], directions["South-East"] };
            for (int i = 0; i < currentPosition.Length; i++)
            {
                if (currentPosition[i].IsPositionValid() && !checkersBoard.IsSquareEmpty(currentPosition[i]) &&
                    (checkersBoard.IsWhitePieceOnSquare(position) != checkersBoard.IsWhitePieceOnSquare(currentPosition[i])))
                {
                    int xChange = currentPosition[i].xPosition - position.xPosition;
                    int yChange = currentPosition[i].yPosition - position.yPosition;
                    Position afterSmashingPosition = new Position(currentPosition[i].xPosition + xChange, currentPosition[i].yPosition + yChange);
                    if (afterSmashingPosition.IsPositionValid() && 
                        checkersBoard.IsSquareEmpty(afterSmashingPosition))
                        captureMoves.Add(afterSmashingPosition);
                }
            }
            Position[] output = new Position[captureMoves.Count()];
            int indexForOutput = 0;
            foreach (Position p in captureMoves)
            {
                output[indexForOutput] = p;
                indexForOutput++;
            }
            return output;
        }
        private static List<Position> ExploreKingCaptures(CheckersBoard checkersBoard, Position position)
        {
            List<Position> captureMoves = new List<Position>();
            for (int i = 0; i < DirectionsString.Length; i++)
            {
                int counter = 1;
                while (true)
                {
                    var directions = new Dictionary<string, Position>()
                    {
                        ["North-West"] = new Position(position.xPosition - counter, position.yPosition + counter),
                        ["North-East"] = new Position(position.xPosition + counter, position.yPosition + counter),
                        ["South-East"] = new Position(position.xPosition + counter, position.yPosition - counter),
                        ["South-West"] = new Position(position.xPosition - counter, position.yPosition - counter),
                    };
                    Position currentPosition = directions[DirectionsString[i]];
                    counter += 1;
                    if (currentPosition.IsPositionValid() && !checkersBoard.IsSquareEmpty(currentPosition) &&
                        (checkersBoard.IsWhitePieceOnSquare(position) != checkersBoard.IsWhitePieceOnSquare(currentPosition)))
                    {
                        Position afterSmashingPosition = new Position(2*currentPosition.xPosition - position.xPosition,
                            2*currentPosition.yPosition - position.yPosition);
                        if (afterSmashingPosition.IsPositionValid() && checkersBoard.IsSquareEmpty(afterSmashingPosition))
                        {
                            List<Position> possibleCaptureMoves = new List<Position>();
                            possibleCaptureMoves.Add(afterSmashingPosition);
                            possibleCaptureMoves = possibleCaptureMoves.Concat(ExploreKingMovesInTheDirection(checkersBoard, afterSmashingPosition, DirectionsString[i])).ToList(); // dodaje wszystkie mozliwe miejsca, ktore moze King zajac po zbiciu
                            captureMoves = captureMoves.Concat(ExploreKingMovesAfterKingCapture(checkersBoard, position, possibleCaptureMoves, DirectionsString[i])).ToList();
                            break;
                        }
                    }
                    if (!currentPosition.IsPositionValid() ||
                        checkersBoard.IsWhitePieceOnSquare(position) == checkersBoard.IsWhitePieceOnSquare(currentPosition)) break;
                }
            }
            return captureMoves;
        }
        private static List<Position> ExploreKingMovesAfterKingCapture(CheckersBoard checkersBoard, Position positionBeforeCapture,
            List<Position> possibleCaptureMoves, string captureDirection)
        {
            string forbiddenDirection = ReturnForbiddenDirection(captureDirection);
            Position[] positionsAfterCapture = possibleCaptureMoves.ToArray();
            if (IsCaptureExistingAfterKingCapture(checkersBoard, positionBeforeCapture, positionsAfterCapture, forbiddenDirection))
            {
                return ReturnPiecesWhoCanCaptureAfterKingCapture(checkersBoard, positionBeforeCapture, 
                    positionsAfterCapture, forbiddenDirection);
            }
            return possibleCaptureMoves;
        }
        private static string ReturnForbiddenDirection(string captureDirection)
        {
            string forbiddenDirection = "";
            for (int i = 0; i < DirectionsString.Length; i++)
            {
                if(captureDirection[0]!= DirectionsString[i][0] && captureDirection[6] != DirectionsString[i][6])
                {
                    forbiddenDirection = DirectionsString[i];
                    break;
                }
            }
            return forbiddenDirection;
        }
        private static List<Position> ExploreManMoves(CheckersBoard checkersBoard, Position position, 
            string direction, Dictionary<string, Position> directions)
        {
            List<Position> output = new List<Position>();
            Position currentPosition = directions[direction];
            if (checkersBoard.IsWhitePieceOnSquare(position)) // ruszamy bialym
            {
                if ((direction == "North-East" || direction == "North-West") 
                    && currentPosition.IsPositionValid() && checkersBoard.IsSquareEmpty(currentPosition))
                {
                    output.Add(currentPosition);
                }
            }
            else // ruszamy czarnym
            {
                if ((direction == "South-West" || direction == "South-East") 
                    && currentPosition.IsPositionValid() && checkersBoard.IsSquareEmpty(currentPosition))
                {
                    output.Add(currentPosition);
                }
            }
            return output;
        }
        private static List<Position> ExploreKingMoves(CheckersBoard checkersBoard, Position position)
        {
            List<Position> output = new List<Position>();
            for (int i = 0; i < DirectionsString.Length; i++)
            {
                int counter = 1;
                while (true)
                {
                    var directions = new Dictionary<string, Position>()
                    {
                        ["North-West"] = new Position(position.xPosition - counter, position.yPosition + counter),
                        ["North-East"] = new Position(position.xPosition + counter, position.yPosition + counter),
                        ["South-East"] = new Position(position.xPosition + counter, position.yPosition - counter),
                        ["South-West"] = new Position(position.xPosition - counter, position.yPosition - counter),
                    };
                    Position currentPosition = directions[DirectionsString[i]];
                    if (!currentPosition.IsPositionValid()) break;
                    if (checkersBoard.IsSquareEmpty(currentPosition))
                    {
                        output.Add(currentPosition);
                        counter += 1;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return output;
        }
        private static List<Position> ExploreKingMovesInTheDirection(CheckersBoard checkersBoard, Position position, string direction)
        {
            List<Position> output = new List<Position>();
            int counter = 1;
            while (true)
            {
                var directions = new Dictionary<string, Position>()
                {
                    ["North-West"] = new Position(position.xPosition - counter, position.yPosition + counter),
                    ["North-East"] = new Position(position.xPosition + counter, position.yPosition + counter),
                    ["South-East"] = new Position(position.xPosition + counter, position.yPosition - counter),
                    ["South-West"] = new Position(position.xPosition - counter, position.yPosition - counter),
                };
                Position currentPosition = directions[direction];
                if (!currentPosition.IsPositionValid()) break;
                if (checkersBoard.IsSquareEmpty(currentPosition))
                {
                    output.Add(currentPosition);
                    counter += 1;
                    continue;
                }
                else
                {
                    break;
                }
            }
            return output;
        }
    }
}