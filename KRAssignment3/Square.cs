/*
 * Project:		KRAssignment3
 * Purpose:		Model a square on the nPuzzle game board
 *
 * History:
 *		Kendall Roth	2015-10-23:		Created
 *										determineValidMoves() added to determine valid moves for selected square
 *										moveSquare() added to move the selected square
 *										Directions enumeration added
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace KRAssignment3
{
	/// <summary>
	/// Enumeration to keep track of move directions
	/// </summary>
	public enum Direction
	{
		Up, Right, Down, Left, None
	}


	/// <summary>
	/// Class that models a Square in the nPuzzle slider game
	/// </summary>
	class Square : Button
	{
		public static int BoardLengthX;
		public static int BoardLengthY;

		public static Square[,] SquaresArray;

		public int SquareX;
		public int SquareY;
		public int SquareValue;

		public const int SQUARE_SIZE = 75;

		public static int OpenX;
		public static int OpenY;

		/// <summary>
		/// Tracks the number of moves in the current game
		/// </summary>
		public static int MoveCounter;


		/// <summary>
		/// Creates a new Square for the board
		/// </summary>
		/// <param name="squareX">Column value of the square in relation to the board</param>
		/// <param name="squareY">Row value of the square in relation to the board</param>
		/// <param name="squareValue">Value of the square</param>
		public Square(int squareX, int squareY, int squareValue)
		{
			this.SquareX = squareX;
			this.SquareY = squareY;
			this.SquareValue = squareValue;

			this.Width = SQUARE_SIZE;
			this.Height = SQUARE_SIZE;
			this.Text = squareValue.ToString();
			this.Font = new Font("Calibri", 20, FontStyle.Bold);

			//Place the new Square into the Squares array
			SquaresArray[squareX, squareY] = this;
		}


		/// <summary>
		/// Returns the direction of the valid move for the selected Square
		/// </summary>
		/// <param name="clickedSquare">Square to check for valid moves</param>
		/// <returns></returns>
		public static Direction determineValidMove(Square clickedSquare)
		{
			Direction validDirection;

			//Check for valid moves
			if (clickedSquare.SquareX == OpenX && clickedSquare.SquareY == OpenY + 1)
			{
				//Square can move Up
				validDirection = Direction.Up;
			}
			else if (clickedSquare.SquareX == OpenX - 1 && clickedSquare.SquareY == OpenY)
			{
				//Square can move Right
				validDirection = Direction.Right;
			}
			else if (clickedSquare.SquareX == OpenX && clickedSquare.SquareY == OpenY - 1)
			{
				//Square can move Down
				validDirection = Direction.Down;
			}
			else if (clickedSquare.SquareX == OpenX + 1 && clickedSquare.SquareY == OpenY)
			{
				//Square can move Left
				validDirection = Direction.Left;
			}
			else
			{
				//No valid move
				validDirection = Direction.None;
			}

			//Return the valid move direction
			return validDirection;
		}


		/// <summary>
		/// Moves the clicked Square into the open space
		/// </summary>
		/// <param name="clickedSquare"></param>
		public static void moveSquare(Square clickedSquare)
		{
			//Temporarily store the clicked Square's coordinates
			int clickedX = clickedSquare.SquareX;
			int clickedY = clickedSquare.SquareY;

			//Update the array of Squares
			SquaresArray[clickedX, clickedY] = null;
			SquaresArray[OpenX, OpenY] = clickedSquare;

			//Move the clicked Square to the open space
			clickedSquare.SquareX = OpenX;
			clickedSquare.SquareY = OpenY;

			//Update the position of the open space
			OpenX = clickedX;
			OpenY = clickedY;
		}
	}
}
