using UnityEngine;
using System.Collections.Generic;
using System;
namespace Application
{

	public class Controller : MonoBehaviour {

		public int[,] twoDimensionalArr = new int[8,8];
		public Board gameBoard = new Board();

		void Start () {

			Board gameBoard = new Board();
			gameBoard.SetupPlayerArray ();
		}

		void Update () {
		
		}

		public void setupArray ()
		{
			twoDimensionalArr = new int[8, 8];
			int x = 0, y = 0, player = 1;
			while (x < 8) {
				if (x == 3 || x == 4) {
					x++;
					//just increment x and continue to next level
					player = 2;
				}
				else {
					if (y % 2 == 0 && x % 2 == 0) {
						twoDimensionalArr [x, y] = player;
						Debug.Log("Added piece: " + player.ToString() + " to x: " + x.ToString() + " y: " + y.ToString());
					}
					else
					if (y % 2 == 1 && x % 2 == 1) {
						twoDimensionalArr [x, y] = player;
						Debug.Log("Added piece: " + player.ToString() + " to x: " + x.ToString() + " y: " + y.ToString());
					}
					else {
						twoDimensionalArr [x, y] = 0;
					}
					y++;
					if (y > 7) {
						y = 0;
						x++;
					}
				}
			}
		}
		
		public bool canMoveDownAndRight(int x, int y) {
			if (twoDimensionalArr [x, y] == 1) {
				if ((y + 1 < 8) && (x + 1 < 8)) {
					if (twoDimensionalArr [x + 1, y + 1] == 0) {
						return true;
					} 
				}
			}
			return false;
		}

		public bool canMoveDownAndLeft(int x, int y) {
			if (twoDimensionalArr [x, y] == 1) {
				if ((y - 1 > -1) && (x + 1 < 8)) {
					if (twoDimensionalArr [x + 1, y - 1] == 0) {
						return true;
					}
				}
			}	
			return false;	
		}

		public bool canMoveUpAndRight(int x, int y) {
			if (twoDimensionalArr [x, y] == 2) {
				if ((y + 1 < 8) && (x - 1 > -1)) {
					if (twoDimensionalArr [x - 1, y + 1] == 0) {
						return true;
					}
				}
			}	
			return false;
		}

		public bool canMoveUpAndLeft(int x, int y) {
			if (twoDimensionalArr [x, y] == 2) {
				if ((y - 1 > -1) && (x - 1 > -1)) {

					if (twoDimensionalArr [x - 1, y - 1] == 0) {
						return true;
					} 
				}
			}
			return false;
		}


		public bool canTakeDownAndRight(int x, int y) {
			if ((y + 1 < 8) && (x + 1 < 8)) {
				if (twoDimensionalArr [x + 1, y + 1] == 2) {
					if(((y + 2 < 8) && (x + 2 < 8)) && (twoDimensionalArr [x + 2, y + 2] == 0)) {
						return true;
					}
				}
			} 
			return false;
		}

		public bool canTakeDownAndLeft(int x, int y) {
			if ((y - 1 > -1) && (x + 1 < 8)) {
				if (twoDimensionalArr [x + 1, y - 1] == 2) {
					if(((y - 2 > -1) && (x + 2 < 8)) && (twoDimensionalArr [x + 2, y - 2] == 0)) {
						return true;
					}
				}
			}
			return false;
		}

		public bool canTakeUpAndRight(int x, int y) {
			if ((y + 1 < 8) && (x - 1 > 0)) {
				if (twoDimensionalArr [x - 1, y + 1] == 1) {
					if(((y + 2 < 8) && (x - 2 > 0)) && (twoDimensionalArr [x - 2, y + 2] == 0)) {
						return true;
					}
				}
			} 
			return false;
		}

		public bool canTakeUpAndLeft(int x, int y) {
			if ((y - 1 > -1) && (x - 1 > 0)) {
				if (twoDimensionalArr [x - 1, y - 1] == 1) {
					if(((y - 2 > 0) && (x - 2 > 0)) && (twoDimensionalArr [x - 2, y - 2] == 0)) {
						return true;
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
			if (canTakeUpAndRight (x, y) && canTakeDownAndRight (x, y))
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

		public bool canTakeBothWaysUpAndDownAndLeft(int x,int y) {
			if (canTakeUpAndBothWays (x, y) && canTakeDownAndLeft (x, y))
				return true;
			else
				return false;
		}

		public bool canTakeBothWaysUpAndDownAndRight(int x,int y) {
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
			if(canTakeBothWaysLeft(x, y) && canTakeUpAndRight(x,y))
				return true;
			else
				return false;
		}

		public bool canTakeBothWaysLeftAndDownAndRight(int x, int y) {
			if(canTakeBothWaysLeft(x, y) && canTakeDownAndRight(x,y))
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
						test.Add( "{" + j.ToString() + "," + i.ToString() + "} can move down and right! ");
					if(canMoveDownAndLeft(i,j))
						test.Add("{" + j.ToString() + "," + i.ToString() + "} can move down and left! ");

					i++;
				}
				j++;
				i = 0;
			}
			return test;
		}

		public List<string> returnMoveablePiecesTwo() {   
			List<String> test = new List<String> ();
			int i = 0, j = 0;
			while(j < 8) {
				while(i < 8) {
					if(canMoveUpAndRight(i,j))
						test.Add("{" + j.ToString() + "," + i.ToString() + "} can move Up + right! ");
					if(canMoveUpAndLeft(i,j))
						test.Add("{" + j.ToString() + "," + i.ToString() + "} can move Up + left! ");
					
					i++;
				}
				j++;
				i = 0;
			}
			return test;
		}

		public List<String> returnTakeablePieces() {
			List<String> test = new List<String> ();
			//String s = "";
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

		public void printBoard(int[,] arrBoard) {
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
		}

		public void printList(String s, List<String> printList) {
			foreach (String o in printList){
				Debug.Log(s + " " + o);
			}
		}
	}
}