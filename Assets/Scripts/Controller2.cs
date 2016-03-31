using UnityEngine;
using System.Collections.Generic;
using System;

namespace Application
{
	public class Controller2 : MonoBehaviour {

		public Board gameBoard = new Board();
		public int[,] testArr;

		void Start () {
			gameBoard.SetupPlayerArray ();
		}
		
		void Update () {
			
		}

		public int getOpponent (int playerNo)
		{
			if (playerNo == 1) {
				return 2;
			} else {
				return 1;
			}
		}

		public bool canMoveDownAndRight(int x, int y) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && 
			    (piece.playerNo == 1 || piece.isKing == true)) {
				if ((y + 1 < 8) && (x + 1 < 8)) {
					if (gameBoard.returnPlayerPiece(x + 1, y + 1) == null) {
						return true;
					} 
				}
			}
			return false;
		}

		public bool canMoveDownAndLeft(int x, int y) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) &&
			    (piece.playerNo == 1 || piece.isKing == true)){
				if ((y - 1 > -1) && (x + 1 < 8)) {
					if (gameBoard.returnPlayerPiece (x + 1, y - 1) == null) {
						return true;
					}
				}
			}
			return false;	
		}

		public bool canMoveUpAndRight(int x, int y) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && 
			    (piece.playerNo == 2 || piece.isKing == true)){
				if ((y + 1 < 8) && (x - 1 > -1)) {
					if (gameBoard.returnPlayerPiece (x - 1, y + 1) == null) {
						return true;
					}
				}
			}
			return false;
		}

		public bool canMoveUpAndLeft(int x, int y) {
			PlayerPiece piece = gameBoard.returnPlayerPiece (x, y);
			if ((piece != null) && 
				(piece.playerNo == 2 || piece.isKing == true)){
				if ((y - 1 > -1) && (x - 1 > -1)) {
					if (gameBoard.returnPlayerPiece(x - 1, y - 1) == null) {
						return true;
					} 
				}
			}
			return false;
		}

		public bool canTakeDownAndRight(int x, int y) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 1 || currentPiece.isKing == true)) 
			{
				if ((y + 2 < 8) && (x + 2 < 8)) 
				{
					PlayerPiece enemy = gameBoard.returnPlayerPiece (x + 1, y + 1);
					if (enemy != null) 
					{
						if (enemy.GetPlayerNumber () == getOpponent (currentPiece.GetPlayerNumber ())) 
						{
							if (gameBoard.returnPlayerPiece (x + 2, y + 2) == null) 
							{
								return true; 
							}
						} 
					}
				}
			}
			return false;
		}

		public bool canTakeDownAndLeft(int x, int y) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 1 || currentPiece.isKing == true)) 
			{
				if ((y - 2 > -1) && (x + 2 < 8))
				{
					PlayerPiece enemy = gameBoard.returnPlayerPiece(x + 1, y - 1);
					if(enemy != null)
					{
						if(enemy.GetPlayerNumber() == getOpponent(currentPiece.GetPlayerNumber()))
						{
							if(gameBoard.returnPlayerPiece(x + 2, y - 2) == null)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool canTakeUpAndLeft(int x, int y) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if(currentPiece != null && (currentPiece.playerNo == 2 || currentPiece.isKing == true))
			{	
				if((x - 2 > - 1) && (y - 2 > - 1))
				{
					PlayerPiece enemy = gameBoard.returnPlayerPiece(x - 1, y - 1);
					if (enemy != null)
					{
						if (enemy.GetPlayerNumber() == getOpponent(currentPiece.GetPlayerNumber())) 
						{
							if(gameBoard.returnPlayerPiece(x - 2, y - 2) == null) 
							{
									return true;
							}
						}
					}			
				}
			} 
			return false;
		}

		public bool canTakeUpAndRight(int x, int y) {
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			if (currentPiece != null && (currentPiece.playerNo == 2 || currentPiece.isKing == true)) 
			{
				if((x - 2 > - 1) && (y + 2 < 8))
				{
					PlayerPiece enemy = gameBoard.returnPlayerPiece(x - 1, y + 1);
					if (enemy != null) 
					{
						if (enemy.GetPlayerNumber() == getOpponent(currentPiece.GetPlayerNumber())) 
						{
							if(gameBoard.returnPlayerPiece(x - 2, y + 2) == null) 
							{
									return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool canTakeDownAndBothWays(int x, int y) {
			if (canTakeDownAndRight (x, y) && canTakeDownAndLeft (x, y))
				return true;
			else
				return false;
		}

		public bool canTakeUpAndBothWays(int x, int y) {
			if (canTakeUpAndRight (x, y) && canTakeUpAndLeft (x, y))
				return true;
			else
				return false;
		}

		public bool canTakeBothWaysRight(int x, int y) {
			if (canTakeUpAndRight (x, y) && canTakeDownAndRight (x, y))
				return true;
			else
				return false;
		}

		public bool canTakeBothWaysLeft(int x, int y) {
			if (canTakeUpAndLeft (x, y) && canTakeDownAndLeft (x, y))
				return true;
			else
				return false;
		}

		public bool canTakeUpAndLeftAndDownAndRight(int x, int y) {
			if (canTakeUpAndLeft (x, y) && canTakeDownAndRight (x, y))
				return true;
			else
				return false;
		}
		
		public bool canTakeUpRightAndDownAndLeft(int x, int y) {
			if (canTakeUpAndRight (x, y) && canTakeDownAndLeft (x, y))
				return true;
			else
				return false;
		}

		public bool canTakeBothWaysRightAndUpAndLeft(int x, int y) {
			if (canTakeBothWaysRight(x, y) && canTakeUpAndLeft(x, y))
				return true;
			else
				return false;
		}
		
		public bool canTakeBothWaysRightAndDownAndLeft(int x, int y) {
			if (canTakeBothWaysRight(x, y) && canTakeDownAndLeft(x, y))
				return true;
			else
				return false;
		}

		public bool canTakeBothWaysUpAndDownAndLeft(int x, int y) {
			if (canTakeUpAndBothWays (x, y) && canTakeDownAndLeft (x, y))
				return true;
			else
				return false;
		}
		
		public bool canTakeBothWaysUpAndDownAndRight(int x, int y) {
			if (canTakeUpAndBothWays (x, y) && canTakeDownAndRight (x, y))
				return true;
			else
				return false;
		}

		public bool canTakeBothWaysDownAndUpAndLeft(int x, int y) {
			if (canTakeDownAndBothWays (x, y) && canTakeUpAndLeft (x, y))
				return true;
			else
				return false;
		}
		
		public bool canTakeBothWaysDownAndUpAndRight(int x, int y) {
			if (canTakeDownAndBothWays (x, y) && canTakeUpAndRight (x, y))
				return true;
			else
				return false;
		}

		public bool canTakeBothWaysLeftAndUpAndRight(int x, int y) {
			if(canTakeBothWaysLeft(x, y) && canTakeUpAndRight(x, y))
				return true;
			else
				return false;
		}

		public bool canTakeBothWaysLeftAndDownAndRight(int x, int y) {
			if(canTakeBothWaysLeft(x, y) && canTakeDownAndRight(x, y))
				return true;
			else
				return false;
		}

		public bool canTakeInAllDirections(int x, int y) {
			if (canTakeUpAndBothWays (x, y) && canTakeDownAndBothWays (x, y))
				return true;
			else
				return false;
		}

		private List<string> returnMoveablePiecesOne() {   
			List<String> test = new List<String> ();
			int i = 0, j = 0;
			while(j < 8) {
				while(i < 8) {
					if(canMoveDownAndRight(i,j))
						test.Add("{" + i.ToString() + "," + j.ToString() + "} can move down and right");
					if(canMoveDownAndLeft(i,j))
						test.Add("{" + i.ToString() + "," + j.ToString() + "} can move down and left! ");
					if(canMoveUpAndRight(i,j))
						test.Add("{" + i.ToString() + "," + j.ToString() + "} can move Up and right");
					if(canMoveUpAndLeft(i,j))
						test.Add("{" + i.ToString() + "," + j.ToString() + "} can move Up and left! ");
					
					i++;
				}
				j++;
				i = 0;
			}
			return test;
		}

		public List<PlayerPiece> moveablePiecesPlayer1() {
			List<PlayerPiece> moveablePieces = new List<PlayerPiece>();
			int i = 0, j = 0;
			while(j < 8) {
				while(i < 8) {
					if(canMoveDownAndRight(i,j))
						moveablePieces.Add(gameBoard.returnPlayerPiece (i, j));
					if(canMoveDownAndLeft(i,j))
						moveablePieces.Add(gameBoard.returnPlayerPiece (i, j));
					
					i++;
				}
				j++;
				i = 0;
			}
			return moveablePieces;
		}

		public List<PlayerPiece> moveablePiecesPlayer2() {
			List<PlayerPiece> moveablePieces = new List<PlayerPiece>();
			int i = 0, j = 0;
			while(j < 8) {
				while(i < 8) {
					if(canMoveUpAndRight(i,j))
						moveablePieces.Add(gameBoard.returnPlayerPiece (i, j));
					if(canMoveUpAndLeft(i,j))
						moveablePieces.Add(gameBoard.returnPlayerPiece (i, j));
					
					i++;
				}
				j++;
				i = 0;
			}
			return moveablePieces;
		}

		public List<string> returnMoveablePiecesTwo() {   
			List<String> test = new List<String> ();
			int i = 0, j = 0;
			while(j < 8) {
				while(i < 8) {
					if(canMoveUpAndRight(i,j))
						test.Add("{" + i.ToString() + "," + j.ToString() + "} can move up and right");
					if(canMoveUpAndLeft(i,j))
						test.Add("{" + i.ToString() + "," + j.ToString() + "} can move up and left! ");
					i++;
				}
				j++;
				i = 0;
			}
			return test;
		}

		public List<String> returnTakeablePieces() {
			List<String> test = new List<String> ();
			int i = 0, j = 0;
			while(j < 8) {
				while(i < 8) {
					if(canTakeDownAndRight(i,j))
						test.Add("{" + j.ToString() + "," + i.ToString() + "} can take down + right! ");
					if(canTakeDownAndLeft(i,j))
						test.Add("{" + j.ToString() + "," + i.ToString() + "} can take down + left! ");
					if(canTakeUpAndRight(i,j))
						test.Add("{" + j.ToString() + "," + i.ToString() + "} can take up + right! ");
					if(canTakeUpAndLeft(i,j))
						test.Add("{" + j.ToString() + "," + i.ToString() + "} can take up + left! ");
					i++;
				}
				j++;
				i = 0;
			}
			return test;
		}
		
		public List<String> moveOrTake() {	
			List<String> temp = new List<String> ();
			temp = returnTakeablePieces ();
			if (temp.Count > 0) {
				printList("Takable Pieces: ", temp);
				return temp;
			} else {
				temp = returnMoveablePiecesOne ();
				printList("Movable pieces: ", temp);
				return temp;
			}
		}

		public void printList(String s, List<String> printList) {
			Debug.Log (s);
			foreach (String o in printList){
				Debug.Log(" " + o);
			}
		}

		public void printList2(List<PlayerPiece> printList) {
			foreach (PlayerPiece o in printList){
				Debug.Log(" " + o);
			}
		}

		public void runTests()
		{
			Debug.Log ("");
			Debug.Log ("Testing if pieces can move down and right; expecting 3 false and 3 true");
			Debug.Log ("Testing if a piece can move down and right (expecting false): " + canMoveDownAndRight (0, 0));
			Debug.Log ("Testing if a piece can move down and right (expecting false): " + canMoveDownAndRight (0, 1));
			Debug.Log ("Testing if a piece can move down and right (expecting false): " + canMoveDownAndRight (1, 7));
			Debug.Log ("Testing if a piece can move down and right (expecting true): " + canMoveDownAndRight (2, 0));
			Debug.Log ("Testing if a piece can move down and right (expecting true): " + canMoveDownAndRight (2, 2));
			Debug.Log ("Testing if a piece can move down and right (expecting true): " + canMoveDownAndRight (2, 4));

			Debug.Log ("");
			Debug.Log ("Testing if pieces can move down and left; expecting 3 false and 3 true");
			Debug.Log ("Testing if a piece can move down and left (expecting false): " + canMoveDownAndLeft (0, 0));
			Debug.Log ("Testing if a piece can move down and left (expecting false): " + canMoveDownAndLeft (0, 1));
			Debug.Log ("Testing if a piece can move down and left (expecting false): " + canMoveDownAndLeft (2, 0));
			Debug.Log ("Testing if a piece can move down and left (expecting true): " + canMoveDownAndLeft (2, 2));
			Debug.Log ("Testing if a piece can move down and left (expecting true): " + canMoveDownAndLeft (2, 4));
			Debug.Log ("Testing if a piece can move down and left (expecting true): " + canMoveDownAndLeft (2, 6));

			Debug.Log ("");
			Debug.Log ("Testing if pieces can move up and right; expecting 3 false and 3 true");
			Debug.Log ("Testing if a piece can move up and right (expecting false): " + canMoveUpAndRight (7, 1));
			Debug.Log ("Testing if a piece can move up and right (expecting false): " + canMoveUpAndRight (7, 0));
			Debug.Log ("Testing if a piece can move up and right (expecting false): " + canMoveUpAndRight (5, 7));
			Debug.Log ("Testing if a piece can move up and right (expecting true): " + canMoveUpAndRight (5, 1));
			Debug.Log ("Testing if a piece can move up and right (expecting true): " + canMoveUpAndRight (5, 3));
			Debug.Log ("Testing if a piece can move up and right (expecting true): " + canMoveUpAndRight (5, 5));

			Debug.Log ("");
			Debug.Log ("Testing if pieces can move up and left; expecting 3 false and 3 true");
			Debug.Log ("Testing if a piece can move up and left (expecting false): " + canMoveUpAndLeft (7, 1));
			Debug.Log ("Testing if a piece can move up and left (expecting false): " + canMoveUpAndLeft (7, 0));
			Debug.Log ("Testing if a piece can move up and left (expecting false): " + canMoveUpAndLeft (6, 0));
			Debug.Log ("Testing if a piece can move up and left (expecting true): " + canMoveUpAndLeft (5, 1));
			Debug.Log ("Testing if a piece can move up and left (expecting true): " + canMoveUpAndLeft (5, 3));
			Debug.Log ("Testing if a piece can move up and left (expecting true): " + canMoveUpAndLeft (5, 5));

			Debug.Log ("");
			Debug.Log ("Testing if a piece can take down and right. Adding appropriate enemy pieces");
			PlayerPiece temp = new PlayerPiece (2);
			gameBoard.boardPieces[3, 1] = temp;
			
			Debug.Log ("Test if a player piece can take down and right (expecting true): " + canTakeDownAndRight(2,0));
			Debug.Log ("Test if a player piece can take down and right (expecting false): " + canTakeDownAndRight(2,1));
			Debug.Log ("Test if a player piece can take down and right (expecting false ): " + canTakeDownAndRight(2,2));

			PlayerPiece temp2 = new PlayerPiece(2);
			temp2.SetKing ();
			gameBoard.boardPieces [3, 1] = temp2;
			gameBoard.boardPieces [4, 2] = new PlayerPiece (1);
			gameBoard.boardPieces [5, 3] = null;
			Debug.Log ("Testing if player two can take down and right AND IS KING (expecting true): " + canTakeDownAndRight(3, 1));
			gameBoard.boardPieces[5,3] = new PlayerPiece (2);
			gameBoard.boardPieces [6, 4] = null;
			Debug.Log ("Testing if player two can take down and right AND IS KING (expecting false): " + canTakeDownAndRight(3, 1));

			Debug.Log ("");
			Debug.Log ("Testing if player one can take down and left (expecting true): " + canTakeDownAndLeft(2, 2));
			Debug.Log ("");
			Debug.Log ("Testing if player two can take down and left AND IS KING (expecting true): " + canTakeDownAndLeft(2, 2));
			gameBoard.boardPieces [2, 2].playerNo = 2;
			Debug.Log ("Changing preivous piece to player 2 then running can take down and left (expecting false): " + canTakeDownAndLeft(2,2));

			Debug.Log ("");
			Debug.Log ("resetting board");
			gameBoard = null;
			gameBoard = new Board();
			gameBoard.SetupPlayerArray ();
			gameBoard.printBoard ();

			Debug.Log ("");
			Debug.Log ("Testing taking moves on pieces that take up and left");
			gameBoard.boardPieces [4, 2] = new PlayerPiece (1);
			Debug.Log ("Testing if player piece two can take up and left (expecting true): " + canTakeUpAndLeft(5,3));
			gameBoard.boardPieces [3, 1] = new PlayerPiece (1);
			Debug.Log ("Testing if player piece two can take up and left (expecting false): " + canTakeUpAndLeft(5,3));
			gameBoard.boardPieces [3, 1] = null;
			gameBoard.boardPieces [4, 2] = null;
			gameBoard.boardPieces [4, 2] = new PlayerPiece (2);
			Debug.Log ("Testing if player piece two can take up and left with player number the same (expecting false): " + canTakeUpAndLeft(5,3));

			Debug.Log ("");
			gameBoard = null;
			gameBoard = new Board();
			gameBoard.SetupPlayerArray ();
			gameBoard.printBoard ();
			Debug.Log ("Testing taking moves on pieces that take up and right");
			gameBoard.boardPieces [4, 2] = null;
			gameBoard.boardPieces [4, 2] = new PlayerPiece (1);
			Debug.Log ("Testing if player piece two can take up and left (expecting true): " + canTakeUpAndRight(5,1));
			gameBoard.boardPieces [3, 3] = new PlayerPiece (1);
			Debug.Log ("Testing if player piece two can take up and left (expecting false): " + canTakeUpAndRight(5,1));
			gameBoard.boardPieces [4, 2].playerNo = 2;
			gameBoard.boardPieces [3, 3] = null;
			Debug.Log ("Testing if player piece two can take up and left where sorrunding is an team member (expecting false): " + canTakeUpAndRight(5,1));

			Debug.Log ("");
			Debug.Log ("Setting up new player board and testing if standard pieces can't take in the wrong direction and king pieces can take in appropriate directions"); 
			gameBoard = null;
			gameBoard = new Board ();

			gameBoard.boardPieces [3, 2] = new PlayerPiece (2);
			gameBoard.boardPieces [5, 2] = new PlayerPiece (2);
			gameBoard.boardPieces [3, 4] = new PlayerPiece (2);
			gameBoard.boardPieces [5, 4] = new PlayerPiece (2);
			gameBoard.boardPieces [4, 3] = new PlayerPiece (1);

			Debug.Log ("Testing if player 1 can take down and right (expecting true): " + canTakeDownAndRight (4, 3));
			Debug.Log ("Testing if player 1 can take down and left (expecting true): " + canTakeDownAndLeft (4, 3));
			Debug.Log ("Testing if player 1 can take up and right (expecting false): " + canTakeUpAndRight (4, 3));
			Debug.Log ("Testing if player 1 can take up and left (expecting false): " + canTakeUpAndLeft (4, 3));

			Debug.Log ("");
			Debug.Log ("Changing to Player 2 and performing same checks in reverse");
			gameBoard = null;
			gameBoard = new Board ();
			
			gameBoard.boardPieces [3, 2] = new PlayerPiece (1);
			gameBoard.boardPieces [5, 2] = new PlayerPiece (1);
			gameBoard.boardPieces [3, 4] = new PlayerPiece (1);
			gameBoard.boardPieces [5, 4] = new PlayerPiece (1);
			gameBoard.boardPieces [4, 3] = new PlayerPiece (2);

			Debug.Log ("Testing if player 2 can take down and right (expecting false): " + canTakeDownAndRight (4, 3));
			Debug.Log ("Testing if player 2 can take down and left (expecting false): " + canTakeDownAndLeft (4, 3));
			Debug.Log ("Testing if player 2 can take up and right (expecting true): " + canTakeUpAndRight (4, 3));
			Debug.Log ("Testing if player 2 can take up and left (expecting true): " + canTakeUpAndLeft (4, 3));

			Debug.Log ("");
			Debug.Log ("Setting up new player board and testing if King pieces can take in all directions for player 1"); 
			gameBoard = null;
			gameBoard = new Board ();
			
			gameBoard.boardPieces [3, 2] = new PlayerPiece (2);
			gameBoard.boardPieces [5, 2] = new PlayerPiece (2);
			gameBoard.boardPieces [3, 4] = new PlayerPiece (2);
			gameBoard.boardPieces [5, 4] = new PlayerPiece (2);
			gameBoard.boardPieces [4, 3] = new PlayerPiece (1);
			gameBoard.boardPieces [4, 3].isKing = true;

			Debug.Log ("Testing if player 1 can take down and right (expecting true): " + canTakeDownAndRight (4, 3));
			Debug.Log ("Testing if player 1 can take down and left (expecting true): " + canTakeDownAndLeft (4, 3));
			Debug.Log ("Testing if player 1 can take up and right (expecting true): " + canTakeUpAndRight (4, 3));
			Debug.Log ("Testing if player 1 can take up and left (expecting true): " + canTakeUpAndLeft (4, 3));

			Debug.Log ("Testing multiple take moves for King without surrounding pieces stopping takes");
			Debug.Log ("Testing if player 1 can take down both ways (expecting true): " + canTakeDownAndBothWays (4, 3));
			Debug.Log ("Testing if player 1 can take left both ways (expecting true): " + canTakeBothWaysLeft (4, 3));
			Debug.Log ("Testing if player 1 can take right both ways (expecting true): " + canTakeBothWaysRight (4, 3));
			Debug.Log ("Testing if player 1 can take Up both ways (expecting true): " + canTakeUpAndBothWays (4, 3)); 

			Debug.Log ("Testing if player 1 can take Up and right and down and left (expecting true): " + canTakeUpRightAndDownAndLeft (4, 3));
			Debug.Log ("Testing if player 1 can take Up and left and down and right (expecting true): " + canTakeUpAndLeftAndDownAndRight (4, 3));

			Debug.Log ("Testing if player 1 can take both ways up and down and left (expecting true): " + canTakeBothWaysUpAndDownAndLeft(4,3));
			Debug.Log ("Testing if player 1 can take both ways up and down and right (expecting true): " + canTakeBothWaysUpAndDownAndRight(4,3));
			Debug.Log ("Testing if player 1 can take both ways right and down and left (expecting true): " + canTakeBothWaysRightAndDownAndLeft(4,3)); 
			Debug.Log ("Testing if player 1 can take both ways right and up and left (expecting true): " + canTakeBothWaysRightAndUpAndLeft(4,3)); 
			Debug.Log ("Testing if player 1 can take both ways down and up and left (expecting true): " + canTakeBothWaysDownAndUpAndLeft(4,3)); 
			Debug.Log ("Testing if player 1 can take both ways down and up and right (expecting true): " + canTakeBothWaysDownAndUpAndRight(4,3));
			Debug.Log ("Testing if player 1 can take both ways left and up and right (expecting true): " + canTakeBothWaysLeftAndUpAndRight(4,3)); 
			Debug.Log ("Testing if player 1 can take both ways left and down and right (expecting true): " + canTakeBothWaysLeftAndDownAndRight(4,3));

			Debug.Log("Testing if player 1 can take in all directions (expecting true): " + canTakeInAllDirections(4,3)); 
		
			Debug.Log ("");
			Debug.Log ("Setting up new player board and testing if King pieces can take in all directions for player 2"); 
			gameBoard = null;
			gameBoard = new Board ();
			
			gameBoard.boardPieces [3, 2] = new PlayerPiece (1);
			gameBoard.boardPieces [5, 2] = new PlayerPiece (1);
			gameBoard.boardPieces [3, 4] = new PlayerPiece (1);
			gameBoard.boardPieces [5, 4] = new PlayerPiece (1);
			gameBoard.boardPieces [4, 3] = new PlayerPiece (2);
			gameBoard.boardPieces [4, 3].isKing = true;
			
			Debug.Log ("Testing if player 2 can take down and right (expecting true): " + canTakeDownAndRight (4, 3));
			Debug.Log ("Testing if player 2 can take down and left (expecting true): " + canTakeDownAndLeft (4, 3));
			Debug.Log ("Testing if player 2 can take up and right (expecting true): " + canTakeUpAndRight (4, 3));
			Debug.Log ("Testing if player 2 can take up and left (expecting true): " + canTakeUpAndLeft (4, 3));
			
			Debug.Log ("Testing multiple take moves for King without surrounding pieces stopping takes");
			Debug.Log ("Testing if player 2 can take down both ways (expecting true): " + canTakeDownAndBothWays (4, 3));
			Debug.Log ("Testing if player 2 can take left both ways (expecting true): " + canTakeBothWaysLeft (4, 3));
			Debug.Log ("Testing if player 2 can take right both ways (expecting true): " + canTakeBothWaysRight (4, 3));
			Debug.Log ("Testing if player 2 can take Up both ways (expecting true): " + canTakeUpAndBothWays (4, 3)); 
			
			Debug.Log ("Testing if player 2 can take Up and right and down and left (expecting true): " + canTakeUpRightAndDownAndLeft (4, 3));
			Debug.Log ("Testing if player 2 can take Up and left and down and right (expecting true): " + canTakeUpAndLeftAndDownAndRight (4, 3));
			
			Debug.Log ("Testing if player 2 can take both ways up and down and left (expecting true): " + canTakeBothWaysUpAndDownAndLeft(4,3));
			Debug.Log ("Testing if player 2 can take both ways up and down and right (expecting true): " + canTakeBothWaysUpAndDownAndRight(4,3));
			Debug.Log ("Testing if player 2 can take both ways right and down and left (expecting true): " + canTakeBothWaysRightAndDownAndLeft(4,3)); 
			Debug.Log ("Testing if player 2 can take both ways right and up and left (expecting true): " + canTakeBothWaysRightAndUpAndLeft(4,3)); 
			Debug.Log ("Testing if player 2 can take both ways down and up and left (expecting true): " + canTakeBothWaysDownAndUpAndLeft(4,3)); 
			Debug.Log ("Testing if player 2 can take both ways down and up and right (expecting true): " + canTakeBothWaysDownAndUpAndRight(4,3));
			Debug.Log ("Testing if player 2 can take both ways left and up and right (expecting true): " + canTakeBothWaysLeftAndUpAndRight(4,3)); 
			Debug.Log ("Testing if player 2 can take both ways left and down and right (expecting true): " + canTakeBothWaysLeftAndDownAndRight(4,3));
			
			Debug.Log ("Testing if player 2 can take in all directions (expecting true): " + canTakeInAllDirections(4,3)); 

			Debug.Log ("");
			Debug.Log ("Testing move or take methods");
			printList ("Takeable pieces: ", returnTakeablePieces ());
			printList ("Moveable pieces: ", returnMoveablePiecesOne ());
			printList ("Moveable pieces: ", returnMoveablePiecesTwo ());

			gameBoard.boardPieces [4, 3] = null;
			Debug.Log ("Deleted piece [4,3] expecting 8 moves available: Move or take? (expecting 8 Moves): " );
			printList ("Moveable pieces: ", returnMoveablePiecesOne ());
		}
	}
}

/**public void runTests()
{
	Debug.Log("Testing the return of a playerpiece from the gameboard where a piece exists (expecting PlayerPiece): " + gameBoard.returnPlayerPiece (0, 0));
	Debug.Log ("");
	Debug.Log("Testing the return of a playerpiece from the gameboard where a piece doesn't exist (expecting Nothing): " + gameBoard.returnPlayerPiece (0, 1));
	
	Debug.Log ("");
	Debug.Log ("Testing if pieces can move down and left; expecting 3 false and 3 true");
	Debug.Log ("Testing if a piece can move down and left (expecting false): " + canMoveDownAndLeft (0, 0));
	Debug.Log ("Testing if a piece can move down and left (expecting false): " + canMoveDownAndLeft (0, 1));
	Debug.Log ("Testing if a piece can move down and left (expecting false): " + canMoveDownAndLeft (2, 0));
	Debug.Log ("Testing if a piece can move down and left (expecting true): " + canMoveDownAndLeft (2, 2));
	Debug.Log ("Testing if a piece can move down and left (expecting true): " + canMoveDownAndLeft (2, 4));
	Debug.Log ("Testing if a piece can move down and left (expecting true): " + canMoveDownAndLeft (2, 6));
	
	Debug.Log ("");
	Debug.Log ("Testing if pieces can move down and right; expecting 3 false and 3 true");
	Debug.Log ("Testing if a piece can move down and right (expecting false): " + canMoveDownAndRight (0, 0));
	Debug.Log ("Testing if a piece can move down and right (expecting false): " + canMoveDownAndRight (0, 1));
	Debug.Log ("Testing if a piece can move down and right (expecting false): " + canMoveDownAndRight (1, 7));
	Debug.Log ("Testing if a piece can move down and right (expecting true): " + canMoveDownAndRight (2, 0));
	Debug.Log ("Testing if a piece can move down and right (expecting true): " + canMoveDownAndRight (2, 2));
	Debug.Log ("Testing if a piece can move down and right (expecting true): " + canMoveDownAndRight (2, 4));
	
	Debug.Log ("");
	Debug.Log ("Testing if pieces can move up and right; expecting 3 false and 3 true");
	Debug.Log ("Testing if a piece can move up and right (expecting false): " + canMoveUpAndRight (7, 1));
	Debug.Log ("Testing if a piece can move up and right (expecting false): " + canMoveUpAndRight (7, 0));
	Debug.Log ("Testing if a piece can move up and right (expecting false): " + canMoveUpAndRight (5, 7));
	Debug.Log ("Testing if a piece can move up and right (expecting true): " + canMoveUpAndRight (5, 1));
	Debug.Log ("Testing if a piece can move up and right (expecting true): " + canMoveUpAndRight (5, 3));
	Debug.Log ("Testing if a piece can move up and right (expecting true): " + canMoveUpAndRight (5, 5));
	
	Debug.Log ("");
	Debug.Log ("Testing if pieces can move up and left; expecting 3 false and 3 true");
	Debug.Log ("Testing if a piece can move up and left (expecting false): " + canMoveUpAndLeft (7, 1));
	Debug.Log ("Testing if a piece can move up and left (expecting false): " + canMoveUpAndLeft (7, 0));
	Debug.Log ("Testing if a piece can move up and left (expecting false): " + canMoveUpAndLeft (6, 0));
	Debug.Log ("Testing if a piece can move up and left (expecting true): " + canMoveUpAndLeft (5, 1));
	Debug.Log ("Testing if a piece can move up and left (expecting true): " + canMoveUpAndLeft (5, 3));
	Debug.Log ("Testing if a piece can move up and left (expecting true): " + canMoveUpAndLeft (5, 5));
	
	Debug.Log ("");
	Debug.Log ("Testing if a piece can take down and right. Adding appropriate enemy pieces");
	PlayerPiece temp = new PlayerPiece (2);
	gameBoard.boardPieces[3, 1] = temp;
	
	Debug.Log("Test if a player piece can take down and right (expecting true): " + canTakeDownAndRight(2,0,1,false));
	Debug.Log("Test if a player piece can take down and right (expecting false): " + canTakeDownAndRight(2,1,1,false));
	Debug.Log("Test if a player piece can take down and right (expecting false ): " + canTakeDownAndRight(2,2,1,false));
	
	Debug.Log ("Testing if a piece can take down and left.");
	
	Debug.Log("Test if a player piece can take down and left (expecting true): " + canTakeDownAndLeft(2,2,1,false));
	Debug.Log("Test if a player piece can take down and left (expecting false): " + canTakeDownAndLeft(2,1,1,false));
	Debug.Log("Test if a player piece can take down and left (expecting false ): " + canTakeDownAndLeft(2,0,1,false));
	Debug.Log("Test if a player piece can take down and left (expecting false ): " + canTakeDownAndLeft(1,0,1,false));
	
	Debug.Log ("");
	
	Debug.Log ("Testing if a piece can take up and right. Adding appropriate enemy pieces");
	PlayerPiece temp2 = new PlayerPiece (1);
	gameBoard.boardPieces[4, 2] = temp2;
	
	Debug.Log("Test if a player piece can take up and right (expecting true): " + canTakeUpAndRight(5,1,2,false));
	Debug.Log("Test if a player piece can take up and right (expecting false): " + canTakeUpAndRight(5,4,1,false));
	Debug.Log("Test if a player piece can take up and right (expecting false): " + canTakeUpAndRight(5,7,1,false));
	Debug.Log("Test if a player piece can take up and right (expecting false ): " + canTakeUpAndRight(6,0,1,false));
	
	Debug.Log ("");
	Debug.Log ("Resetting board!");
	
	gameBoard = null;
	gameBoard = new Board ();
	gameBoard.SetupPlayerArray ();
	
	temp2 = new PlayerPiece (1);
	gameBoard.boardPieces[4, 4] = temp2;
	
	Debug.Log ("Testing if a piece can take up and left.");
	Debug.Log("Test if a player piece can take up and left (expecting true): " + canTakeUpAndLeft(5,5,2,false));
	Debug.Log("Test if a player piece can take up and left (expecting false): " + canTakeUpAndLeft(5,4,1,false));
	Debug.Log("Test if a player piece can take up and left (expecting false ): " + canTakeUpAndLeft(5,7,1,false));
	Debug.Log("Test if a player piece can take up and left (expecting false ): " + canTakeUpAndLeft(6,0,1,false));
	
	Debug.Log ("");
	Debug.Log ("Resetting board!");
	
	Debug.Log ("Testing can take in multiple directions");
	
	gameBoard = null;
	gameBoard = new Board ();
	PlayerPiece player = new PlayerPiece (2);
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	
	gameBoard.boardPieces [2, 2] = player;
	gameBoard.boardPieces [3, 1] = temp;
	gameBoard.boardPieces [3, 3] = temp2;
	
	Debug.Log("Test if a player piece can take down and left and down and right (expecting true): " + canTakeDownAndBothWays(2,2,1,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	player = new PlayerPiece (2);
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	
	gameBoard.boardPieces [4, 2] = temp;
	gameBoard.boardPieces [4, 4] = temp2;
	gameBoard.boardPieces [5, 3] = player;
	
	Debug.Log("Test if a player piece can take up and left and up and right (expecting true): " + canTakeUpAndBothWays(5,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [4, 2] = temp;
	gameBoard.boardPieces [2, 2] = temp2;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take up and left and down and left (expecting true): " + canTakeBothWaysLeft(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [4, 4] = temp;
	gameBoard.boardPieces [2, 4] = temp2;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take up and right and down and right (expecting true): " + canTakeBothWaysRight(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [4, 4] = temp;
	gameBoard.boardPieces [2, 2] = temp2;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take up and right and down and left (expecting true): " + canTakeUpAndLeftAndDownAndRight(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 4] = temp;
	gameBoard.boardPieces [4, 2] = temp2;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take up and left and down and right (expecting true): " + canTakeUpRightAndDownAndLeft(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	PlayerPiece temp3 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 4] = temp;
	gameBoard.boardPieces [4, 4] = temp2;
	gameBoard.boardPieces [2, 2] = temp3;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take both ways right and up and left (expecting true): " + canTakeBothWaysRightAndUpAndLeft(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	temp3 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 4] = temp;
	gameBoard.boardPieces [4, 4] = temp2;
	gameBoard.boardPieces [4, 2] = temp3;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take both ways right and down and left (expecting true): " + canTakeBothWaysRightAndDownAndLeft(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	temp3 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 2] = temp;
	gameBoard.boardPieces [2, 4] = temp2;
	gameBoard.boardPieces [4, 2] = temp3;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take both ways up and down and left (expecting true): " + canTakeBothWaysUpAndDownAndLeft(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	temp3 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 2] = temp;
	gameBoard.boardPieces [2, 4] = temp2;
	gameBoard.boardPieces [4, 4] = temp3;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take both ways up and down and left (expecting true): " + canTakeBothWaysUpAndDownAndRight(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	temp3 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 2] = temp;
	gameBoard.boardPieces [4, 2] = temp2;
	gameBoard.boardPieces [4, 4] = temp3;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take both ways down and up and left (expecting true): " + canTakeBothWaysDownAndUpAndLeft(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	temp3 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 4] = temp;
	gameBoard.boardPieces [4, 2] = temp2;
	gameBoard.boardPieces [4, 4] = temp3;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take both ways down and up and right (expecting true): " + canTakeBothWaysDownAndUpAndRight(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	temp3 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 2] = temp;
	gameBoard.boardPieces [4, 2] = temp2;
	gameBoard.boardPieces [2, 4] = temp3;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take both ways left and up and right (expecting true): " + canTakeBothWaysLeftAndUpAndRight(3,3,2,false));
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	temp3 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 2] = temp;
	gameBoard.boardPieces [4, 2] = temp2;
	gameBoard.boardPieces [4, 4] = temp3;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take both ways left and down and right (expecting true): " + canTakeBothWaysLeftAndDownAndRight(3,3,2,false));
	
	PlayerPiece finalPiece = new PlayerPiece (1);
	gameBoard.boardPieces [2, 4] = finalPiece;
	Debug.Log("Test is a player piece can take in all directions!! (expecting true): " + canTakeInAllDirections(3,3,2,false));
	
	Debug.Log ("");
	Debug.Log ("Resetting board!");
	
	
	Debug.Log ("Getting Moveable Pieces from both players");
	gameBoard = null;
	gameBoard = new Board ();
	gameBoard.SetupPlayerArray ();
	
	List<string> testList = new List<string> ();
	testList = returnMoveablePiecesOne ();
	testList.AddRange(returnMoveablePiecesTwo ());
	
	foreach (String o in testList){
		Debug.Log(" " + o);
	}
	
	gameBoard = null;
	gameBoard = new Board ();
	temp = new PlayerPiece (1);
	temp2 = new PlayerPiece (1);
	temp3 = new PlayerPiece (1);
	player = new PlayerPiece (2);
	gameBoard.boardPieces [2, 2] = temp;
	gameBoard.boardPieces [4, 2] = temp2;
	gameBoard.boardPieces [4, 4] = temp3;
	gameBoard.boardPieces [3, 3] = player;	
	Debug.Log("Test if a player piece can take both ways left and down and right (expecting true): " + canTakeBothWaysLeftAndDownAndRight(3,3,2,false));
	
	gameBoard.boardPieces [2, 4] = finalPiece;
	
	testList = null;
	
	testList = returnTakeablePieces ();
	
	foreach (String o in testList){
		Debug.Log(" " + o);
	}
	
	Debug.Log ("Performing move or take method");
	moveOrTake ();
	
	gameBoard = null;
	gameBoard = new Board ();
	gameBoard.SetupPlayerArray ();
	
	Debug.Log ("Performing move or take method");
	moveOrTake ();
}**/


/**public void printBoard(int[,] arrBoard) {
			string s = "";
			int i = 0, j = 0;
			
			while(j < 8) {
				while(i < 8) {
					s+= arrBoard[j, i] + " ";
					i++;
				}
				
				s += "\n";
				j++;
				i = 0;
			}
			Debug.Log(s);
		}**/


/**
 * public bool canTakeDownAndRight(int x, int y, int playerNo, Boolean kingPiece) {
			
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			
			if (currentPiece != null) {
				if(currentPiece.playerNo == 1 || currentPiece.CheckIsKing() == true) {
					int opponet = getOpponent (currentPiece.GetPlayerNumber ());
					
					if ((y + 2 < 8) && (x + 2 < 8) && (opponet != null)) {
						if (gameBoard.returnPlayerPiece (x + 1, y + 1) != null) {
							if (gameBoard.returnPlayerPiece (x + 1, y + 1).GetPlayerNumber () == opponet) {
								if (gameBoard.returnPlayerPiece (x + 2, y + 2) == null) {
									return true;
								}
							}
						}
					} 
				}											
			} 
			return false;
		}

		public bool canTakeDownAndLeft(int x, int y, int playerNo, Boolean kingPiece) {
			
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			
			if (currentPiece != null) 
			{
				if ((y - 2 > -1) && (x + 2 < 8))
				{
					PlayerPiece enemy = gameBoard.returnPlayerPiece(x + 1, y - 1);
					if(enemy != null)
					{
						if(enemy.GetPlayerNumber() == getOpponent(currentPiece.GetPlayerNumber()))
						{
							if(gameBoard.returnPlayerPiece( x + 2, y - 2) == null)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool canTakeUpAndLeft(int x, int y, int playerNo, Boolean kingPiece) {
			
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			
			if(currentPiece != null)
			{
				int opponent = getOpponent (currentPiece.GetPlayerNumber());
				
				if ((y - 2 > 0) && (x - 2 > 0) && (opponent != null)) {
					if(gameBoard.returnPlayerPiece(x - 1, y - 1) != null) {
						if (gameBoard.returnPlayerPiece(x - 1, y - 1).GetPlayerNumber() == opponent) {
							if(gameBoard.returnPlayerPiece(x - 2, y - 2) == null) {
								return true;
							}
						}
					}
				}
			} 
			return false;
		}

		public bool canTakeUpAndRight(int x, int y, int playerNo, Boolean kingPiece) {
			
			PlayerPiece currentPiece = gameBoard.returnPlayerPiece (x, y);
			
			if (currentPiece != null) {
				int opponent = getOpponent (currentPiece.GetPlayerNumber ());
				
				if ((y + 2 < 8) && (x - 2 > 0) && (opponent != null)) {
					if (gameBoard.returnPlayerPiece(x - 1, y + 1) != null) {
						if (gameBoard.returnPlayerPiece(x - 1, y + 1).GetPlayerNumber() == opponent) {
							if(gameBoard.returnPlayerPiece(x - 2, y + 2) == null) {
								return true;
							}
						}
					}
				}
			}
			return false;
		}**/