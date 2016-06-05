using UnityEngine;
using System.Collections.Generic;
using System;

namespace Application {
	
	public class LogicController : MonoBehaviour
	{	


		public  LogicController (Board gameBoard) {

		}

		public int getOpponent (int playerNo) {
			if (playerNo == 1)
				return 2;
			else 
				return 1;
		}

		public bool canMove(int x, int y, Board gameBoard) {
			return (canMoveDownAndLeft (x, y, gameBoard) || canMoveDownAndRight (x, y, gameBoard) || canMoveUpAndLeft (x, y, gameBoard) || canMoveUpAndRight (x, y, gameBoard));
		}
		
		public bool canTake(int x, int y, Board gameBoard) {
			return (canTakeUpAndLeft (x, y, gameBoard) || canTakeUpAndRight (x, y, gameBoard) || canTakeDownAndLeft (x, y, gameBoard) || canTakeDownAndRight (x, y, gameBoard));
		}

		public bool canMoveDownAndRight (int x, int y, Board gameBoard) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 1 || piece.isKing == true) && ((y + 1 < 8) && (x + 1 < 8)) && gameBoard.returnPlayerPiece (x + 1, y + 1) == null) {
				return true;
			}
			return false;
		}
		
		public bool canMoveDownAndLeft (int x, int y, Board gameBoard) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 1 || piece.isKing == true) && ((y - 1 > -1) && (x + 1 < 8)) && gameBoard.returnPlayerPiece (x + 1, y - 1) == null) {
				return true;
			}
			return false;	
		}
		
		public bool canMoveUpAndRight (int x, int y, Board gameBoard) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 2 || piece.isKing == true) && ((y + 1 < 8) && (x - 1 > -1)) && gameBoard.returnPlayerPiece (x - 1, y + 1) == null) {
				return true;
			}
			return false;
		}
		
		public bool canMoveUpAndLeft (int x, int y, Board gameBoard) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 2 || piece.isKing == true) && ((y - 1 > -1) && (x - 1 > -1)) && gameBoard.returnPlayerPiece (x - 1, y - 1) == null) {
				return true;
			}
			return false;
		}
		
		public bool canTakeDownAndRight (int x, int y, Board gameBoard) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 1 || currentPiece.isKing == true) && ((y + 2 < 8) && (x + 2 < 8))) {
				PlayerPiece enemy = gameBoard.returnPlayerPiece (x + 1, y + 1);
				if (enemy != null && enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ()) && gameBoard.returnPlayerPiece (x + 2, y + 2) == null) {
					return true;
				}
			}
			return false;
		}
		
		public bool canTakeDownAndLeft (int x, int y, Board gameBoard) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 1 || currentPiece.isKing == true) && ((y - 2 > -1) && (x + 2 < 8))) {
				PlayerPiece enemy = gameBoard.returnPlayerPiece (x + 1, y - 1);
				if (enemy != null && enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ()) && gameBoard.returnPlayerPiece (x + 2, y - 2) == null) {
					return true;
				}
			}
			return false;
		}
		
		public bool canTakeUpAndLeft (int x, int y, Board gameBoard) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 2 || currentPiece.isKing == true) && ((y - 2 > -1) && (x - 2 > -1))) {
				PlayerPiece enemy = gameBoard.returnPlayerPiece (x - 1, y - 1);
				if (enemy != null && enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ()) && gameBoard.returnPlayerPiece (x - 2, y - 2) == null) {
					return true;
				}
			} 
			return false;
		}
		
		public bool canTakeUpAndRight (int x, int y, Board gameBoard) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 2 || currentPiece.isKing == true) && ((x - 2 > -1) && (y + 2 < 8))) {
				PlayerPiece enemy = gameBoard.returnPlayerPiece (x - 1, y + 1);
				if (enemy != null && enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ()) && gameBoard.returnPlayerPiece (x - 2, y + 2) == null) {
					return true;
				}
			}
			return false;
		}

		public Boolean playerHasTakeableMoves (int playerNumber, Board gameBoard) {
			int i = 0, j = 0;
			while (j < 8) {
				while (i < 8) {
					if ((canTakeDownAndRight (i, j, gameBoard) || canTakeDownAndLeft (i, j, gameBoard) || canTakeUpAndRight (i, j, gameBoard) || canTakeUpAndLeft (i, j, gameBoard)) && gameBoard.returnPlayerPiece (i, j).playerNo == playerNumber) {
						return true;
					}
					i++;
				}
				j++;
				i = 0;
			}
			return false;
		}
	}
}