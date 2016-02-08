/*
 * Project:		KRAssignment3
 * Purpose:		Utility class
 *
 * History:
 *		Kendall Roth	2015-10-23:		Created
 *										Constants and static fields added
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRAssignment3
{
	/// <summary>
	/// Utility class for game constants
	/// </summary>
	class GUID
	{
		public const int PADDING_SMALL = 10;
		public const int PADDING_MEDIUM = 25;
		public const int PADDING_LARGE = 50;

		public const int SIZE_SMALL = 10;
		public const int SIZE_MEDIUM = 25;
		public const int SIZE_LARGE = 50;

		public const int MIN_BOARD_SIZE = 3;
		public const int MAX_BOARD_SIZE = 10;
		public const int DEFAULT_BOARD_SIZE = 4;
	}
}
