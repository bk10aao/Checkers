using UnityEngine;
using System.Collections;
namespace Application {

	public class Board {
		
		public PlayerPiece[,] boardPieces = new PlayerPiece[8, 8];

		public void SetupPlayerArray() {	
			PlayerPiece piece;
			
			int x = 0, y = 0, player = 1;
			int count = 1;
			while (x < 8) {
				if (x == 3) {
					x = x + 2;
					player = 2;
					count = 1;
				}
				else {
					if (y % 2 == 0 && x % 2 == 0) {
						piece = new PlayerPiece(player, false);
						boardPieces [x, y] = piece;
						count++;
						y++;
					}
					else {
						if (y % 2 == 1 && x % 2 == 1) {
							piece = new PlayerPiece(player, false);
							boardPieces [x, y] = piece;
							count++;
							y++;
						}
						else {
							boardPieces [x, y] = null;
							y++;
						}
					}
					if (y > 7) {
						y = 0;
						x++;
					}
				}
			}
		}
		
		public PlayerPiece returnPlayerPiece(int x, int y) {
			return boardPieces [x, y];
		}
		
		public void removePiece(int x, int y) {
			boardPieces [x, y] = null;
		}
		
		public void AddPlayerPiece(PlayerPiece piece, int x, int y) {
			boardPieces [x, y] = piece;
		}
		
	}
}