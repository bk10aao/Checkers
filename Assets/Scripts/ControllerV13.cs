using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Application {
	
	public class ControllerV13 : MonoBehaviour {
		public Board gameBoard = new Board();
		
		private RaycastHit hit;
		private Ray rayRay = new Ray();

		private GameObject interactionPiece;
		
		private float startPosX, startPosZ;
		
		private int playerNo = 1;

		int playerTwoPiecesCount = 11;
		
		private Boolean pieceChangedToKing = false, aiTaken = false;
		
		private LogicController logicController;

		private Queue<String> moveableAiPieces = new Queue<String>();
		Boolean aiMoved = false;
		
		private void Start () {
			gameBoard.SetupPlayerArray ();
			logicController = new LogicController (gameBoard);
			getAIQueueMoves ();
		}

		private void takeWithAiPiece (RaycastHit hit, int moveToXPos, int moveToZPos, int enemyXPos, int enemyZPos ) {
			destroyPiece ((int)hit.transform.position.x, (int)hit.transform.position.z, moveToXPos, moveToZPos);
			hit.collider.gameObject.transform.position = new Vector3 (this.hit.transform.position.x + enemyXPos, 0.1f, this.hit.transform.position.z + enemyZPos );
			aiTaken = true;
		}

		private bool moveWithAiPiece (int x, int z) {
			hit.collider.gameObject.transform.position = (new Vector3 (hit.transform.position.x + x, 0.1f, hit.transform.position.z + z));
			return true;
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
						if (logicController.canTakeUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							takeUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							takeWithAiPiece (hit, -2, 2, -1, 1);
							playerTwoPiecesCount--;
						}
						else if(logicController.canTakeUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							takeUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							takeWithAiPiece (hit, -2, -2, -1, -1);
							playerTwoPiecesCount--;
						}
						else if(logicController.canTakeDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
							takeDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
							takeWithAiPiece (hit, 2, 2, 1, 1);
							playerTwoPiecesCount--;
						}
						else if(logicController.canTakeDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
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
			System.Threading.Thread.Sleep(1000);
			
			Ray aiRay = new Ray();
			while (j < 8) {
				while(i < 8) {
					aiRay.origin = (new Vector3 (i, 1.0f, j));
					aiRay.direction = (new Vector3 (0 , -1.0f, 0));
					if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						if(!moveableAiPieces.Contains(hit.transform.position.x.ToString() + "," + hit.transform.position.z.ToString ())) {
							if (logicController.canMoveUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
								moveableAiPieces.Enqueue(hit.transform.position.x.ToString() + "," + hit.transform.position.z.ToString ());
							}
							else if(logicController.canMoveUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
								moveableAiPieces.Enqueue(hit.transform.position.x.ToString() + "," + hit.transform.position.z.ToString ());
							}
							else if(logicController.canMoveDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
								moveableAiPieces.Enqueue(hit.transform.position.x.ToString() + "," + hit.transform.position.z.ToString ());
							}
							else if(logicController.canMoveDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
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

		private void AIQueueMove() {
			Boolean hasMovedFromQueue = false;

			while(!hasMovedFromQueue) {
				String s = moveableAiPieces.Dequeue ().ToString();

				String[] positions = s.Split(',');

				rayRay.origin = (new Vector3 (Int32.Parse(positions[0]), 1.0f, Int32.Parse(positions[1])));
				rayRay.direction = (new Vector3 (0 , -1.0f, 0));
				if ((Physics.Raycast (rayRay, out hit) && hit.collider.tag.Contains ("Player2"))) {
					if (logicController.canMoveUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
						moveUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
						hasMovedFromQueue = moveWithAiPiece (-1, 1);
					} else if (logicController.canMoveUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
						moveUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
						hasMovedFromQueue = moveWithAiPiece(-1, -1);
					} else if (logicController.canMoveDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
						moveDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);

						hasMovedFromQueue = moveWithAiPiece(1, -1);
					} else if (logicController.canMoveDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
						moveDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
						hasMovedFromQueue = moveWithAiPiece(1, 1);
					}
				}	
			}
		}

		/** 
		 * Comment out above and uncomment below code to implement depth first search
		 * */
		private void AIMove() {
			float i = 0, j = 0;
			System.Threading.Thread.Sleep(1000);
			
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

		Vector3 distance;
		float posX, posY;

		void OnMouseDown() {
			distance = Camera.main.WorldToScreenPoint (transform.position);
			posX = Input.mousePosition.x - distance.x;
			posY = Input.mousePosition.y - distance.y;
		}
		
		void OnMouseDrag() {
			Vector3 curPosistion = new Vector3 (Input.mousePosition.x - posX, Input.mousePosition.y - posY, distance.z);
			
			Vector3 worldPos = Camera.main.ScreenToWorldPoint (curPosistion);
			transform.position = worldPos;
		}

		private void Update () {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (playerNo == 2) {
				if(logicController.playerHasTakeableMoves(2, gameBoard) && 	playerTwoPiecesCount > 01) {
					AITake();
					//comment out bline below when implementing depth first search
					getAIQueueMoves();
				}
				else  {
					//comment out lines below when implementing depth first search
					//AIQueueMove();
					//getAIQueueMoves();
					//uncomment line below to implement deth first search
					AIMove();
				}
			} else {
				//WORKS PICKING UP AND PUTTING DOWN OBJECTS BUT NOT DRAGGING
				if (Input.GetMouseButtonDown (0)) {
					if ((Physics.Raycast (ray, out hit)) && hit.collider.tag.Contains ("Player")) {	
						startPosX = hit.collider.transform.localPosition.x;
						startPosZ = hit.collider.transform.localPosition.z;
						interactionPiece = hit.collider.gameObject;
						
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
					float tempz = hit.collider.transform.localPosition.z;
					float tempx = hit.collider.transform.localPosition.x;
					if (logicController.canTake ((int)startPosX, (int)startPosZ, gameBoard)) {
						take (tempx, tempz);
					} else if (logicController.canMove ((int)startPosX, (int)startPosZ, gameBoard)) {
						move ((int)tempx, (int)tempz);
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
				updateBoardOnMove((int)startPosX, (int)startPosZ, 1, 1, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx > startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && logicController.canMoveDownAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				updateBoardOnMove((int)startPosX, (int)startPosZ, 1, -1, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && ((tempz - startPosZ) < 1.5) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && logicController.canMoveUpAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				updateBoardOnMove((int)startPosX, (int)startPosZ, -1, 1, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && ((tempz - startPosZ) < 0) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && logicController.canMoveUpAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				updateBoardOnMove((int)startPosX, (int)startPosZ, -1, -1, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			}
			else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			}
		}
		
		private void take (float tempx, float tempz)
		{
			interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 2.5)) && logicController.canTakeDownAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeDownAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, 2.0f, 2.0f);
			}
			else if ((tempx > startPosX) && (tempz - startPosZ < -1.5) && ((tempz - startPosZ) > -2.5) && logicController.canTakeDownAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeDownAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, 2.0f, -2.0f);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ < -1.5) && (tempz - startPosZ > -2.5)) && (tempx - startPosX < -1) && logicController.canTakeUpAndLeft ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeUpAndLeft ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, -2.0f, -2.0f);
			}
			else if ((tempx < startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 2.5)) && (tempx - startPosX < -1) && logicController.canTakeUpAndRight ((int)startPosX, (int)startPosZ, gameBoard)) {
				takeUpAndRight ((int)startPosX, (int)startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, -2.0f, 2.0f);
			}
			else {
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
		
		private void transformPieceToKing(PlayerPiece piece, GameObject playerPiece) {
			piece.isKing = true;
			playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
			changePlayer();
			pieceChangedToKing = true;
		}

		private void updateBoardOnTake(int x, int y, int enemyXPos, int enemyYPos, int emptyXPos, int emptyYPos, GameObject playerPiece) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			gameBoard.removePiece (x, y);
			gameBoard.removePiece (x + enemyXPos, y + enemyYPos);
			gameBoard.AddPlayerPiece (piece, x + emptyXPos, y + emptyYPos);
			if (piece.isKing == false && (x + emptyXPos == 0 || x + emptyXPos == 7)) {
				piece.isKing = true;
				transformPieceToKing(piece, playerPiece);
				//playerPiece.transform.localScale = new Vector3 (1.0f, 1.5f, 1.0f);
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
		
		private void updateBoardOnMove (int x, int y, int moveToPosX, int moveToPosY, GameObject playerPiece) {
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
		}
		
		private void moveDownAndRight (int x, int y, GameObject playerPiece) {
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
		}
	}
}