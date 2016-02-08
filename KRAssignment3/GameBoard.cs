/*
 * Project:		KRAssignment3
 * Purpose:		Model a square on the nPuzzle game board
 *
 * History:
 *		Kendall Roth	2015-10-23:		Created
 *										Form added
 *										Board generation added
 *										Game loading added
 *										Game saving added
 *										Win checking added
 *						2015-10-24:		Randomization logic added
 *										New Game button added
 *										Save tracking logic added
 */
 
 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KRAssignment3
{
	/// <summary>
	/// Class that models a Game Board for an nPuzzle game
	/// </summary>
	public partial class GameBoard : Form
	{
		public NumericUpDown numBoardWidth;
		public NumericUpDown numBoardHeight;
		public Button btnGenerateBoard;
		public ProgressBar proGenerateProgress;
		public Button btnLoadGame;
		public Button btnSaveGame;
		public Button btnNewGame;
		public Label lblMoveCountLabel;
		public Label lblMoveCount;

		/// <summary>
		/// Tracks if the game has just been saved
		/// </summary>
		public bool justSaved;


		/// <summary>
		/// Initializes the game
		/// </summary>
		public GameBoard()
		{
			InitializeComponent();

			//Set up the game form
			initializeForm();

			//Display the main menu
			initializeGameMenu();
		}


		/// <summary>
		/// Set up the game form
		/// </summary>
		private void initializeForm()
		{
			this.Text = "nPuzzle";
			this.MaximizeBox = false;
			this.FormBorderStyle = FormBorderStyle.Fixed3D;
			this.StartPosition = FormStartPosition.CenterScreen;
		}


		/// <summary>
		/// Creates the main game menu
		/// </summary>
		private void initializeGameMenu()
		{
			//Clear the form
			this.Controls.Clear();

			//Create board width label
			Label lblBoardWidth = new Label();
			lblBoardWidth.Text = "Board Width: ";
			lblBoardWidth.AutoSize = true;
			lblBoardWidth.Location = new Point(GUID.PADDING_LARGE,
				GUID.PADDING_MEDIUM);
			this.Controls.Add(lblBoardWidth);

			//Allow the user to select the board width
			numBoardWidth = new NumericUpDown();
			numBoardWidth.Minimum = GUID.MIN_BOARD_SIZE;
			numBoardWidth.Maximum = GUID.MAX_BOARD_SIZE;
			numBoardWidth.Value = GUID.DEFAULT_BOARD_SIZE;
			numBoardWidth.Width = GUID.SIZE_LARGE;
			numBoardWidth.Location = new Point(lblBoardWidth.Location.X + lblBoardWidth.Width + GUID.PADDING_SMALL,
				(lblBoardWidth.Location.Y + (lblBoardWidth.Height / 2)) - (numBoardWidth.Height / 2));
			this.Controls.Add(numBoardWidth);

			//Create board height label
			Label lblBoardHeight = new Label();
			lblBoardHeight.Text = "Board Height: ";
			lblBoardHeight.AutoSize = true;
			lblBoardHeight.Location = new Point(GUID.PADDING_LARGE, 
				numBoardWidth.Location.Y + numBoardWidth.Height + GUID.PADDING_MEDIUM);
			this.Controls.Add(lblBoardHeight);

			//Allow the user to select the board height
			numBoardHeight = new NumericUpDown();
			numBoardHeight.Minimum = GUID.MIN_BOARD_SIZE;
			numBoardHeight.Maximum = GUID.MAX_BOARD_SIZE;
			numBoardHeight.Value = GUID.DEFAULT_BOARD_SIZE;
			numBoardHeight.Width = GUID.SIZE_LARGE;
			numBoardHeight.Location = new Point(lblBoardHeight.Location.X + lblBoardHeight.Width + GUID.PADDING_SMALL,
				(lblBoardHeight.Location.Y + (lblBoardHeight.Height / 2)) - (numBoardHeight.Height / 2));
			this.Controls.Add(numBoardHeight);

			//Create the generate new game button
			btnGenerateBoard = new Button();
			btnGenerateBoard.Text = "Generate Board";
			btnGenerateBoard.Width = (numBoardHeight.Location.X + numBoardHeight.Width) - lblBoardHeight.Location.X;
			btnGenerateBoard.Height = GUID.SIZE_LARGE;
			btnGenerateBoard.Location = new Point(GUID.PADDING_LARGE,
				numBoardHeight.Location.Y + numBoardHeight.Height + GUID.PADDING_MEDIUM);
			btnGenerateBoard.Click += btnGenerateBoard_Click;
			this.Controls.Add(btnGenerateBoard);

			//Create the Load game button
			btnLoadGame = new Button();
			btnLoadGame.Text = "Load Game";
			btnLoadGame.Width = btnGenerateBoard.Width;
			btnLoadGame.Height = GUID.SIZE_LARGE;
			btnLoadGame.Location = new Point(GUID.PADDING_LARGE,
				btnGenerateBoard.Location.Y + btnGenerateBoard.Height + GUID.PADDING_SMALL);
			btnLoadGame.Click += btnLoadGame_Click;
			this.Controls.Add(btnLoadGame);

			//Set the form size
			this.ClientSize = new Size(numBoardHeight.Location.X + numBoardHeight.Width + GUID.PADDING_LARGE,
				btnLoadGame.Location.Y + btnLoadGame.Height + GUID.PADDING_MEDIUM);
		}


		/// <summary>
		/// Allows the user to generate a new board based on their height and width specifications
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGenerateBoard_Click(object sender, EventArgs e)
		{
			//Store the board settings for a new game
			try
			{
				Square.BoardLengthX = (int)numBoardWidth.Value;
				Square.BoardLengthY = (int)numBoardHeight.Value;
			}
			catch (Exception)
			{
				MessageBox.Show("Please enter a valid length and height");
				return;
			}

			//Initialize the game
			initializeGameBoard();
		}


		/// <summary>
		/// Allows the user to generate a board from a save file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLoadGame_Click(object sender, EventArgs e)
		{
			string filePath = "";

			//Create an open file dialogue to allow the user to select a save file to load
			OpenFileDialog loadFile = new OpenFileDialog();
			loadFile.Title = "Select Save File";
			loadFile.CheckFileExists = true;
			loadFile.CheckPathExists = true;
			loadFile.Filter = "nPuzzle Save | *.k3n";
			loadFile.DefaultExt = "k3n";
			loadFile.AddExtension = true;
			loadFile.InitialDirectory = "E:\\Documents\\Kendall\\Conestoga\\Fall 2015\\KRAssignment3";

			DialogResult loadResult = loadFile.ShowDialog();
			switch (loadResult)
			{
				case DialogResult.OK:
					filePath = loadFile.FileName;
					break;
				default:
					return;
			}

			k3nFile.loadGame(filePath);

			//Initialize the game board
			initializeGameBoard(gameLoad: true);
		}


		/// <summary>
		/// Creates a new game board and prepares to load/generate a game
		/// </summary>
		/// <param name="gameLoad">If a saved game has been loaded</param>
		private void initializeGameBoard(bool gameLoad = false)
		{
			//Clears the board
			this.Controls.Clear();

			//If the game is being generated, there are several extra steps
			if (gameLoad == false)
			{
				//Initialize the Squares array with the user size input
				generateSquares(Square.BoardLengthX, Square.BoardLengthY);

				//Randomize the squares on the board (according to the number of squares to the power of 2)
				randomizeSquares((int)Math.Pow((Square.BoardLengthX * Square.BoardLengthY), 2));

				//Remove the progress bar after it has finished loading
				this.Controls.Remove(proGenerateProgress);

				//The game has not yet been saved
				justSaved = false;
			}
			else
			{
				//If a game save was loaded, it has "just been saved"
				justSaved = true;
			}

			//Finish setting up the board
			finishSetup();
		}


		/// <summary>
		/// Generates a new board of Squares
		/// </summary>
		/// <param name="boardWidth">Width of the board</param>
		/// <param name="boardHeight">Height of the board</param>
		private void generateSquares(int boardWidth, int boardHeight)
		{
			//Initialize the array of Squares that form the Game Board
			Square.SquaresArray = new Square[boardWidth, boardHeight];

			//Calculate the number of squares in the puzzle
			int numberOfSquares = boardWidth * boardHeight - 1;

			//Keep track of each square's value
			int squareValueCounter = 0;

			for (int y = 0; y < boardHeight; y++)
			{
				for (int x = 0; x < boardWidth; x++)
				{					
					if (squareValueCounter++ == numberOfSquares)
					{
						//Store the location of the open square
						Square.OpenX = x;
						Square.OpenY = y;

						return;
					}

					//Create the new square and add it to the game board
					Square currentSquare = new Square(x, y, squareValueCounter);
				}
			}
		}


		/// <summary>
		/// Randomizes the Squares in the board according to the number of Squares
		/// </summary>
		private void randomizeSquares(int randomMovesCap)
		{
			//Create and display the progress bar
			generateProgressBar(randomMovesCap);

			//Track the number of random moves performed
			int randomMoves = 0;

			//Create a new Random generator to move the squares randomly
			Random random = new Random();

			//Perform random moves until the number of random moves is equal to the random moves cap
			while (randomMoves < randomMovesCap)
			{
				try
				{
					//Get a randomized direction
					Direction randomDirection = (Direction)random.Next(0, 4);

					//Perform a random move in the corresponding direction
					switch (randomDirection)
					{
						case Direction.Up:
							Square.moveSquare(Square.SquaresArray[Square.OpenX, Square.OpenY - 1]);
							break;
						case Direction.Right:
							Square.moveSquare(Square.SquaresArray[Square.OpenX - 1, Square.OpenY]);
							break;
						case Direction.Down:
							Square.moveSquare(Square.SquaresArray[Square.OpenX, Square.OpenY + 1]);
							break;
						case Direction.Left:
							Square.moveSquare(Square.SquaresArray[Square.OpenX + 1, Square.OpenY]);
							break;
						default:
							//Throw an exception if the random direction is invalid
							throw new Exception("Invalid Square");
					}
				}
				catch (Exception)
				{
					//If the selected square is not valid, continue with the next iteration of the loop
					continue;
				}

				//Increment the random moves counter and the progress bar
				++randomMoves;
				proGenerateProgress.PerformStep();
				proGenerateProgress.Update();
			}
		}


		/// <summary>
		/// Creates a Progress Bar to an approximation of the display Board randomization progress
		/// </summary>
		private void generateProgressBar(int randomMoveCap)
		{
			//Create the progress bar for board generation
			proGenerateProgress = new ProgressBar();
			proGenerateProgress.Minimum = 0;
			proGenerateProgress.Maximum = randomMoveCap;
			proGenerateProgress.Value = 0;
			proGenerateProgress.Step = 1;
			proGenerateProgress.Width = this.ClientSize.Width - (GUID.PADDING_LARGE * 2);
			proGenerateProgress.Height = GUID.SIZE_MEDIUM;
			proGenerateProgress.Location = new Point((this.ClientSize.Width / 2) - (proGenerateProgress.Width / 2),
				(this.ClientSize.Height / 2) - (proGenerateProgress.Height / 2));
			this.Controls.Add(proGenerateProgress);
		}


		/// <summary>
		/// Completes the Game Board setup
		/// </summary>
		private void finishSetup()
		{
			//Add each square to the Game Board, and set its Click Event
			foreach (Square square in Square.SquaresArray)
			{
				if (square != null)
				{
					this.Controls.Add(square);
					square.Click += Square_Click; 
				}
			}

			//Re-adjust the window size and display location
			this.ClientSize = new Size((GUID.PADDING_SMALL * 2) + (Square.BoardLengthX * Square.SQUARE_SIZE),
				GUID.PADDING_LARGE + (Square.BoardLengthY * Square.SQUARE_SIZE) + GUID.PADDING_SMALL);
			this.CenterToScreen();

			//Create the move counter and new game/save buttons
			createMoveCounter();
			createSaveButton();
			createNewGameButton();

			//Update the board display
			updateBoard();
		}


		/// <summary>
		/// Create the Move Counter display
		/// </summary>
		private void createMoveCounter()
		{
			//Create and display the move counter text label
			lblMoveCountLabel = new Label();
			lblMoveCountLabel.Text = "Moves:";
			lblMoveCountLabel.ForeColor = Color.Maroon;
			lblMoveCountLabel.Location = new Point(GUID.PADDING_SMALL, GUID.PADDING_SMALL);
			lblMoveCountLabel.AutoSize = true;
			this.Controls.Add(lblMoveCountLabel);

			//Create and display the move counter
			lblMoveCount = new Label();
			lblMoveCount.Text = Square.MoveCounter.ToString();
			lblMoveCount.Location = new Point(lblMoveCountLabel.Location.X + lblMoveCountLabel.Width, GUID.PADDING_SMALL);
			lblMoveCount.AutoSize = true;
			this.Controls.Add(lblMoveCount);
		}


		/// <summary>
		/// Create the Save Game button
		/// </summary>
		private void createSaveButton()
		{
			btnSaveGame = new Button();
			btnSaveGame.Text = "Save";
			btnSaveGame.Location = new Point(this.ClientSize.Width - btnSaveGame.Width - GUID.PADDING_SMALL, GUID.PADDING_SMALL);
			btnSaveGame.Click += btnSaveGame_Click;
			this.Controls.Add(btnSaveGame);
		}


		/// <summary>
		/// Create the New Game button
		/// </summary>
		private void createNewGameButton()
		{
			btnNewGame = new Button();
			btnNewGame.Text = "New Game";
			btnNewGame.Location = new Point(btnSaveGame.Location.X - btnNewGame.Width - GUID.PADDING_SMALL, GUID.PADDING_SMALL);
			btnNewGame.Click += btnNewGame_Click;
			this.Controls.Add(btnNewGame);
		}

		/// <summary>
		/// Updates the board and redisplays the position of each button
		/// </summary>
		private void updateBoard()
		{
			//Update the move counter
			lblMoveCount.Text = Square.MoveCounter.ToString();

			//Update the position of each square
			foreach (Square square in Square.SquaresArray)
			{
				if (square != null)
				{
					square.Location = new Point(GUID.PADDING_SMALL + (square.SquareX * Square.SQUARE_SIZE),
						GUID.PADDING_LARGE + (square.SquareY * Square.SQUARE_SIZE)); 
				}
			}
		}


		/// <summary>
		/// Click handler for each square
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Square_Click(object sender, EventArgs e)
		{
			//Check if the clicked Square has a valid move
			Direction moveDirection = Square.determineValidMove((Square)sender);

			//If there are no valid moves, exit the method
			if (moveDirection == Direction.None)
			{
				return;
			}

			//Move the clicked Square
			Square.moveSquare((Square)sender);

			//Update the game board
			updateBoard();

			//Increment the move counter
			lblMoveCount.Text = (++Square.MoveCounter).ToString();

			//The game has no longer been "just saved"
			justSaved = false;

			//Check win
			if (checkWin() == true)
			{
				//Prompt the user to choose if they wish to play again
				DialogResult newGame = MessageBox.Show(
					"You have won in " + Square.MoveCounter + " moves!\nDo you want to play again?",
					"New Game?",
					MessageBoxButtons.YesNo);

				switch (newGame)
				{
					//If the user wants to play again, display the main menu
					case DialogResult.Yes:
						initializeGameMenu();
						break;
					//If the user doesn't want to play again, exit the game
					case DialogResult.No:
						this.Close();
						break;
					default:
						break;
				}
			}
		}


		/// <summary>
		/// Determine if the puzzle has been solved
		/// </summary>
		/// <returns>Whether the puzzle is solved or not</returns>
		private bool checkWin()
		{
			//Counter to keep track of what the next square's value should be
			int squareValueCounter = 0;

			for (int y = 0; y < Square.BoardLengthY; y++)
			{
				for (int x = 0; x < Square.BoardLengthX; x++)
				{
					//If the current square is null, but not the last space on the board, return false
					//	Otherwise, if the current square's value is not sequentially correct, return false
					if (Square.SquaresArray[x, y] == null)
					{
						//If the current square is the last one on the board, and the sequence is unbroken, return true
						if (squareValueCounter + 1== Square.BoardLengthX * Square.BoardLengthY)
						{
							return true;
						}

						return false;
					}					
					else if (Square.SquaresArray[x, y].SquareValue != ++squareValueCounter)
					{
						return false;
					}
					
				}
			}

			return false;
		}


		/// <summary>
		/// Allows the user save the current game state
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSaveGame_Click(object sender, EventArgs e)
		{
			string filePath = "";

			//Create and display the Save file dialogue
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Title = "Save Game File";
			saveFile.CheckPathExists = true;
			saveFile.Filter = "nPuzzle Save | *.k3n";
			saveFile.DefaultExt = "k3n";
			saveFile.AddExtension = true;
			saveFile.InitialDirectory = "E:\\Documents\\Kendall\\Conestoga\\Fall 2015\\KRAssignment3";

			DialogResult loadResult = saveFile.ShowDialog();
			switch (loadResult)
			{
				case DialogResult.OK:
					filePath = saveFile.FileName;
					break;
				default:
					return;
			}

			//Save the file to the specified file path
			k3nFile.saveGame(filePath);

			//The game has just been saved
			justSaved = true;
		}


		/// <summary>
		/// Allow the user to start a new game
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnNewGame_Click(object sender, EventArgs e)
		{
			//If the game was just saved, start a new game automatically
			if (justSaved == true)
			{				
				initializeGameMenu();
				return;
			}

			//Otherwise, prompt the user to save it and act according to their selection
			DialogResult newGameResult = MessageBox.Show("Do you want to save the current game?", "Save Current Game?",
					MessageBoxButtons.YesNoCancel);

			switch (newGameResult)
			{
				case DialogResult.Cancel:
					return;
				case DialogResult.Yes:
					btnSaveGame.PerformClick();
					break;
				case DialogResult.No:
					initializeGameMenu();
					break;
				default:
					break;
			}
		}

		
		/// <summary>
		/// Checks at form exiting if the user has saved their game, and prompts them if not
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameBoard_FormClosing(object sender, FormClosingEventArgs e)
		{
			//If the game has not been saved, prompt the user to do so and act according to their selection
			if (justSaved == false)
			{
				DialogResult gameExitResult = MessageBox.Show("Do you want to save the current game?", "Save Current Game?",
					MessageBoxButtons.YesNoCancel);

				switch (gameExitResult)
				{
					case DialogResult.Cancel:
						e.Cancel = true;
						return;
					case DialogResult.Yes:
						btnSaveGame.PerformClick();
						break;
					case DialogResult.No:
						break;
					default:
						break;
				}
			}
		}
	}
}