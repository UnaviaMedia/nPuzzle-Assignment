/*
 * Project:		KRAssignment3
 * Purpose:		Save/Load file class
 *
 * History:
 *		Kendall Roth	2015-10-23:		Created (again)
 *										Load game added
 *										Save game added
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KRAssignment3
{
	/// <summary>
	/// Class that models the k3n game state files for the nPuzzle game
	/// </summary>
	public static class k3nFile
	{
		/// <summary>
		/// Loads an nPuzzle game save and sets up the board
		/// </summary>
		/// <param name="filePath">File path to the save file</param>
		public static void loadGame(string filePath)
		{
			//Create a new XML document and load the selected save file
			XmlDocument loadFile = new XmlDocument();
			loadFile.Load(filePath);
			XmlNodeList settings = loadFile.SelectNodes("/nPuzzleGame/*");

			//Iterate over each node in the XML file, and save it as the appropriate setting
			foreach (XmlNode item in settings)
			{
				switch (item.Name)
				{
					//Save the Board Size properties
					case "BoardSize":
						Square.BoardLengthX = int.Parse(item.Attributes["width"].Value);
						Square.BoardLengthY = int.Parse(item.Attributes["height"].Value);
						Square.SquaresArray = new Square[Square.BoardLengthX, Square.BoardLengthX];
						break;
					//Save the position of the Open Square
					case "OpenSquare":
						Square.OpenX = int.Parse(item.Attributes["openX"].Value);
						Square.OpenY = int.Parse(item.Attributes["openY"].Value);
						break;
					//Create the necessary squares for the board
					case "SquaresArray":
						foreach (XmlNode square in loadFile.SelectNodes("//SquaresArray/*"))
						{
							int x = int.Parse(square.Attributes["squareX"].Value);
							int y = int.Parse(square.Attributes["squareY"].Value);
							int value = int.Parse(square.Attributes["squareValue"].Value);
							Square currentSquare = new Square(x, y, value);
						}
						break;
					//Create the move counter
					case "MoveCounter":
						Square.MoveCounter = int.Parse(item.Attributes["counter"].Value);
						break;
					default:
						break;
				}
			}
		}


		/// <summary>
		/// Saves the current game state to a save file
		/// </summary>
		/// <param name="filePath">File path to save the file to</param>
		public static void saveGame(string filePath)
		{
			//Create a new XML document and create the root elements
			XmlDocument saveFile = new XmlDocument();
			XmlNode rootNode = saveFile.CreateElement("nPuzzleGame");
			saveFile.AppendChild(rootNode);

			//Save the current board size attributes
			XmlNode boardSize = saveFile.CreateElement("BoardSize");
			XmlAttribute width = saveFile.CreateAttribute("width");
			width.Value = Square.BoardLengthX.ToString();
			boardSize.Attributes.Append(width);
			XmlAttribute height = saveFile.CreateAttribute("height");
			height.Value = Square.BoardLengthY.ToString();
			boardSize.Attributes.Append(height);
			rootNode.AppendChild(boardSize);

			//Save the currently open square's attributes
			XmlNode openSquare = saveFile.CreateElement("OpenSquare");
			XmlAttribute openX = saveFile.CreateAttribute("openX");
			openX.Value = Square.OpenX.ToString();
			openSquare.Attributes.Append(openX);
			XmlAttribute openY = saveFile.CreateAttribute("openY");
			openY.Value = Square.OpenY.ToString();
			openSquare.Attributes.Append(openY);
			rootNode.AppendChild(openSquare);

			//Save the position and value of each square
			XmlNode squaresArray = saveFile.CreateElement("SquaresArray");
			foreach (Square current in Square.SquaresArray)
			{
				if (current == null)
				{
					continue;
				}

				XmlNode square = saveFile.CreateElement("Square");
				XmlAttribute squareX = saveFile.CreateAttribute("squareX");
				squareX.Value = current.SquareX.ToString();
				square.Attributes.Append(squareX);
				XmlAttribute squareY = saveFile.CreateAttribute("squareY");
				squareY.Value = current.SquareY.ToString();
				square.Attributes.Append(squareY);
				XmlAttribute squareValue = saveFile.CreateAttribute("squareValue");
				squareValue.Value = current.SquareValue.ToString();
				square.Attributes.Append(squareValue);
				squaresArray.AppendChild(square);
			}
			rootNode.AppendChild(squaresArray);

			//Save the move counter's attributes
			XmlNode moveCounter = saveFile.CreateElement("MoveCounter");
			XmlAttribute counter = saveFile.CreateAttribute("counter");
			counter.Value = Square.MoveCounter.ToString();
			moveCounter.Attributes.Append(counter);
			rootNode.AppendChild(moveCounter);

			//Save the XML file to the specified path
			saveFile.Save(filePath);
		}
	}
}
