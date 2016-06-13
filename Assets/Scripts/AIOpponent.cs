using UnityEngine;
using System.Collections;

namespace Application {

	public class AIOpponent : MonoBehaviour {

		ArrayList moveablePieces;
		ArrayList takeablePieces = new ArrayList();
		Board gameBoard;
		LogicController logicController;

		public AIOpponent(Board gameBoard) {
			gameBoard = this.gameBoard;
			logicController = new LogicController (gameBoard);
		}

		/**public void getMoveablePiecesInArray() {
			moveablePieces = new ArrayList();

			int i = 0, j = 0;

			while (j < 8) {
				while (i < 8) {
					if ((logicController.canMoveDownAndLeft (i, j, gameBoard) || logicController.canMoveDownAndRight (i, j, gameBoard) || 
					     logicController.canMoveUpAndLeft (i, j, gameBoard) || logicController.canMoveUpAndRight (i, j, gameBoard)) 
					     && gameBoard.returnPlayerPiece (i, j).playerNo == 2) {

						moveablePieces.Add(gameBoard.returnPlayerPiece(i,j));
					}
					i++;
				}
				j++;
				i = 0;
			}
		}

		public void getTakeablePieces() {
			takeablePieces = new ArrayList ();

			int i = 0, j = 0;
							
			while (j < 8) {
				while (i < 8) {
					if ((logicController.canTakeDownAndRight (i, j, gameBoard) || logicController.canTakeDownAndLeft (i, j, gameBoard) || 
					     logicController.canTakeUpAndRight (i, j, gameBoard) || logicController.canTakeUpAndLeft (i, j, gameBoard)) 
					     && gameBoard.returnPlayerPiece (i, j).playerNo == 2) {

						takeablePieces.Add(gameBoard.returnPlayerPiece(i,j));
					}
					i++;
				}
				j++;
				i = 0;
			}
		}

		public void moveAIPiece() {
			if (moveablePieces.Count > 0) {
				PlayerPiece AIpiece = moveablePieces[0]; 

				if (logicController.canMoveDownAndLeft(AIpiece.
			}
		}**/
	}

}
/**
 * public Boolean playerHasTakeableMoves (int playerNumber, Board gameBoard) {
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

 * **/