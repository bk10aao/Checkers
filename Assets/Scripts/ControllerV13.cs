﻿using System;
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
		private GUIText aiText;

		private int startPosX, startPosZ, aiPieceStartPosX, aiPieceStartPosZ, playerNo = 1, 
					playerOnePieceCount = 12, playerTwoPieceCount = 12, aiType = 0;

		private Boolean pieceChangedToKing = false;

		private Queue<String> moveableAiPieces = new Queue<String>();

		private ArrayList takeablePlayerPieces = new ArrayList();
		private ArrayList moveablePlayerPieces = new ArrayList();
		private ArrayList randomMoves = new ArrayList();

		private GameObject[] playerPieces = new GameObject[12];

		private void Start () {
			gameBoard.SetupPlayerArray ();
			logic = new LogicController ();
			getAIMoves (false);
			aiText  = GameObject.FindWithTag("aiText").GetComponent<GUIText>() as GUIText;
			addPlayerObjects ();
		}

		private void Update () {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (aiType == 0) {
				getOpponentType ();
			} else {
				if (playerNo == 2 && playerTwoPieceCount > 0) {
					System.Threading.Thread.Sleep (500);
					if (logic.playerHasTakeableMoves (2, gameBoard))
						AITake (); 
					else
						aiSearchImplementation ();
				} else {
					getTakeablePlayerPieces ();
					getMoveablePlayerPieces ();
					if (Input.GetMouseButtonDown (0)) {
						if ((Physics.Raycast (ray, out hit)) && hit.collider.tag.Contains ("Player" + playerNo.ToString ()))
							getMoveProperties ();
						else
							interactionPiece = null;
					} else if (Input.GetMouseButtonUp (0) && interactionPiece != null && Physics.Raycast (ray, out hit)) {
						int endPointX = (int)hit.collider.transform.localPosition.x;
						int endPointZ = (int)hit.collider.transform.localPosition.z;
						implementPlayerClickRelease (endPointX, endPointZ);
					}
				}
			}
		}

		private void addPlayerObjects () {
			playerPieces [0] = GameObject.FindGameObjectWithTag ("Player1-1");
			playerPieces [1] = GameObject.FindGameObjectWithTag ("Player1-2");
			playerPieces [2] = GameObject.FindGameObjectWithTag ("Player1-3");
			playerPieces [3] = GameObject.FindGameObjectWithTag ("Player1-4");
			playerPieces [4] = GameObject.FindGameObjectWithTag ("Player1-5");
			playerPieces [5] = GameObject.FindGameObjectWithTag ("Player1-6");
			playerPieces [6] = GameObject.FindGameObjectWithTag ("Player1-7");
			playerPieces [7] = GameObject.FindGameObjectWithTag ("Player1-8");
			playerPieces [8] = GameObject.FindGameObjectWithTag ("Player1-9");
			playerPieces [9] = GameObject.FindGameObjectWithTag ("Player1-10");
			playerPieces [10] = GameObject.FindGameObjectWithTag ("Player1-11");
			playerPieces [11] = GameObject.FindGameObjectWithTag ("Player1-12");
		}

		private void getMoveablePlayerPieces() {
			if (takeablePlayerPieces.Count == 0) {
				for (int i = 0; i < playerPieces.Length; i++) {
					GameObject g = playerPieces [i];
					if (g != null) {
						if (logic.canMove ((int)g.transform.position.x, (int)g.transform.position.z, gameBoard)) {
							(g.GetComponent (typeof(MeshCollider)) as Collider).enabled = true;
							(g.GetComponent (typeof(Collider)) as Collider).enabled = true;
							moveablePlayerPieces.Add (g);
						} else {
							(g.GetComponent (typeof(MeshCollider)) as Collider).enabled = false;
							(g.GetComponent (typeof(Collider)) as Collider).enabled = false;
						}
					}
				}
			}
		}

		private void getTakeablePlayerPieces() {
			for (int i = 0; i < playerPieces.Length; i++) {
				GameObject g = playerPieces [i];
				if (g != null) {
					if (logic.canTake ((int)g.transform.position.x, (int)g.transform.position.z, gameBoard)) {
						(g.GetComponent (typeof(MeshCollider)) as Collider).enabled = true;
						(g.GetComponent (typeof(Collider)) as Collider).enabled = true;
						takeablePlayerPieces.Add (g);
					} else {
						(g.GetComponent (typeof(MeshCollider)) as Collider).enabled = false;
						(g.GetComponent (typeof(Collider)) as Collider).enabled = false;
					}
				}
			}
		}

		private void getOpponentType () {
			if (Input.GetKeyDown ("1")) {
				aiType = 1;
				aiText.enabled = false;
				getMoveablePlayerPieces ();
			} else if (Input.GetKeyDown ("2")) {
				aiType = 2;
				aiText.enabled = false;
				getMoveablePlayerPieces ();
			} else if (Input.GetKeyDown ("3")) {
				aiType = 3;
				aiText.enabled = false;
				getMoveablePlayerPieces ();
			}
		}

		private void getMoveProperties () {
			startPosX = (int)hit.collider.transform.localPosition.x;
			startPosZ = (int)hit.collider.transform.localPosition.z;
			interactionPiece = hit.collider.gameObject;
			implementPlayerClick ();
		}

		private void implementPlayerClick () {
			if (logic.playerHasTakeableMoves (playerNo, gameBoard) && logic.canTake (startPosX, startPosZ, gameBoard)) {
				interactionPiece.transform.position = new Vector3 (startPosX, 1.0f, startPosZ);
			} else if (logic.canMove (startPosX, startPosZ, gameBoard) && !logic.playerHasTakeableMoves (playerNo, gameBoard)) {
				if(moveablePlayerPieces.Contains(interactionPiece)) 
					interactionPiece.transform.position = new Vector3 (startPosX, 1.0f, startPosZ);
			} else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
				interactionPiece = null;
			}
		}

		private void implementPlayerClickRelease (int endPointX, int endPointZ) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (startPosX, startPosZ);
			if (logic.canTake (startPosX, startPosZ, gameBoard)) {
				take (endPointX, endPointZ);
			} else if (logic.canMove (startPosX, startPosZ, gameBoard)) {
				move (endPointX, endPointZ);
			} else {
				setPieceHeightAfterMove (piece, startPosX, startPosZ);
			}
		}

		private void aiSearchImplementation() {
			switch (aiType) {
				case 1: {
					AIQueueMove ();
					break; }
				case 2: {
					AIMove ();
					break; }
				case 3: {
					AIRandomMove ();
					break; } 
				default: {
					break; }
			}
		}

		private void AIRandomMove() {
			getAIMoves(true);
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
							performAITake ();
							hasTaken = true;
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

		private void takeWithAiPiece (RaycastHit hit, int moveToXPos, int moveToZPos) {
			destroyPiece ((int)hit.transform.position.x, (int)hit.transform.position.z, moveToXPos, moveToZPos);
			PlayerPiece piece = gameBoard.returnPlayerPiece ((int)hit.transform.position.x, (int)hit.transform.position.z);
			setPieceHeightAfterMove (piece, (int)hit.transform.position.x, (int)hit.transform.position.z);
		}

		private void performAITake () {
			if (logic.canTakeUpAndRight (aiPieceStartPosX, aiPieceStartPosZ, gameBoard)) {
				takeUpAndRight (aiPieceStartPosX, aiPieceStartPosZ, interactionPiece);
				takeWithAiPiece (hit, -2, 2);
			} else if (logic.canTakeUpAndLeft (aiPieceStartPosX, aiPieceStartPosZ, gameBoard)) {
				takeUpAndLeft (aiPieceStartPosX, aiPieceStartPosZ, interactionPiece);
				takeWithAiPiece (hit, -2, -2);
			} else if (logic.canTakeDownAndRight (aiPieceStartPosX, aiPieceStartPosZ, gameBoard)) {
				takeDownAndRight (aiPieceStartPosX, aiPieceStartPosZ, interactionPiece);
				takeWithAiPiece (hit, 2, 2);
			} else if (logic.canTakeDownAndLeft (aiPieceStartPosX, aiPieceStartPosZ, gameBoard)) {
				takeDownAndLeft (aiPieceStartPosX, aiPieceStartPosZ, interactionPiece);
				takeWithAiPiece (hit, 2, -2);
			}
		}

		private void getAIMoves(bool random) {
			float i = 0, j = 0;
			Ray aiRay = new Ray();
			while (j < 8) {
				while(i < 8) {
					aiRay.origin = new Vector3 (i, 1.0f, j);
					aiRay.direction = new Vector3 (0 , -1.0f, 0);
					if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						int hitPosX = (int)hit.transform.position.x;
						int hitPosZ = (int)hit.transform.position.z;
						if (random) {
							if (logic.canMove (hitPosX, hitPosZ, gameBoard))
								randomMoves.Add (hitPosX.ToString () + "," + hitPosZ.ToString ());
						} else {
							if (!moveableAiPieces.Contains (hitPosX.ToString () + "," + hitPosZ.ToString ())) {
								if (logic.canMove (hitPosX, hitPosZ, gameBoard)) 
									moveableAiPieces.Enqueue (hitPosX.ToString () + "," + hitPosZ.ToString ());
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
			int i = 0, j = 0;
			Ray aiRay = new Ray();
			bool hasMoved = false;
			while (j < 8 && !hasMoved) {
				while(i < 8 && !hasMoved) {
					aiRay.origin = new Vector3 (i, 1.0f, j);
					aiRay.direction = new Vector3 (0 , -1.0f, 0);
					if ((Physics.Raycast (aiRay, out hit) && hit.collider.tag.Contains("Player2"))) {
						int hitPosX = (int)hit.transform.position.x;
						int hitPosZ = (int)hit.transform.position.z;
						if(logic.canMove(hitPosX, hitPosZ, gameBoard)) {
							performAIMove (hitPosX, hitPosZ);
							hasMoved = true;
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
					if (logic.canMove (hitPosX, hitPosZ, gameBoard)) 
						hasMoved = performAIMove (hitPosX, hitPosZ);
				}	
			}
			getAIMoves (false);
		}

		void setPieceHeightAfterMove (PlayerPiece piece, int tempx, int tempz){
			if (piece.isKing)
				interactionPiece.transform.position = new Vector3 (tempx, 0.4f, tempz);
			else
				interactionPiece.transform.position = new Vector3 (tempx, 0.1f, tempz);
		}
			
		private void move (int tempx, int tempz) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (startPosX, startPosZ);
			interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			if (MoveDownAndRightRangeCheck (tempx, tempz) && logic.canMoveDownAndRight (startPosX, startPosZ, gameBoard)) {
				moveDownAndRight(startPosX, startPosZ, interactionPiece);
				setPieceHeightAfterMove (piece, tempx, tempz);
			} else if (moveDownAndLeftRangeCheck (tempx, tempz) && logic.canMoveDownAndLeft (startPosX, startPosZ, gameBoard)) {
				moveDownAndLeft(startPosX, startPosZ, interactionPiece);
				setPieceHeightAfterMove (piece, tempx, tempz);
			} else if (moveUpAndRightRangeCheck (tempx, tempz) && logic.canMoveUpAndRight (startPosX, startPosZ, gameBoard)) {
				moveUpAndRight(startPosX, startPosZ, interactionPiece);
				setPieceHeightAfterMove (piece, tempx, tempz);
			} else if (moveUpAndLeftRangeCheck (tempx, tempz) && logic.canMoveUpAndLeft (startPosX, startPosZ, gameBoard)) {
				moveUpAndLeft(startPosX, startPosZ, interactionPiece);
				setPieceHeightAfterMove (piece, tempx, tempz);
			} else {
				interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
				setPieceHeightAfterMove (piece, startPosX, startPosZ);
			}
		}

		private bool MoveDownAndRightRangeCheck (int tempx, int tempz) {
			return (tempx > startPosX) && moveRightCheck (tempz) && moveDownCheck (tempx);
		}

		private bool moveDownAndLeftRangeCheck (int tempx, int tempz){
			return (tempx > startPosX) && moveLeftCheck (tempz) && moveDownCheck (tempx);
		}

		private bool moveUpAndRightRangeCheck (int tempx, int tempz) {
			return (tempx < startPosX) && moveRightCheck (tempz) && moveUpCheck (tempx);
		}

		private bool moveUpAndLeftRangeCheck (int tempx, int tempz) {
			return (tempx < startPosX) && moveLeftCheck (tempz) && moveUpCheck(tempx);
		}

		private bool moveDownCheck (int tempx) {
			return ((tempx - startPosX) > 0.25) && ((tempx - startPosX) < 1.45);
		}

		private bool moveUpCheck (int tempx) {
			return ((tempx - startPosX) < -0.25) && ((tempx - startPosX) > -1.45);
		}

		private bool moveRightCheck (int tempz) {
			return ((tempz - startPosZ) < 1.5) && ((tempz - startPosZ) > 0.5);
		}

		private bool moveLeftCheck (int tempz) {
			return ((tempz - startPosZ) < -0.5) && ((tempz - startPosZ) > -1.45);
		}

		private void take (float tempx, float tempz) {
			interactionPiece.transform.position = new Vector3 (startPosX, 0.1f, startPosZ);
			if (takeDownAndRightRangeCheck (tempx, tempz) && logic.canTakeDownAndRight (startPosX, startPosZ, gameBoard)) {
				takeDownAndRight (startPosX, startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, 2, 2);
				playerTwoPieceCount--;
			} else if (takeDownAndLeftRangeCheck (tempx, tempz) && logic.canTakeDownAndLeft (startPosX, startPosZ, gameBoard)) {
				takeDownAndLeft (startPosX, startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, 2, -2);
				playerTwoPieceCount--;
			} else if (takeUpAndLeftRangeCheck (tempx, tempz) && logic.canTakeUpAndLeft (startPosX, startPosZ, gameBoard)) {
				takeUpAndLeft (startPosX, startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, -2, -2);
				playerTwoPieceCount--;
			} else if (takeUpAndRightRangeCheck (tempx, tempz) && logic.canTakeUpAndRight (startPosX, startPosZ, gameBoard)) {
				takeUpAndRight (startPosX, startPosZ, interactionPiece);
				destroyPiece(startPosX, startPosZ, -2, 2);
				playerTwoPieceCount--;
			}
			if (playerTwoPieceCount == 0) {
				aiText.text = "Player One Won!!";
				aiText.enabled = true;
			}
		}

		private bool takeDownAndRightRangeCheck (float tempx, float tempz) {
			return (tempx > startPosX) && takeRightCheck (tempz) && takeDownCheck (tempx);
		}

		private bool takeDownAndLeftRangeCheck (float tempx, float tempz) {
			return (tempx > startPosX) && takeLeftCheck (tempz) && takeDownCheck (tempx);
		}

		private bool takeUpAndLeftRangeCheck (float tempx, float tempz) {
			return (tempx < startPosX) && takeLeftCheck (tempz) && takeUpCheck (tempx);
		}

		private bool takeUpAndRightRangeCheck (float tempx, float tempz) {
			return (tempx < startPosX) && takeRightCheck (tempz) && takeUpCheck (tempx);
		}

		private bool takeRightCheck (float tempz) {
			return (tempz - startPosZ > 1.45) && (tempz - startPosZ < 2.88);
		}

		private bool takeLeftCheck (float tempz) {
			return (tempz - startPosZ > -2.88) && ((tempz - startPosZ) < -1.45);
		}

		private bool takeDownCheck (float tempx) {
			return (tempx - startPosX > 1.45) && (tempx - startPosX < 2.88);
		}

		private bool takeUpCheck (float tempx) {
			return (tempx - startPosX < -1.45) && (tempx - startPosX > -2.88);
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

		private void destroyPiece(int aiStartPosX, int aiStartPosZ, int directionX, int directionZ) {
			interactionPiece.transform.position = new Vector3 (aiStartPosX + directionX, 0.1f, aiStartPosZ + directionZ);
			rayRay.origin = new Vector3 (aiStartPosX, 0.1f, aiStartPosZ);
			rayRay.direction = new Vector3 (directionX, 0.1f, directionZ);
			if (Physics.Raycast (rayRay, out hit))
				Destroy (hit.transform.gameObject);
			if (!logic.canTake (aiStartPosX + directionX, aiStartPosZ + directionZ, gameBoard) && pieceChangedToKing == false) 
				changePlayer ();
		}

		private void changePlayer () {
			if(playerNo == 1) {
				for (int i = 0; i < playerPieces.Length; i++) {
					GameObject g = playerPieces [i];
					if(g != null) {
						(g.GetComponent (typeof(MeshCollider)) as Collider).enabled = true;
						(g.GetComponent (typeof(Collider)) as Collider).enabled = true;
						moveablePlayerPieces.Clear ();
						takeablePlayerPieces.Clear ();
						randomMoves.Clear ();
					}
				}
			}
			playerNo = playerNo == 1 ? 2 : 1;
			pieceChangedToKing = false;
		}

		private void transformPieceToKing(PlayerPiece piece, GameObject playerPiece) {
			piece.isKing = true;
			playerPiece.transform.localScale = new Vector3 (1.0f, 0.4f, 1.0f);
			playerPiece.transform.localPosition = new Vector3 (playerPiece.transform.localPosition.x, 0.4f, playerPiece.transform.localPosition.z);
			changePlayer();
			pieceChangedToKing = true;
		}

		private void updateBoardOnTake(int x, int y, int enemyXPos, int enemyYPos, int emptyXPos, int emptyYPos, GameObject playerPiece) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			gameBoard.removePiece (x, y);
			gameBoard.removePiece (x + enemyXPos, y + enemyYPos);
			gameBoard.AddPlayerPiece (piece, x + emptyXPos, y + emptyYPos);
			if (piece.isKing == false && (x + emptyXPos == 0 || x + emptyXPos == 7))
				transformPieceToKing(piece, playerPiece);
		}

		//x values are supplied as either -1 1, -2 or 2. Positive values will move the piece down the board visually negative will move up 
		//z values are supplied as either -1 1, -2 or 2. Positive values will move the piece right visually negative will move left 
		private void takeUpAndRight (int x, int y, GameObject playerPiece) {
			if (logic.canTakeUpAndRight (x, y, gameBoard)) 
				updateBoardOnTake (x, y, -1, 1, -2, 2, playerPiece);
		}

		private void takeUpAndLeft (int x, int y, GameObject playerPiece) {
			if (logic.canTakeUpAndLeft (x, y, gameBoard)) 
				updateBoardOnTake (x, y, -1, -1, -2, -2, playerPiece);
		}

		private void takeDownAndLeft(int x, int y, GameObject playerPiece) {
			if (logic.canTakeDownAndLeft (x, y, gameBoard)) 
				updateBoardOnTake (x, y, 1, -1, 2, -2, playerPiece);
		}

		private void takeDownAndRight (int x, int y, GameObject playerPiece) {
			if (logic.canTakeDownAndRight (x, y, gameBoard)) 
				updateBoardOnTake (x, y, 1, 1, 2, 2, playerPiece);
		}

		//x values are supplied as either -1 or 1. Positive values will move the piece down the board visually negative will move up 
		//z values are supplied as either -1 or 1. Positive values will move the piece rightvisually negative will move left 
		private void updateGameBoardOnMove (int x, int y, int moveToPosX, int moveToPosY, int playerNumber, GameObject playerPiece, PlayerPiece piece) {
			if (piece.playerNo == playerNumber || piece.isKing == true) {
				gameBoard.removePiece (x, y);
				gameBoard.AddPlayerPiece (piece, x + moveToPosX , y + moveToPosY);
				if (piece.isKing == false && (x + moveToPosX == 0 || x + moveToPosX == 7)) 
					transformPieceToKing (piece, playerPiece);
				else
					changePlayer();
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