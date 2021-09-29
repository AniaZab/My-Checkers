using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersLogic
{
    public class CheckersPiece
    {
		public Figure figure { get; set; }
		public bool isWhite { get; set; }

		public Position position { get; set; }

		public enum Figure
		{
			Men,
			King
		}

		public CheckersPiece(Figure figure, bool isWhite, Position position)
		{
			this.figure = figure;
			this.isWhite = isWhite;
			this.position = position;
		}

		public override string ToString()
		{
			return $"ChessPiece[figure:{{{figure.ToString()}}}, isWhite:{{{isWhite}}}, position:{{{position.ToString()}}}]";
		}

		public bool IsPieceOnBoard()
		{
			return position.IsPositionValid();
		}
	}
}


