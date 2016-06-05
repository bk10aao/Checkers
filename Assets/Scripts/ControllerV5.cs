using UnityEngine;
using System.Collections.Generic;
using System;

namespace Application {
	
	public class ControllerV5 : MonoBehaviour
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
						interactionPiece.transform.position = new Vector3(startPosX, 0.1f, startPosX);
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

		private void movePiece(int x, int y, int verticalPosition, int endPointZ, int playerNumber, GameObject playerPiece) {
			if (canMove(x,y)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				if(piece.playerNo == playerNumber || piece.isKing == true) {
					gameBoard.removePiece (x, y);
					gameBoard.AddPlayerPiece (piece, x + verticalPosition, endPointZ);
					if((piece.isKing == false && x + verticalPosition == 7 && playerNo == 1) 
					   || (piece.isKing == false && x + verticalPosition == 0 && playerNo == 2)) {
						piece.isKing = true;
						playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
					}
				}
				changePlayer();
			}
		}

		void move (int tempx, int tempz)
		{
			interactionPiece.transform.position = new Vector3(startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && canMoveDownAndRight ((int)startPosX, (int)startPosZ)) {
				movePiece ((int)startPosX, (int)startPosZ, 1, 1, playerNo, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx > startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && canMoveDownAndLeft ((int)startPosX, (int)startPosZ)) {
				movePiece ((int)startPosX, (int)startPosZ, 1, -1, playerNo, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && ((tempz - startPosZ) < 1.5) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && canMoveUpAndRight ((int)startPosX, (int)startPosZ)) {
				movePiece((int)startPosX, (int)startPosZ, -1, 1, playerNo, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && ((tempz - startPosZ) < 0) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && canMoveUpAndLeft ((int)startPosX, (int)startPosZ)) {
				movePiece ((int)startPosX, (int)startPosZ, -1, -1, playerNo, interactionPiece);
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
				takePiece((int)startPosX, (int)startPosZ, 1, 1, 2, 2,  interactionPiece);
				destroyPiece(2.0f, 2.0f);
			}
			else if ((tempx > startPosX) && (tempz - startPosZ < -1.5) && ((tempz - startPosZ) > -2.5) && canTakeDownAndLeft ((int)startPosX, (int)startPosZ)) {
				takePiece((int)startPosX, (int)startPosZ,1, -1, 2, -2, interactionPiece);
				destroyPiece(2.0f, -2.0f);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 3)) && (tempx - startPosX < -1) && canTakeUpAndRight ((int)startPosX, (int)startPosZ)) {
				takePiece((int)startPosX, (int)startPosZ,-1, 1, -2, 2, interactionPiece);
				destroyPiece(-2.0f, 2.0f);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ < -1.5) && (tempz - startPosZ > -3)) && (tempx - startPosX < -1) && canTakeUpAndLeft ((int)startPosX, (int)startPosZ)) {
				takePiece((int)startPosX, (int)startPosZ,-1, -1, -2, -2, interactionPiece);
				destroyPiece(-2.0f, -2.0f);
			}
			else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			}
		}



		private void takePiece(int x, int y, int enemyXPos, int enemyZPos, int emptyXPos, int emptyZPos, GameObject playerPiece) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if (canTake (x, y)) {
				gameBoard.removePiece (x, y);
				gameBoard.removePiece (x + enemyXPos, y + enemyZPos);
				gameBoard.AddPlayerPiece (piece, x + emptyXPos, y + emptyZPos);
				if((piece.isKing == false && x + emptyXPos == 7 && playerNo == 1) 
				   || (piece.isKing == false && x + emptyXPos == 0 && playerNo == 2)) {
					transformPieceToKing(piece, playerPiece);
				}

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
			Debug.Log ("changed Player to: " + playerNo);
			pieceChangedToKing = false;	
		}
		
		private void transformPieceToKing(PlayerPiece piece, GameObject playerPiece) {
			piece.isKing = true;
			playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
			changePlayer();
			pieceChangedToKing = true;
		}

		private bool pieceCanMove(int x, int y, int moveToXPos, int moveToYPos) {

			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 1 || piece.isKing == true)) {
				Debug.Log ("passed first if 1 x: " + x +" y:" + y);
				if ((x - 1 >= -1) && (x + 1 <= 7) && (y - 1 >= -1) && (y + 1 <= 7 )) {
					Debug.Log ("passed second if 1");
					if(gameBoard.returnPlayerPiece(x + moveToXPos, y + moveToYPos) == null) {
						return true;
					}
				}
			} else if ((piece != null) && (piece.playerNo == 2 || piece.isKing == true)) {
				Debug.Log ("passed first if 2 x: " + x +" y:" + y);

				if ((x - 1 >= -1) && (x + 1 <= 7 ) && (y - 1 >= -1) && (y + 1 <= 7)) {
					Debug.Log ("passed second if 2");
					if(gameBoard.returnPlayerPiece(x + moveToXPos,y + moveToYPos) == null) {
						return true;
					}
				}
			} else {

			}

			return false;
		}

		private bool canMoveDown(int x, int y, int movementDirection) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 1 || piece.isKing == true)) {
				if ((y + 1 < 8) && (y - 1 > -1) && (x + 1 < 8)) {
					if (gameBoard.returnPlayerPiece (x +1, y + movementDirection) == null) {
						return true;
					} 
				}
			}
			return false;
		}

		private bool canMoveUp (int x, int y, int movementDirection) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && (piece.playerNo == 2 || piece.isKing == true)) {
				if ((y + 1 < 8) && (y - 1 > -1) && (x - 1 > -1)) {
					if (gameBoard.returnPlayerPiece (x - 1, y + movementDirection) == null) {
						return true;
					}
				}
			}
			return false;
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