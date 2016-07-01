using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Application {
	
	public class ControllerV13 : MonoBehaviour {

		public Board gameBoard = new Board();
		
		private RaycastHit hit, hit2;
		private Ray rayRay = new Ray();
		
		private GameObject interactionPiece;
		
		private float startPosX, startPosZ;
		
		private int playerNo = 1, aiPieceStartPosX, aiPieceStartPosZ, playerTwoPiecesCount = 11;
		
		private Boolean pieceChangedToKing = false, aiTaken = false, aiMoved = false;
		
		private LogicController logicController;
		
		private Queue<String> moveableAiPieces = new Queue<String>();

		//TODO when an opponent piece is transformed to a king it no longer takes

		private void Start () {
			gameBoard.SetupPlayerArray ();
			logicController = new LogicController (gameBoard);
			getAIQueueMoves ();
		}

		private bool moveWithAiPiece (int x, int z) {
			hit.collider.gameObject.transform.position = (new Vector3 (hit.transform.position.x + x, 0.1f, hit.transform.position.z + z));
			if (hit.transform.position.x + x == 0) {
				transformPieceToKing(gameBoard.returnPlayerPiece((int)this.hit.transform.position.x + x, (int)this.hit.transform.position.z + z), hit.collider.gameObject);
			}
			return true;
		}

		private void takeWithAiPiece (RaycastHit hit, int moveToXPos, int moveToZPos, int enemyXPos, int enemyZPos ) {
			destroyPiece ((int)hit.transform.position.x, (int)hit.transform.position.z, moveToXPos, moveToZPos);
			hit.collider.gameObject.transform.position = new Vector3 (this.hit.transform.position.x + enemyXPos, 0.1f, this.hit.transform.position.z + enemyZPos );
			aiTaken = true;

			rayRay.origin = new Vector3 ((float)aiPieceStartPosX + moveToXPos, 1.0f, aiPieceStartPosZ + moveToZPos +(float)moveToZPos); //TODO: HUH?
			rayRay.direction = (new Vector3 (0 , -1.0f, 0));
			Physics.Raycast (rayRay, out hit2);
			if (moveToXPos == 0) {
				transformPieceToKing(gameBoard.returnPlayerPiece(moveToXPos, moveToZPos), hit2.collider.gameObject);
			}
		}
		
		private void AITake() {
			float i = 0, j = 0;
			System.Threading.Thread.Sleep(500);
			
			while (j < 8 && !aiTaken) {
				while(i < 8 && !aiTaken) {
					rayRay.origin = (new Vector3 (i, 1.0f, j));
					rayRay.direction = (new Vector3 (0 , -1.0f, 0));
					if ((Physics.Raycast (rayRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						interactionPiece = hit.collider.gameObject;
						aiPieceStartPosX = (int)hit.transform.position.x;
						aiPieceStartPosZ = (int)hit.transform.position.z;
						if (logicController.canTakeUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							Debug.Log ("can take up and right");
							takeUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							takeWithAiPiece (hit, -2, 2, -1, 1);
							playerTwoPiecesCount--;
						} else if(logicController.canTakeUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							Debug.Log ("can take up and left");
							takeUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							takeWithAiPiece (hit, -2, -2, -1, -1);
							playerTwoPiecesCount--;
						} else if(logicController.canTakeDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							Debug.Log ("can take down and right");
							takeDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							takeWithAiPiece (hit, 2, 2, 1, 1);
							playerTwoPiecesCount--;
						} else if(logicController.canTakeDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							Debug.Log ("can take down and left");
							takeDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							takeWithAiPiece (hit, 2, -2, 1, -1);
							playerTwoPiecesCount--;
						}
					}
					i++;
				}
				j++;
				i = 0;
			}
			aiTaken = false;
		}
		
		private void getAIQueueMoves() {
			float i = 0, j = 0;
			System.Threading.Thread.Sleep(500);
			
			Ray aiRay = new Ray();
			while (j < 8) {
				while(i < 8) {
					aiRay.origin = (new Vector3 (i, 1.0f, j));
					aiRay.direction = (new Vector3 (0 , -1.0f, 0));
					if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						if(!moveableAiPieces.Contains(hit.transform.position.x.ToString() + "," + hit.transform.position.z.ToString ())) {
							if(logicController.canMove((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
								moveableAiPieces.Enqueue(hit.transform.position.x.ToString() + "," + hit.transform.position.z.ToString ());
							}
						}
					}
					i++;
				}
				j++;
				i = 0;
			}
		}

		private void AIMove() {
			float i = 0, j = 0;
			System.Threading.Thread.Sleep(500);
			
			Ray aiRay = new Ray();
			while (j < 8 && !aiMoved) {
				while(i < 8 && !aiMoved) {
					aiRay.origin = (new Vector3 (i, 1.0f, j));
					aiRay.direction = (new Vector3 (0 , -1.0f, 0));
					if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						if (logicController.canMoveUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							moveUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, interactionPiece);
							hit.collider.gameObject.transform.position = new Vector3 (hit.transform.position.x - 1, 0.1f, hit.transform.position.z + 1);
							aiMoved = true;
						}
						else if(logicController.canMoveUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							moveUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							hit.collider.gameObject.transform.position = (new Vector3 ((int)hit.transform.position.x - 1, 0.1f, (int)hit.transform.position.z - 1));
							aiMoved = true;
						}
						else if(logicController.canMoveDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							moveDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							hit.collider.gameObject.transform.position = (new Vector3 ((int)hit.transform.position.x + 1, 0.1f, (int)hit.transform.position.z + 1));
							aiMoved = true;
						}
						else if(logicController.canMoveDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							moveDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							hit.collider.gameObject.transform.position = (new Vector3 ((int)hit.transform.position.x + 1, 0.1f, (int)hit.transform.position.z - 1));
							aiMoved = true;
						}
					}
					i++;
				}
				j++;
				i = 0;
			}
			aiMoved = false;
		}
		
		private void AIQueueMove() {
			Boolean hasMovedFromQueue = false;
			Ray aiRay = new Ray();
			while(!hasMovedFromQueue) {
				String s = moveableAiPieces.Dequeue ().ToString();
				String[] positions = s.Split(',');
				aiRay.origin = (new Vector3 (Int32.Parse(positions[0]), 1.0f, Int32.Parse(positions[1])));
				aiRay.direction = (new Vector3 (0 , -1.0f, 0));
				if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
					if (logicController.canMoveUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
						moveUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, interactionPiece);
						hit.collider.gameObject.transform.position = new Vector3 (hit.transform.position.x - 1, 0.1f, hit.transform.position.z + 1);
						break;
					} else if(logicController.canMoveUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
						moveUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
						hit.collider.gameObject.transform.position = (new Vector3 ((int)hit.transform.position.x - 1, 0.1f, (int)hit.transform.position.z - 1));
						break;
					} else if(logicController.canMoveDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
						moveDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
						hit.collider.gameObject.transform.position = (new Vector3 ((int)hit.transform.position.x + 1, 0.1f, (int)hit.transform.position.z + 1));
						break;
					} else if(logicController.canMoveDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
						moveDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
						hit.collider.gameObject.transform.position = (new Vector3 ((int)hit.transform.position.x + 1, 0.1f, (int)hit.transform.position.z - 1));
						break;
					}
				}	
			}
		}
		
		private void Update () {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (playerNo == 2) {
				if(logicController.playerHasTakeableMoves(2, gameBoard)) {
					AITake();
					//comment out bline below when implementing depth first search
					getAIQueueMoves();
				} else {
					//comment out lines below when implementing depth first search
					AIQueueMove();
					getAIQueueMoves();
					//uncomment line below to implement depth first search
					//AIMove();
				}
			} else {
				if (Input.GetMouseButtonDown (0)) {
					if ((Physics.Raycast (ray, out hit)) && hit.collider.tag.Contains ("Player")) {	
						startPosX = hit.collider.transform.localPosition.x;
						startPosZ = hit.collider.transform.localPosition.z;
						interactionPiece = hit.collider.gameObject;
						//TODO: look into simplifying
						if (logicController.playerHasTakeableMoves (playerNo, gameBoard) && (interactionPiece.tag.Contains ("Player" + playerNo.ToString ()) && canTake ((int)startPosX, (int)startPosZ))) {
							interactionPiece.transform.position = new Vector3 (hit.collider.transform.localPosition.x, 1.0f, hit.collider.transform.localPosition.z);
						} else if (logicController.canMove ((int)startPosX, (int)startPosZ, gameBoard) && (interactionPiece.tag.Contains ("Player" + playerNo.ToString ())) && (!logicController.playerHasTakeableMoves (playerNo, gameBoard))) {
							interactionPiece.transform.position = new Vector3 (hit.collider.transform.localPosition.x, 1.0f, hit.collider.transform.localPosition.z);

						} else {
							interactionPiece = null;
						}
					} else {
						interactionPiece = null;
					}
				}
				if (Input.GetMouseButtonUp (0) && interactionPiece != null && Physics.Raycast (ray, out hit)) {
					if (logicController.canTake ((int)startPosX, (int)startPosZ, gameBoard)) {
						take ((int)hit.collider.transform.localPosition.x, (int)hit.collider.transform.localPosition.z);
					} else if (logicController.canMove ((int)startPosX, (int)startPosZ, gameBoard)) {
						move ((int)hit.collider.transform.localPosition.x, (int)hit.collider.transform.localPosition.z);
					} else {
						interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosX);
					}
				}
			}
		}
		
		private bool canMove(int x, int y) {
			return (logicController.canMoveDownAndLeft (x, y, gameBoard) || logicController.canMoveDownAndRight (x, y, gameBoard) || logicController.canMoveUpAndLeft (x, y, gameBoard) || logicController.canMoveUpAndRight (x, y, gameBoard));
		}
		
		private bool canTake(int x, int y) {
			return (logicController.canTakeUpAndLeft (x, y, gameBoard) || logicController.canTakeUpAndRight (x, y, gameBoard) || logicController.canTakeDownAndLeft (x, y, gameBoard) || logicController.canTakeDownAndRight (x, y, gameBoard));
		}
		
		private void move (int tempx, int tempz) {
			interactionPiece.transform.position = new Vector3(startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && logicController.canMoveDownAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				//updateBoardOnMove((int)startPosX, (int)startPosZ, 1, 1, interactionPiece);
				moveDownAndRight((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			} else if ((tempx > startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && logicController.canMoveDownAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				//updateBoardOnMove((int)startPosX, (int)startPosZ, 1, -1, interactionPiece);
				moveDownAndLeft((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			} else if ((tempx < startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && ((tempz - startPosZ) < 1.5) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && logicController.canMoveUpAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				//updateBoardOnMove((int)startPosX, (int)startPosZ, -1, 1, interactionPiece);
				moveUpAndRight((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			} else if ((tempx < startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && ((tempz - startPosZ) < 0) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && logicController.canMoveUpAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				//updateBoardOnMove((int)startPosX, (int)startPosZ, -1, -1, interactionPiece);
				moveUpAndLeft((int)startPosX, (int)startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			}
		}
		
		private void take (float tempx, float tempz) {
			interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 2.5)) && logicController.canTakeDownAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeDownAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, 2.0f, 2.0f);
			} else if ((tempx > startPosX) && (tempz - startPosZ < -1.5) && ((tempz - startPosZ) > -2.5) && logicController.canTakeDownAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeDownAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, 2.0f, -2.0f);
			} else if ((tempx < startPosX) && ((tempz - startPosZ < -1.5) && (tempz - startPosZ > -2.5)) && (tempx - startPosX < -1) && logicController.canTakeUpAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeUpAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, -2.0f, -2.0f);
			} else if ((tempx < startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 2.5)) && (tempx - startPosX < -1) && logicController.canTakeUpAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeUpAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, -2.0f, 2.0f);
			} else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			}
		}
		
		private void destroyPiece(float aiStartPosX, float aiStartPosZ, float directionX, float directionZ) {
			interactionPiece.transform.position = new Vector3 (aiStartPosX + directionX, 0.1f, aiStartPosZ + directionZ);
			rayRay.origin = new Vector3 (aiStartPosX, 0.1f, aiStartPosZ);
			rayRay.direction = new Vector3 (directionX, 0.1f, directionZ);
			if (Physics.Raycast (rayRay, out hit)) {
				Destroy (hit.transform.gameObject);
			}
			if ((!canTake ((int)aiStartPosX + (int)directionX, (int)aiStartPosZ + (int)directionZ) && pieceChangedToKing == false)
			    && ((((int)aiStartPosX + (int)directionX) > -1) 
			    && (((int)aiStartPosX + (int)directionX) < 8) && (((int)aiStartPosZ + (int)directionZ) > -1) 
			    && (((int)aiStartPosZ + (int)directionZ) < 8))) {
				changePlayer ();
			}
		}
		
		private void changePlayer () { //use lambda?
			if (playerNo == 1) 
				playerNo = 2;
			else 
				playerNo = 1;
			pieceChangedToKing = false;
		}
		
		private void transformPieceToKing(PlayerPiece piece, GameObject playerPiece)  {
			try {
				piece.isKing = true;
				playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
				changePlayer();
				pieceChangedToKing = true;
			} catch (Exception e) {
				Debug.Log (e.StackTrace.ToString ());
			}
		}
		
		private void updateBoardOnTake(int x, int y, int enemyXPos, int enemyYPos, int emptyXPos, int emptyYPos, GameObject playerPiece) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			gameBoard.removePiece (x, y);
			gameBoard.removePiece (x + enemyXPos, y + enemyYPos);
			gameBoard.AddPlayerPiece (piece, x + emptyXPos, y + emptyYPos);
			if (piece.isKing == false && (x + emptyXPos == 0 || x + emptyXPos == 7)) {
				piece.isKing = true;
				transformPieceToKing(piece, playerPiece);
			}
		}
		
		private void takeUpAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canTakeUpAndRight (x, y, gameBoard)) {
				updateBoardOnTake (x, y, -1, 1, -2, 2, playerPiece);
			}
		}
		
		private void takeUpAndLeft (int x, int y, GameObject playerPiece) {
			if (logicController.canTakeUpAndLeft (x, y, gameBoard)) {
				updateBoardOnTake (x, y, -1, -1, -2, -2, playerPiece);
			}
		}
		
		private void takeDownAndLeft(int x, int y, GameObject playerPiece) {
			if (logicController.canTakeDownAndLeft (x, y, gameBoard)) {
				updateBoardOnTake (x, y, 1, -1, 2, -2, playerPiece);
			}
		}
		
		private void takeDownAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canTakeDownAndRight (x, y, gameBoard)) {
				updateBoardOnTake (x, y, 1, 1, 2, 2, playerPiece);
			}
		}

		private void updateGameBoardOnMove (int x, int y, int moveToPosX, int moveToPosY, int playerNumber, GameObject playerPiece, PlayerPiece piece) {
			if (piece.playerNo == playerNumber || piece.isKing == true) {
				gameBoard.removePiece (x, y);
				gameBoard.AddPlayerPiece (piece, x + moveToPosX , y + moveToPosY);
				if (piece.isKing == false && ((x + moveToPosX == 0 && piece.playerNo == 2) || (x + moveToPosX == 7 && piece.playerNo == 1))) {
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
				updateGameBoardOnMove (x, y, -1, 1, piece.playerNo, playerPiece, piece);
				changePlayer ();
			}
		}

		/**private void updateBoardOnMove (int x, int y, int moveToPosX, int moveToPosY, GameObject playerPiece) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if (piece.playerNo == playerNo|| piece.isKing == true) {
				gameBoard.removePiece (x, y);
				gameBoard.AddPlayerPiece (piece, x + moveToPosX , y + moveToPosY);
				if (piece.isKing == false && (x + moveToPosX == 0 || x + moveToPosX == 7)) {
					transformPieceToKing(piece, playerPiece);
				} else {
					changePlayer ();
				}
			}
		}**/

		/**private void moveDownAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canMoveDownAndRight (x, y, gameBoard)) {
				updateBoardOnMove(x, y, 1, 1, playerPiece);
			}
		}
		
		private void moveDownAndLeft (int x, int y, GameObject playerPiece) {
			if (logicController.canMoveDownAndLeft (x, y, gameBoard)) {
				updateBoardOnMove(x, y, 1, -1, playerPiece);
			}
		}
		
		private void moveUpAndLeft (int x, int y, GameObject playerPiece) {
			if (logicController.canMoveUpAndLeft (x, y, gameBoard)) {
				updateBoardOnMove(x, y, -1, -1, playerPiece);
			}
		}
		
		private void moveUpAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canMoveUpAndRight (x, y, gameBoard)) {
				updateBoardOnMove(x, y, -1, 1, playerPiece);
			}
		}**/
	}
}








		/**
 		 * void updateBoardOnTakeUpAndRight (int x, int y, PlayerPiece piece)
		{
			gameBoard.removePiece (x, y);
			gameBoard.removePiece (x - 1, y + 1);
			gameBoard.AddPlayerPiece (piece, x - 2, y + 2);
		}

		void updateBoardOnTakeUpAndLeft (int x, int y, PlayerPiece piece)
		{
			gameBoard.removePiece (x, y);
			gameBoard.removePiece (x - 1, y - 1);
			gameBoard.AddPlayerPiece (piece, x - 2, y - 2);
		}

		void updateBoardOnTakeDownAndLeft (int x, int y, PlayerPiece piece)
		{
			gameBoard.removePiece (x, y);
			gameBoard.removePiece (x + 1, y - 1);
			gameBoard.AddPlayerPiece (piece, x + 2, y - 2);
		}

		void updateBoardOnTakeDownAndRight (int x, int y, PlayerPiece piece)
		{
			gameBoard.removePiece (x, y);
			gameBoard.removePiece (x + 1, y + 1);
			gameBoard.AddPlayerPiece (piece, x + 2, y + 2);
		}

		private void takeUpAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canTakeUpAndRight (x, y, gameBoard)) {;
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateBoardOnTakeUpAndRight (x, y, piece);
				if(piece.isKing == false && x-2 == 0) {
					transformPieceToKing(piece, playerPiece);
				}
			}
		}
		
		private void takeUpAndLeft (int x, int y, GameObject playerPiece) {
			if (logicController.canTakeUpAndLeft (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateBoardOnTakeUpAndLeft (x, y, piece);
				if(piece.isKing == false && x - 2  == 0) {
					transformPieceToKing(piece, playerPiece);
				}
			}
		}
		
		private void takeDownAndLeft(int x, int y, GameObject playerPiece) {
			if (logicController.canTakeDownAndLeft (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece(x, y);
				updateBoardOnTakeDownAndLeft (x, y, piece);
				if(piece.isKing == false && x + 2 == 7) {
					transformPieceToKing(piece, playerPiece);
				}
			}
		}
		
		private void takeDownAndRight (int x, int y, GameObject playerPiece) {
			if (logicController.canTakeDownAndRight (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateBoardOnTakeDownAndRight (x, y, piece);
				if(piece.isKing == false && x + 2 == 7) {
					transformPieceToKing(piece, playerPiece);
				}
			}
		}

		private void updateGameBoardOnMove (int x, int y, int moveToPosX, int moveToPosY, int playerNumber, GameObject playerPiece, PlayerPiece piece) {
			if (piece.playerNo == playerNumber || piece.isKing == true) {
				gameBoard.removePiece (x, y);
				gameBoard.AddPlayerPiece (piece, x + moveToPosX , y + moveToPosY);
				if (piece.isKing == false && (x + moveToPosX == 0 || x + moveToPosX == 7)) {
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
		}*/