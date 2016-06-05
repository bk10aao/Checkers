using UnityEngine;
using System.Collections.Generic;
using System;

namespace Application {
	
	public class ControllerV13 : MonoBehaviour {
		public Board gameBoard = new Board();
		
		RaycastHit hit;
		
		GameObject interactionPiece;
		
		float startPosX, startPosZ;
		
		int playerNo = 1;
		
		Boolean pieceChangedToKing = false;

		private LogicController logicController;// = new LogicController ();

		List<PlayerPiece> moveablePieces = new List<PlayerPiece> ();
		List<PlayerPiece> takeablePieces = new List<PlayerPiece> ();
		
		Ray rayRay = new Ray();
		
		void Start () {
			gameBoard.SetupPlayerArray ();
			logicController = new LogicController (gameBoard);
		}
		
		void Update () {
			takeablePieces.Clear ();
			moveablePieces.Clear ();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			//WORKS PICKING UP AND PUTTING DOWN OBJECTS BUT NOT DRAGGING
			if (Input.GetMouseButtonDown (0)) {
				if ((Physics.Raycast (ray, out hit)) && hit.collider.tag.Contains("Player") ) {	
					startPosX = hit.collider.transform.localPosition.x;
					startPosZ = hit.collider.transform.localPosition.z;
					interactionPiece = hit.collider.gameObject;
					
					if(logicController.playerHasTakeableMoves(playerNo, gameBoard)) {
						if (interactionPiece.tag.Contains("Player" + playerNo.ToString()) && canTake((int)startPosX, (int)startPosZ)) {
							interactionPiece.transform.position = new Vector3 (hit.collider.transform.localPosition.x, 1.0f, hit.collider.transform.localPosition.z);
						}
						else {
							interactionPiece = null;
						}
					}
					else if (logicController.canMove((int)startPosX, (int)startPosZ, gameBoard)) {
						if (interactionPiece.tag.Contains("Player" + playerNo.ToString())) {
							interactionPiece.transform.position = new Vector3 (hit.collider.transform.localPosition.x, 1.0f, hit.collider.transform.localPosition.z);
						}
						else {
							interactionPiece = null;
						}
					}
					else {
						interactionPiece = null;
					}
				}
				else {
					interactionPiece = null;
				}
			}
			if (Input.GetMouseButtonUp (0) && interactionPiece != null && Physics.Raycast (ray, out hit)) {
				float tempz = hit.collider.transform.localPosition.z;
				float tempx = hit.collider.transform.localPosition.x;
				if (logicController.canTake ((int)startPosX, (int)startPosZ, gameBoard)) {
					take (tempx, tempz);
				}
				else if (logicController.canMove ((int)startPosX, (int)startPosZ, gameBoard)) {
					move ((int)tempx, (int)tempz);
				}
				else {
					interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosX);
				}
			}
		}
		
		private bool canMove(int x, int y) {
			return (logicController.canMoveDownAndLeft (x, y, gameBoard) || logicController.canMoveDownAndRight (x, y, gameBoard) || logicController.canMoveUpAndLeft (x, y, gameBoard) || logicController.canMoveUpAndRight (x, y, gameBoard));
		}
		
		private bool canTake(int x, int y) {
			return (logicController.canTakeUpAndLeft (x, y, gameBoard) || logicController.canTakeUpAndRight (x, y, gameBoard) || logicController.canTakeDownAndLeft (x, y, gameBoard) || logicController.canTakeDownAndRight (x, y, gameBoard));
		}
		
		void move (int tempx, int tempz) {
			interactionPiece.transform.position = new Vector3(startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && logicController.canMoveDownAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				moveDownAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx > startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && logicController.canMoveDownAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				moveDownAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && ((tempz - startPosZ) < 1.5) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && logicController.canMoveUpAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				moveUpAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && ((tempz - startPosZ) < 0) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && logicController.canMoveUpAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				moveUpAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			}
		}
		
		void take (float tempx, float tempz)
		{
			interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 2.5)) && logicController.canTakeDownAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeDownAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(2.0f, 2.0f);
			}
			else if ((tempx > startPosX) && (tempz - startPosZ < -1.5) && ((tempz - startPosZ) > -2.5) && logicController.canTakeDownAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeDownAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(2.0f, -2.0f);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ < -1.5) && (tempz - startPosZ > -3)) && (tempx - startPosX < -1) && logicController.canTakeUpAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeUpAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(-2.0f, -2.0f);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 3)) && (tempx - startPosX < -1) && logicController.canTakeUpAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeUpAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(-2.0f, 2.0f);
			}
			else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			}
		}
		
		private void destroyPiece(float directionX, float directionZ) {
			interactionPiece.transform.position = new Vector3 (startPosX + directionX, 0.1f, startPosZ + directionZ);
			rayRay.origin = new Vector3 (startPosX, 0.1f, startPosZ);
			rayRay.direction = new Vector3 (directionX, 0.1f, directionZ);
			if (Physics.Raycast (rayRay, out hit)) {
				Destroy (hit.transform.gameObject);
			}
			if (!canTake ((int)startPosX + (int)directionX, (int)startPosZ + (int)directionZ) && pieceChangedToKing == false) {
				changePlayer ();
			}
		}
		
		private void changePlayer () {
			if (playerNo == 1) 
				playerNo = 2;
			else 
				playerNo = 1;
			pieceChangedToKing = false;
		}
		
		private void transformPieceToKing(PlayerPiece piece, GameObject playerPiece) {
			piece.isKing = true;
			playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
			changePlayer();
			pieceChangedToKing = true;
		}

		private void takeUpAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canTakeUpAndRight (x, y, gameBoard)) {;
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				gameBoard.removePiece (x, y);
				gameBoard.removePiece (x - 1, y + 1);
				gameBoard.AddPlayerPiece (piece, x - 2, y + 2);
				if(piece.isKing == false && x-2 == 0) {
					transformPieceToKing(piece, playerPiece);
				}
			}
		}
		
		private void takeUpAndLeft (int x, int y, GameObject playerPiece) {
			if (logicController.canTakeUpAndLeft (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				gameBoard.removePiece (x, y);
				gameBoard.removePiece (x - 1, y - 1);
				gameBoard.AddPlayerPiece (piece, x - 2, y - 2);
				if(piece.isKing == false && x - 2  == 0) {
					transformPieceToKing(piece, playerPiece);
				}
			}
		}
		
		private void takeDownAndLeft(int x, int y, GameObject playerPiece) {
			if (logicController.canTakeDownAndLeft (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece(x, y);
				gameBoard.removePiece(x, y);
				gameBoard.removePiece(x + 1, y - 1);
				gameBoard.AddPlayerPiece(piece, x + 2, y - 2);
				if(piece.isKing == false && x + 2 == 7) {
					transformPieceToKing(piece, playerPiece);
				}
			}
		}
		
		private void takeDownAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canTakeDownAndRight (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				gameBoard.removePiece (x, y);
				gameBoard.removePiece(x + 1, y + 1);
				gameBoard.AddPlayerPiece(piece, x + 2, y + 2);
				if(piece.isKing == false && x + 2 == 7) {
					transformPieceToKing(piece, playerPiece);
				}
			}
		}

		private void updateGameBoardOnMove (int x, int y, int moveToPosX, int moveToPosY, int playerNumber, GameObject playerPiece, PlayerPiece piece) {
			if (piece.playerNo == playerNumber || piece.isKing == true) {
				gameBoard.removePiece (x, y);
				gameBoard.AddPlayerPiece (piece, x + moveToPosX , y + moveToPosY);
				if (piece.isKing == false && x + moveToPosX == 0) {
					piece.isKing = true;
					playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
				}
			}
		}
		
		private void moveDownAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canMoveDownAndRight (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateGameBoardOnMove(x, y, 1, 1, piece.playerNo, playerPiece, piece);
				changePlayer();
			}
		}
		
		private void moveDownAndLeft (int x, int y, GameObject playerPiece) {
			if (logicController.canMoveDownAndLeft (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateGameBoardOnMove(x, y, 1, -1, piece.playerNo, playerPiece, piece);
				changePlayer();
			}
		}
		
		private void moveUpAndLeft (int x, int y, GameObject playerPiece) {
			if (logicController.canMoveUpAndLeft (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateGameBoardOnMove(x, y, -1, -1, piece.playerNo, playerPiece, piece);
				changePlayer();
			}
		}

		private void moveUpAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canMoveUpAndRight (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateGameBoardOnMove(x, y, -1, 1, piece.playerNo, playerPiece, piece);
				changePlayer();
			}
		}
	}
}