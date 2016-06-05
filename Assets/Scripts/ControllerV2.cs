using UnityEngine;
using System.Collections.Generic;
using System;

namespace Application {
	
	public class ControllerV2 : MonoBehaviour
	{
		public Board gameBoard = new Board();
		
		RaycastHit hit;
		
		GameObject interactionPiece;
		
		float startPosX, startPosZ;

		int playerNo = 1;

		Boolean pieceChangedToKing = false;
		
		List<PlayerPiece> moveablePieces = new List<PlayerPiece> ();
		List<PlayerPiece> takeablePieces = new List<PlayerPiece> ();

		Ray rayRay = new Ray();

		void Start () {
			gameBoard.SetupPlayerArray ();
		}
		
		void Update () {
			takeablePieces.Clear ();
			moveablePieces.Clear ();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction * 100, Color.red);

			//WORKS PICKING UP AND PUTTING DOWN OBJECTS BUT NOT DRAGGING
			if (Input.GetMouseButtonDown (0)) {
				if ((Physics.Raycast (ray, out hit)) && hit.collider.tag.Contains("Player") ) {	
					startPosX = hit.collider.transform.localPosition.x;
					startPosZ = hit.collider.transform.localPosition.z;
					interactionPiece = hit.collider.gameObject;
					
					if(playerHasTakeableMoves(playerNo)) {
						if (interactionPiece.tag.Contains("Player" + playerNo.ToString()) && canTake((int)startPosX, (int)startPosZ)) {
							interactionPiece.transform.position = new Vector3 (hit.collider.transform.localPosition.x, 1.0f, hit.collider.transform.localPosition.z);
						}
						else {
							interactionPiece = null;
						}
					}
					else if (canMove((int)startPosX, (int)startPosZ)) {
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
			if (Input.GetMouseButtonUp (0) && interactionPiece != null) {
				if (Physics.Raycast (ray, out hit)) {
					
					float tempz = hit.collider.transform.localPosition.z;
					float tempx = hit.collider.transform.localPosition.x;
					
					if(canTake((int)startPosX, (int)startPosZ)) {
						take(tempx, tempz);
					}
					else if (canMove((int)startPosX, (int)startPosZ)) {

						move((int)tempx, (int)tempz);
					}
					else {
						interactionPiece.transform.position = new Vector3(startPosX, 0.1f, startPosX); //TODO this should be start pos?
					}
				}
				else{
					interactionPiece.transform.position = new Vector3(startPosX, 0.1f, startPosX);
				}
			}
		}
		
		private bool canMove(int x, int y) {
			return (canMoveDownAndLeft (x, y) || canMoveDownAndRight (x, y) || canMoveUpAndLeft (x, y) || canMoveUpAndRight (x, y));
		}
		
		private bool canTake(int x, int y) {
			return (canTakeUpAndLeft (x, y) || canTakeUpAndRight (x, y) || canTakeDownAndLeft (x, y) || canTakeDownAndRight (x, y));
		}

		void move (int tempx, int tempz)
		{
			interactionPiece.transform.position = new Vector3(startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && canMoveDownAndRight ((int)startPosX, (int)startPosZ)) {
				moveDownAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx > startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && canMoveDownAndLeft ((int)startPosX, (int)startPosZ)) {
				moveDownAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && ((tempz - startPosZ) < 1.5) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && canMoveUpAndRight ((int)startPosX, (int)startPosZ)) {
				moveUpAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && ((tempz - startPosZ) < 0) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && canMoveUpAndLeft ((int)startPosX, (int)startPosZ)) {
				moveUpAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else {
				moveablePieces.Clear ();
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			}
		}

		void take (float tempx, float tempz)
		{
			interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 2.5)) && canTakeDownAndRight ((int)startPosX, (int)startPosZ)) {
				takeDownAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(2.0f, 2.0f);
			}
			else if ((tempx > startPosX) && (tempz - startPosZ < -1.5) && ((tempz - startPosZ) > -2.5) && canTakeDownAndLeft ((int)startPosX, (int)startPosZ)) {
				takeDownAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(2.0f, -2.0f);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ < -1.5) && (tempz - startPosZ > -3)) && (tempx - startPosX < -1) && canTakeUpAndLeft ((int)startPosX, (int)startPosZ)) {
				takeUpAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(-2.0f, -2.0f);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 3)) && (tempx - startPosX < -1) && canTakeUpAndRight ((int)startPosX, (int)startPosZ)) {
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

		private int getOpponent (int playerNo) {
			if (playerNo == 1)
				return 2;
			else 
				return 1;
		}
		
		private void changePlayer () {
			if (playerNo == 1) {
				playerNo = 2;
			} else {
				playerNo = 1;
			}
			pieceChangedToKing = false;
			takeablePieces = new List<PlayerPiece> ();
			moveablePieces = new List<PlayerPiece> ();
		}
		
		private void transformPieceToKing(PlayerPiece piece, GameObject playerPiece) {
			piece.isKing = true;
			playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
			changePlayer();
			pieceChangedToKing = true;
		}
		
		private void takeUpAndRight (int x, int y, GameObject playerPiece) {
			if (canTakeUpAndRight (x, y)) {
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
			if (canTakeUpAndLeft (x, y)) {
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
			if (canTakeDownAndLeft (x, y)) {
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
			if (canTakeDownAndRight (x, y)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				gameBoard.removePiece (x, y);
				gameBoard.removePiece(x + 1, y + 1);
				gameBoard.AddPlayerPiece(piece, x + 2, y + 2);
				if(piece.isKing == false && x + 2 == 7) {
					transformPieceToKing(piece, playerPiece);
				}
			}
		}
		
		private void moveDownAndRight (int x, int y, GameObject playerPiece) {
			if (canMoveDownAndRight (x, y)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				if(piece.playerNo == 1 || piece.isKing == true) {
					gameBoard.removePiece (x, y);
					gameBoard.AddPlayerPiece (piece, x + 1, y + 1);
					if(piece.isKing == false && x + 1 == 7) {
						piece.isKing = true;
						playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
					}
				}
				changePlayer();
			}
		}
		
		private void moveDownAndLeft (int x, int y, GameObject playerPiece) {
			if (canMoveDownAndLeft (x, y)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				if(piece.playerNo == 1 || piece.isKing == true) {
					gameBoard.removePiece (x, y);
					gameBoard.AddPlayerPiece (piece, x + 1, y - 1);
					if(piece.isKing == false && x + 1 == 7) {
						piece.isKing = true;
						playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
						
					}
				}
				changePlayer();
			}
		}
		
		private void moveUpAndLeft (int x, int y, GameObject playerPiece) {
			if (canMoveUpAndLeft (x, y)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				if(piece.playerNo == 2 || piece.isKing == true) {
					gameBoard.removePiece (x, y);
					gameBoard.AddPlayerPiece (piece, x - 1, y - 1);
					if(piece.isKing == false && x - 1 == 0) {
						piece.isKing = true;
						playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
					}
				}
				changePlayer();
			}
		}
		
		private void moveUpAndRight (int x, int y, GameObject playerPiece) {
			if (canMoveUpAndRight (x, y)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				if(piece.playerNo == 2 || piece.isKing == true) {
					gameBoard.removePiece (x, y);
					gameBoard.AddPlayerPiece (piece, x - 1, y + 1);
					if(piece.isKing == false && x - 1 == 0) {
						piece.isKing = true;
						playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
					}
				}
				changePlayer();
			}
		}
		
		private bool canMoveDownAndRight (int x, int y) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 1 || piece.isKing == true)) {
				if ((y + 1 < 8) && (x + 1 < 8)) {
					if (gameBoard.returnPlayerPiece (x + 1, y + 1) == null) {
						return true;
					} 
				}
			}
			return false;
		}
		
		private bool canMoveDownAndLeft (int x, int y) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 1 || piece.isKing == true)) {
				if ((y - 1 > -1) && (x + 1 < 8)) {
					if (gameBoard.returnPlayerPiece (x + 1, y - 1) == null) {
						return true;
					}
				}
			}
			return false;	
		}
		
		private bool canMoveUpAndRight (int x, int y) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 2 || piece.isKing == true)) {
				if ((y + 1 < 8) && (x - 1 > -1)) {
					if (gameBoard.returnPlayerPiece (x - 1, y + 1) == null) {
						return true;
					}
				}
			}
			return false;
		}
		
		private bool canMoveUpAndLeft (int x, int y)	{
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 2 || piece.isKing == true)) {
				if ((y - 1 > -1) && (x - 1 > -1)) {
					if (gameBoard.returnPlayerPiece (x - 1, y - 1) == null) {
						return true;
					} 
				}
			}
			return false;
		}
		
		private bool canTakeDownAndRight (int x, int y) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 1 || currentPiece.isKing == true)) {
				if ((y + 2 < 8) && (x + 2 < 8)) {
					PlayerPiece enemy = gameBoard.returnPlayerPiece (x + 1, y + 1);
					if (enemy != null) {
						if (enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ())) {
							if (gameBoard.returnPlayerPiece (x + 2, y + 2) == null) {
								return true; 
							}
						} 
					}
				}
			}
			return false;
		}
		
		private bool canTakeDownAndLeft (int x, int y) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 1 || currentPiece.isKing == true)) {
				if ((y - 2 > -1) && (x + 2 < 8)) {
					PlayerPiece enemy = gameBoard.returnPlayerPiece (x + 1, y - 1);
					if (enemy != null) {
						if (enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ())) {
							if (gameBoard.returnPlayerPiece (x + 2, y - 2) == null) {
								return true;
							}
						}
					}
				}
			}
			return false;
		}
		
		private bool canTakeUpAndLeft (int x, int y) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 2 || currentPiece.isKing == true)) {
				if ((y - 2 > - 1) && (x - 2 > - 1)) {
					PlayerPiece enemy = gameBoard.returnPlayerPiece (x - 1, y - 1);
					if (enemy != null) {
						if (enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ())) {
							if (gameBoard.returnPlayerPiece (x - 2, y - 2) == null) {
								return true;
							}
						}
					}			
				}
			} 
			return false;
		}
		
		private bool canTakeUpAndRight (int x, int y) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 2 || currentPiece.isKing == true)) {
				if ((x - 2 > - 1) && (y + 2 < 8)) {
					PlayerPiece enemy = gameBoard.returnPlayerPiece (x - 1, y + 1);
					if (enemy != null) {
						if (enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ())) {
							if (gameBoard.returnPlayerPiece (x - 2, y + 2) == null) {
								return true;
							}
						}
					}
				}
			}
			return false;
		}
		
		private List<PlayerPiece> returnTakeablePieces (int playerNumber) {
			int i = 0, j = 0;
			while (j < 8) {
				while (i < 8) {
					if (canTakeDownAndRight (i, j) || canTakeDownAndLeft (i, j) || canTakeUpAndRight (i, j) || canTakeUpAndLeft (i, j)) {
						if(gameBoard.returnPlayerPiece(i,j).playerNo == playerNumber)
						{
							takeablePieces.Add (gameBoard.returnPlayerPiece (i, j));
						}
					}
					i++;
				}
				j++;
				i = 0;
			}
			return takeablePieces;
		}
		
		private bool playerHasTakeableMoves(int playerNumber) {
			List<PlayerPiece> takeablePieces = returnTakeablePieces (playerNumber);
			return (takeablePieces.Count > 0);
		}
	}
}