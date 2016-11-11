using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Application {

	public class LogicController {	
		
		public  LogicController () {
			
		}

		public int getOpponent (int playerNo) {
			return playerNo = playerNo == 1 ? 2 : 1;
		}
		
		public bool canMove(int x, int y, Board gameBoard) {
			return (canMoveDownAndLeft (x, y, gameBoard) || canMoveDownAndRight (x, y, gameBoard) || canMoveUpAndLeft (x, y, gameBoard) || canMoveUpAndRight (x, y, gameBoard));
		}
		
		public bool canTake(int x, int y, Board gameBoard) {
			return (canTakeUpAndLeft (x, y, gameBoard) || canTakeUpAndRight (x, y, gameBoard) || canTakeDownAndLeft (x, y, gameBoard) || canTakeDownAndRight (x, y, gameBoard));
		}

		static bool checkComponentMove (PlayerPiece piece, int playerNumber) {
			return (piece != null) && (piece.playerNo == playerNumber || piece.isKing == true);
		}

		public bool canMoveDownAndRight (int x, int y, Board gameBoard) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			return (checkComponentMove (piece, 1) && ((y + 1 < 8) && (x + 1 < 8)) && gameBoard.returnPlayerPiece (x + 1, y + 1) == null);

		}
		
		public bool canMoveDownAndLeft (int x, int y, Board gameBoard) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			return  (checkComponentMove (piece, 1) && ((y - 1 > -1) && (x + 1 < 8)) && gameBoard.returnPlayerPiece (x + 1, y - 1) == null);
		}
		
		public bool canMoveUpAndRight (int x, int y, Board gameBoard) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			return (checkComponentMove (piece, 2) && ((y + 1 < 8) && (x - 1 > -1)) && gameBoard.returnPlayerPiece (x - 1, y + 1) == null);
		}
		
		public bool canMoveUpAndLeft (int x, int y, Board gameBoard) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			return (checkComponentMove (piece, 2) && ((y - 1 > -1) && (x - 1 > -1)) && gameBoard.returnPlayerPiece (x - 1, y - 1) == null);
		}

		static bool checkTakeComponentInitialPosition (PlayerPiece currentPiece, int playerNumber) {
			return (currentPiece != null && (currentPiece.playerNo == playerNumber || currentPiece.isKing == true));
		}

		bool checkTakeEnemy (PlayerPiece currentPiece, PlayerPiece enemy) {
			return (enemy != null && enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ()));
		}
		
		public bool canTakeDownAndRight (int x, int y, Board gameBoard) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (checkTakeComponentInitialPosition (currentPiece, 1) && ((y + 2 < 8) && (x + 2 < 8))) {
				PlayerPiece enemy = gameBoard.returnPlayerPiece (x + 1, y + 1);
				return (checkTakeEnemy (currentPiece, enemy) && gameBoard.returnPlayerPiece (x + 2, y + 2) == null);
			}
			return false;
		}
		
		public bool canTakeDownAndLeft (int x, int y, Board gameBoard) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (checkTakeComponentInitialPosition (currentPiece, 1) && ((y - 2 > -1) && (x + 2 < 8))) {
				PlayerPiece enemy = gameBoard.returnPlayerPiece (x + 1, y - 1);
				return (checkTakeEnemy (currentPiece, enemy) && gameBoard.returnPlayerPiece (x + 2, y - 2) == null); 
			}
			return false;
		}
		
		public bool canTakeUpAndLeft (int x, int y, Board gameBoard) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (checkTakeComponentInitialPosition (currentPiece, 2) && ((y - 2 > -1) && (x - 2 > -1))) {
				PlayerPiece enemy = gameBoard.returnPlayerPiece (x - 1, y - 1);
				return (checkTakeEnemy (currentPiece, enemy) && gameBoard.returnPlayerPiece (x - 2, y - 2) == null);
			} 
			return false;
		}
		
		public bool canTakeUpAndRight (int x, int y, Board gameBoard) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (checkTakeComponentInitialPosition (currentPiece, 2) && ((x - 2 > -1) && (y + 2 < 8))) {
				PlayerPiece enemy = gameBoard.returnPlayerPiece (x - 1, y + 1);
				return (checkTakeEnemy (currentPiece, enemy) && gameBoard.returnPlayerPiece (x - 2, y + 2) == null);
			}
			return false;
		}
		
		public Boolean playerHasTakeableMoves (int playerNumber, Board gameBoard) {
			int i = 0, j = 0;
			while (j < 8) {
				while (i < 8) {
					if (canTake(i, j, gameBoard) && gameBoard.returnPlayerPiece (i, j).playerNo == playerNumber) {
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