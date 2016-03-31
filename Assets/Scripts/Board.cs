using UnityEngine;
using System.Collections;
namespace Application{
	public class Board
	{
		public PlayerPiece[,] boardPieces = new PlayerPiece[8, 8];

		void Start ()
		{

		}
		
		// Update is called once per frame
		void Update ()
		{

		}

		public void SetupPlayerArray()
		{	
			PlayerPiece piece;

			int x = 0, y = 0, player = 1;

			while (x < 8) {
				if (x == 3) {
					x = x + 2;
					player = 2;
				}
				else {
					if (y % 2 == 0 && x % 2 == 0) {
						piece = new PlayerPiece(player);
						boardPieces [x, y] = piece;
						y++;
					}
					else
					if (y % 2 == 1 && x % 2 == 1) {
						piece = new PlayerPiece(player);
						boardPieces [x, y] = piece;
						y++;
					}
					else {
						boardPieces [x, y] = null;
						y++;
					}

					if (y > 7) {
						y = 0;
						x++;
					}
				}
			}
		}

		public void printBoard()
		{
			string s = "";
			int x = 0, y = 0;

			while(x < 8) {
				while(y < 8) {
					if(boardPieces[x,y] == null)
					{
					   s+=" null ";
					}
					else
					{
						s += boardPieces[x,y].PrintPlayer() + " ";
					}
					y++;
				}
				
				s += "\n";
				x++;
				y = 0;
			}

			Debug.Log(s);
		}

		public PlayerPiece returnPlayerPiece(int x, int y)
		{
			return boardPieces [x, y];
		}
	}
}


