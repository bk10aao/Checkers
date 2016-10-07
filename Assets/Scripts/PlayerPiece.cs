using System;
using UnityEngine;

public class PlayerPiece {

	public int playerNo;
	public Boolean isKing = false;

	public PlayerPiece (int playerNo, Boolean isKing) {
		this.playerNo = playerNo;
		this.isKing = isKing;
	}

	public int GetPlayerNumber() {
		return playerNo;
	}

	public Boolean CheckIsKing() {
		return isKing;
	}
}