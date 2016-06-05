using UnityEngine;
using System.Collections;
namespace Application {

	[System.Serializable]
	public class Board {
		public PlayerPiece[,] boardPieces = new PlayerPiece[8, 8];

		void Start () {

		}
		
		// Update is called once per frame
		void Update () {

		}

		public void SetupPlayerArray() {	
			PlayerPiece piece;

			int x = 0, y = 0, player = 1;
			int count = 1;
			while (x < 8) {
				if (x == 3) {
					x = x + 2;
					player = 2;
					count = 1;
				}
				else {
					if (y % 2 == 0 && x % 2 == 0) {
						piece = new PlayerPiece(player, false);
						boardPieces [x, y] = piece;
						GameObject cylinderTest = CreatePlayerObject (x, y, player, count);
						count++;
						y++;
					}
					else
					{
						if (y % 2 == 1 && x % 2 == 1) {
							piece = new PlayerPiece(player, false);
							boardPieces [x, y] = piece;
							GameObject cylinderTest = CreatePlayerObject (x, y, player, count);
							count++;
							y++;
						}
						else {
							boardPieces [x, y] = null;
							y++;
						}
					}
					if (y > 7) {
						y = 0;
						x++;
					}
				}
			}
		}

		public void printBoard()
		{
			//string s = "";
			int x = 0, y = 0;

			while(x < 8) {
				while(y < 8) {
					if(boardPieces[x,y] == null) {
					   //s+=" null ";
					}
					else {
						//s += boardPieces[x,y].playerNo;
					}
					y++;
				}
				
				//s += "\n";
				x++;
				y = 0;
			}

			//Debug.Log(s);
		}

		public GameObject CreatePlayerObject (int x, int y, int player, int count) {
			GameObject cylinderTest = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			cylinderTest.transform.position = new Vector3 (x, 0.1f, y);
			cylinderTest.transform.localScale = new Vector3 (1.0f, 0.075f, 1.0f);
	
			cylinderTest.tag = "Player" + player.ToString() + "-" + count;
			Renderer rend = cylinderTest.GetComponent<Renderer> ();

			if (player == 1)
				rend.material.color = Color.red;
			else 
				rend.material.color = Color.blue;
			//Debug.Log ("Tag: " + cylinderTest.tag);

			BoxCollider collider = cylinderTest.AddComponent<BoxCollider>();
			collider.size = new Vector3 (1.0f, 1.0f, 1.0f);
			cylinderTest.GetComponent<CapsuleCollider> ().enabled = false;

			return cylinderTest;
		}

		public void DeletePlayerObject(int x, int y)
		{
			GameObject.Destroy (GameObject.FindWithTag (x + "-" + y));
		}

		public PlayerPiece returnPlayerPiece(int x, int y) {
			return boardPieces [x, y];
		}

		public void removePiece(int x, int y) {
			boardPieces [x, y] = null;
			Debug.Log ("removed a player Piece");
		}

		public void AddPlayerPiece(PlayerPiece piece, int x, int y) {
			boardPieces [x, y] = piece;
		}

	}
}


