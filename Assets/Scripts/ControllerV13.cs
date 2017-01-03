using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application {

	public class ControllerV13 : MonoBehaviour {
		public Board gameBoard = new Board();

		private LogicController logic;

		private RaycastHit hit, hit2;
		private Ray rayRay = new Ray();

		private GameObject interactionPiece;
		GUIText aiText;

		private int startPosX, startPosZ, playerNo = 1, 
					aiPieceStartPosX, aiPieceStartPosZ, 
					playerOnePieceCount = 12, playerTwoPieceCount = 12,
					aiType = 0;

		private Boolean pieceChangedToKing = false;

		private Queue<String> moveableAiPieces = new Queue<String>();
		private ArrayList randomMoves = new ArrayList();

		private void Start () {
			gameBoard.SetupPlayerArray ();
			logic = new LogicController ();
			getAIQueueMoves ();
			aiText  = GameObject.FindWithTag("aiText").GetComponent<GUIText>() as GUIText;
		}

		private void Update () {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (aiType == 0) {
				if (Input.GetKeyDown ("1")) {
					aiType = 1;
					aiText.enabled = false;
				}
				if (Input.GetKeyDown ("2")) {
					aiType = 2;					
					aiText.enabled = false;
				}
				if (Input.GetKeyDown ("3")) {
					aiType = 3;
					aiText.enabled = false;
				}
			} else {
				if (playerNo == 2 && playerTwoPieceCount > 0) {
					System.Threading.Thread.Sleep (500);
					if (aiType == 1) {
						breadthFirstSearch ();
					} 
					if (aiType == 2) {
						depthFirstSearch ();
					} 
					if (aiType == 3) {
						randomOpponent ();
					}
				} else {
					if (Input.GetMouseButtonDown (0)) {
						if ((Physics.Raycast (ray, out hit)) && hit.collider.tag.Contains ("Player" + playerNo.ToString ())) {	
							startPosX = (int)hit.collider.transform.localPosition.x;
							startPosZ = (int)hit.collider.transform.localPosition.z;
							interactionPiece = hit.collider.gameObject;
							implementPlayerClick ();
						} else {
							interactionPiece = null;
						}
					} else if (Input.GetMouseButtonUp (0) && interactionPiece != null && Physics.Raycast (ray, out hit)) {
						int endPointX = (int)hit.collider.transform.localPosition.x;
						int endPointZ = (int)hit.collider.transform.localPosition.z;
						implementPlayerClickRelease (endPointX, endPointZ);
					}
				}
			}
		}

		void implementPlayerClick () {
			if (logic.playerHasTakeableMoves (playerNo, gameBoard) && logic.canTake (startPosX, startPosZ, gameBoard)) {
				interactionPiece.transform.position = new Vector3 (startPosX, 1.0f, startPosZ);
			} else if (logic.canMove (startPosX, startPosZ, gameBoard) && !logic.playerHasTakeableMoves (playerNo, gameBoard)) {
				interactionPiece.transform.position = new Vector3 (startPosX, 1.0f, startPosZ);
			} else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
				interactionPiece = null;
			}
		}

		void implementPlayerClickRelease (int endPointX, int endPointZ) {
			if (logic.canTake (startPosX, startPosZ, gameBoard)) {
				take (endPointX, endPointZ);
			} else if (logic.canMove (startPosX, startPosZ, gameBoard)) {
				move (endPointX, endPointZ);
			} else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosX);
			}
		}

		private void AIRandomMove() {
			Ray aiRay = new Ray();
			System.Random rnd = new System.Random();
			string s = randomMoves[rnd.Next(randomMoves.Count)].ToString();
			String[] positions = s.Split(',');
			aiRay.origin = new Vector3 (Int32.Parse(positions[0]), 1.0f, Int32.Parse(positions[1]));
			aiRay.direction = new Vector3 (0 , -1.0f, 0);
			Physics.Raycast (aiRay, out hit);
			performAIMove (Int32.Parse(positions[0]), Int32.Parse(positions[1]));
		}

		private void AITake() {
			float i = 0, j = 0;
			bool hasTaken = false;
			while (j < 8 && hasTaken == false) {
				while(i < 8 && hasTaken == false) {
					rayRay.origin = new Vector3 (i, 1.0f, j);
					rayRay.direction = new Vector3 (0 , -1.0f, 0);
					if ((Physics.Raycast (rayRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						interactionPiece = hit.collider.gameObject;
						aiPieceStartPosX = (int)hit.transform.position.x;
						aiPieceStartPosZ = (int)hit.transform.position.z;
						if(logic.canTake(aiPieceStartPosX, aiPieceStartPosZ, gameBoard)) {
							hasTaken = performAITake ();
						}
					}
					i++;
				}
				j++;
				i = 0;
			}
			playerOnePieceCount--;
			if (playerOnePieceCount == 0) {
				aiText.text = "AI Opponent won!!";
				aiText.enabled = true;
			}
		}

		void breadthFirstSearch () {
			if (logic.playerHasTakeableMoves (2, gameBoard) && playerTwoPieceCount > -1) {
				AITake ();
			}
		}

		void depthFirstSearch () {
			if (logic.playerHasTakeableMoves (2, gameBoard)) {
				AITake ();
			} else {
				AIMove ();
			}
		}

		void randomOpponent () {
			if (logic.playerHasTakeableMoves (2, gameBoard) && playerTwoPieceCount > -1) {
				AITake ();
			} else if (logic.playerHasTakeableMoves (2, gameBoard) && playerTwoPieceCount > -1) {
				AITake ();
			} else if (playerTwoPieceCount > -1) {
				getAIrandomMoves ();
				AIRandomMove ();
				randomMoves.Clear ();
			}
		}

		private void takeWithAiPiece (RaycastHit hit, int moveToXPos, int moveToZPos, int enemyXPos, int enemyZPos ) {
			destroyPiece ((int)hit.transform.position.x, (int)hit.transform.position.z, moveToXPos, moveToZPos);
			hit.collider.gameObject.transform.position = new Vector3 (this.hit.transform.position.x + enemyXPos, 0.1f, this.hit.transform.position.z + enemyZPos );
			if (moveToXPos == 0) {
				transformPieceToKing(gameBoard.returnPlayerPiece(moveToXPos, moveToZPos), hit.collider.gameObject);
			}
		}

		private bool performAITake () {
			if (logic.canTakeUpAndRight (aiPieceStartPosX, aiPieceStartPosZ, gameBoard)) {
				takeUpAndRight (aiPieceStartPosX, aiPieceStartPosZ, interactionPiece);
				takeWithAiPiece (hit, -2, 2, -1, 1);
			} else if (logic.canTakeUpAndLeft (aiPieceStartPosX, aiPieceStartPosZ, gameBoard)) {
				takeUpAndLeft (aiPieceStartPosX, aiPieceStartPosZ, interactionPiece);
				takeWithAiPiece (hit, -2, -2, -1, -1);
			} else if (logic.canTakeDownAndRight (aiPieceStartPosX, aiPieceStartPosZ, gameBoard)) {
				takeDownAndRight (aiPieceStartPosX, aiPieceStartPosZ, interactionPiece);
				takeWithAiPiece (hit, 2, 2, 1, 1);
			} else if (logic.canTakeDownAndLeft (aiPieceStartPosX, aiPieceStartPosZ, gameBoard)) {
				takeDownAndLeft (aiPieceStartPosX, aiPieceStartPosZ, interactionPiece);
				takeWithAiPiece (hit, 2, -2, 1, -1);
			}

			return true;
		}

		private void getAIQueueMoves() {
			float i = 0, j = 0;
			Ray aiRay = new Ray();
			while (j < 8) {
				while(i < 8) {
					aiRay.origin = new Vector3 (i, 1.0f, j);
					aiRay.direction = new Vector3 (0 , -1.0f, 0);
					if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						int hitPosX = (int)hit.transform.position.x;
						int hitPosZ = (int)hit.transform.position.z;
						if(!moveableAiPieces.Contains(hitPosX.ToString() + "," + hitPosZ.ToString ())) {
							if(logic.canMove(hitPosX, hitPosZ, gameBoard)) {
								moveableAiPieces.Enqueue(hitPosX.ToString() + "," + hitPosZ.ToString ());
							}
						}
					}
					i++;
				}
				j++;
				i = 0;
			}
		}

		private void getAIrandomMoves() {
			float i = 0, j = 0;
			Ray aiRay = new Ray();
			while (j < 8) {
				while(i < 8) {
					aiRay.origin = new Vector3 (i, 1.0f, j);
					aiRay.direction = new Vector3 (0 , -1.0f, 0);
					if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						int hitPosX = (int)hit.transform.position.x;
						int hitPosZ = (int)hit.transform.position.z;
						if(logic.canMove(hitPosX, hitPosZ, gameBoard)) {
							randomMoves.Add (hitPosX.ToString () + "," + hitPosZ.ToString ());
						}
					}
					i++;
				}
				j++;
				i = 0;
			}
		}

		private void AIMove() {
			int i = 0, j = 0;
			Ray aiRay = new Ray();
			bool aiQueueMoved = false;
			while (j < 8 && !aiQueueMoved) {
				while(i < 8 && !aiQueueMoved) {
					aiRay.origin = new Vector3 (i, 1.0f, j);
					aiRay.direction = new Vector3 (0 , -1.0f, 0);
					if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						int hitPosX = (int)hit.transform.position.x;
						int hitPosZ = (int)hit.transform.position.z;
						if(logic.canMove(hitPosX, hitPosZ, gameBoard)) {
							aiQueueMoved = performAIMove (hitPosX, hitPosZ);
						}
					}
					i++;
				}
				j++;
				i = 0;
			}
		}

		private void AIQueueMove() {
			Ray aiRay = new Ray();
			bool hasMoved = false;
			while(hasMoved == false) {
				String s = moveableAiPieces.Dequeue ().ToString();
				String[] positions = s.Split(',');
				aiRay.origin = new Vector3 (Int32.Parse(positions[0]), 1.0f, Int32.Parse(positions[1]));
				aiRay.direction = new Vector3 (0 , -1.0f, 0);
				if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
					int hitPosX = Int32.Parse(positions[0]);
					int hitPosZ = Int32.Parse(positions[1]);
					if (logic.canMove (hitPosX, hitPosZ, gameBoard)) {
						hasMoved = performAIMove (hitPosX, hitPosZ);
					}
				}	
			}
		}
			
		private void move (int tempx, int tempz) {
			interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && logic.canMoveDownAndRight (startPosX, startPosZ, gameBoard)) {
				moveDownAndRight(startPosX, startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			} else if ((tempx > startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && (tempx - startPosX > 0) && (tempx - startPosX < 1.5) && logic.canMoveDownAndLeft (startPosX, startPosZ, gameBoard)) {
				moveDownAndLeft(startPosX, startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			} else if ((tempx < startPosX) && ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0) && (tempz - startPosZ < 1.5) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && logic.canMoveUpAndRight (startPosX, startPosZ, gameBoard)) {
				moveUpAndRight(startPosX, startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			} else if ((tempx < startPosX) && ((tempz - startPosZ) < 0) && ((tempz - startPosZ) > -1.5) && (tempz - startPosZ < 0) && (tempx - startPosX < 0) && (tempx - startPosX > -1.5) && logic.canMoveUpAndLeft (startPosX, startPosZ, gameBoard)) {
				moveUpAndLeft(startPosX, startPosZ, interactionPiece);
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
			} else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			}
		}

		bool performAIMove (int hitPosX, int hitPosZ) {
			if (logic.canMoveUpAndRight (hitPosX, hitPosZ, gameBoard)) {
				moveUpAndRight (hitPosX, hitPosZ, hit.collider.gameObject);
				hit.collider.gameObject.transform.position = new Vector3 (hitPosX - 1, 0.1f, hitPosZ + 1);
			} else if (logic.canMoveUpAndLeft (hitPosX, hitPosZ, gameBoard)) {
				moveUpAndLeft (hitPosX, hitPosZ, hit.collider.gameObject);
				hit.collider.gameObject.transform.position = new Vector3 (hitPosX - 1, 0.1f, hitPosZ - 1);
			} else if (logic.canMoveDownAndRight (hitPosX, hitPosZ, gameBoard)) {
				moveDownAndRight (hitPosX, hitPosZ, hit.collider.gameObject);
				hit.collider.gameObject.transform.position = new Vector3 (hitPosX + 1, 0.1f, hitPosZ + 1);
			} else if (logic.canMoveDownAndLeft (hitPosX, hitPosZ, gameBoard)) {
				moveDownAndLeft (hitPosX, hitPosZ, hit.collider.gameObject);
				hit.collider.gameObject.transform.position = new Vector3 (hitPosX + 1, 0.1f, hitPosZ - 1);
			} 

			return true;
		}

		private void take (float tempx, float tempz) {
			interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			if ((tempx > startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 2.5)) && logic.canTakeDownAndRight (startPosX, startPosZ, gameBoard)) {
				takeDownAndRight (startPosX, startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, 2, 2);
				playerTwoPieceCount--;
			} else if ((tempx > startPosX) && (tempz - startPosZ < -1.5) && ((tempz - startPosZ) > -2.5) && logic.canTakeDownAndLeft (startPosX, startPosZ, gameBoard)) {
				takeDownAndLeft (startPosX, startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, 2, -2);
				playerTwoPieceCount--;
			} else if ((tempx < startPosX) && ((tempz - startPosZ < -1.5) && (tempz - startPosZ > -2.5)) && (tempx - startPosX < -1) && logic.canTakeUpAndLeft (startPosX, startPosZ, gameBoard)) {
				takeUpAndLeft (startPosX, startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, -2, -2);
				playerTwoPieceCount--;
			} else if ((tempx < startPosX) && ((tempz - startPosZ > 1.5) && (tempz - startPosZ < 2.5)) && (tempx - startPosX < -1) && logic.canTakeUpAndRight (startPosX, startPosZ, gameBoard)) {
				takeUpAndRight (startPosX, startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, -2, 2);
				playerTwoPieceCount--;
			}
			if (playerTwoPieceCount == 0) {
				aiText.text = "Player One Won!!";
				aiText.enabled = true;
			}
		}

		private void destroyPiece(int aiStartPosX, int aiStartPosZ, int directionX, int directionZ) {
			interactionPiece.transform.position = new Vector3 (aiStartPosX + directionX, 0.1f, aiStartPosZ + directionZ);
			rayRay.origin = new Vector3 (aiStartPosX, 0.1f, aiStartPosZ);
			rayRay.direction = new Vector3 (directionX, 0.1f, directionZ);
			if (Physics.Raycast (rayRay, out hit)) {
				Destroy (hit.transform.gameObject);
			}
			if (!logic.canTake (aiStartPosX + directionX, aiStartPosZ + directionZ, gameBoard) && pieceChangedToKing == false) {
				changePlayer ();
			}
		}

		private void changePlayer () {
			playerNo = playerNo == 1 ? 2 : 1;
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
				transformPieceToKing(piece, playerPiece);
			}
		}

		//x values are supplied as either -1 1, -2 or 2. Positive values will move the piece down the board visually negative will move up 
		//z values are supplied as either -1 1, -2 or 2. Positive values will move the piece right visually negative will move left 
		private void takeUpAndRight (int x, int y, GameObject playerPiece) {
			if (logic.canTakeUpAndRight (x, y, gameBoard)) {
				updateBoardOnTake (x, y, -1, 1, -2, 2, playerPiece);
			}
		}

		private void takeUpAndLeft (int x, int y, GameObject playerPiece) {
			if (logic.canTakeUpAndLeft (x, y, gameBoard)) {
				updateBoardOnTake (x, y, -1, -1, -2, -2, playerPiece);
			}
		}

		private void takeDownAndLeft(int x, int y, GameObject playerPiece) {
			if (logic.canTakeDownAndLeft (x, y, gameBoard)) {
				updateBoardOnTake (x, y, 1, -1, 2, -2, playerPiece);
			}
		}

		private void takeDownAndRight (int x, int y, GameObject playerPiece) {
			if (logic.canTakeDownAndRight (x, y, gameBoard)) {
				updateBoardOnTake (x, y, 1, 1, 2, 2, playerPiece);
			}
		}

		//x values are supplied as either -1 or 1. Positive values will move the piece down the board visually negative will move up 
		//z values are supplied as either -1 or 1. Positive values will move the piece rightvisually negative will move left 
		private void updateGameBoardOnMove (int x, int y, int moveToPosX, int moveToPosY, int playerNumber, GameObject playerPiece, PlayerPiece piece) {
			if (piece.playerNo == playerNumber || piece.isKing == true) {
				gameBoard.removePiece (x, y);
				gameBoard.AddPlayerPiece (piece, x + moveToPosX , y + moveToPosY);
				if (piece.isKing == false && (x + moveToPosX == 0 || x + moveToPosX == 7)) {
					transformPieceToKing (piece, playerPiece);
				} else {
					changePlayer();
				}
			}
		}

		private void moveDownAndRight (int x, int y, GameObject playerPiece) {
			if (logic.canMoveDownAndRight (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateGameBoardOnMove(x, y, 1, 1, piece.playerNo, playerPiece, piece);
			}
		}

		private void moveDownAndLeft (int x, int y, GameObject playerPiece) {
			if (logic.canMoveDownAndLeft (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateGameBoardOnMove(x, y, 1, -1, piece.playerNo, playerPiece, piece);
			}
		}

		private void moveUpAndRight (int x, int y, GameObject playerPiece) {
			if (logic.canMoveUpAndRight (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateGameBoardOnMove (x, y, -1, 1, piece.playerNo, playerPiece, piece);
			}
		}

		private void moveUpAndLeft (int x, int y, GameObject playerPiece) {
			if (logic.canMoveUpAndLeft (x, y, gameBoard)) {
				PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
				updateGameBoardOnMove(x, y, -1, -1, piece.playerNo, playerPiece, piece);
			}
		}
	}
}