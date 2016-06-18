/**using UnityEngine;
using System.Collections;
using System;

namespace Application {

	public class AIOpponent : MonoBehaviour {

		ArrayList moveablePieces;
		ArrayList takeablePieces = new ArrayList();
		Board gameBoard;
		LogicController logicController;

		Boolean aiTaken = false;

		GameObject interactionPiece;

		public AIOpponent() {

		}

		public void getAiMove (LogicController logicController, ControllerV13 gameContonroller, RaycastHit hit)
		{

			if (logicController.canTakeUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
				interactionPiece = hit.collider.gameObject;
				gameContonroller.takeUpAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, interactionPiece);
				gameContonroller.destroyPieceAI ((int)hit.transform.position.x, (int)hit.transform.position.z, -2.0f, 2.0f);
				interactionPiece.transform.position = new Vector3 (hit.transform.position.x - 1, 0.1f, hit.transform.position.z + 1);
				aiTaken = true;
			}
			else if (logicController.canTakeUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
				interactionPiece = hit.collider.gameObject;
				gameContonroller.takeUpAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
				gameContonroller.destroyPieceAI ((int)hit.transform.position.x, (int)hit.transform.position.z, -2.0f, -2.0f);
				hit.collider.gameObject.transform.position = (new Vector3 ((int)hit.transform.position.x - 1, 0.1f, (int)hit.transform.position.z - 1));
				aiTaken = true;
			}
			else if (logicController.canTakeDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
				interactionPiece = hit.collider.gameObject;
				gameContonroller.takeDownAndRight ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
				gameContonroller.destroyPieceAI ((int)hit.transform.position.x, (int)hit.transform.position.z, 2.0f, 2.0f);
				hit.collider.gameObject.transform.position = (new Vector3 ((int)hit.transform.position.x + 1, 0.1f, (int)hit.transform.position.z + 1));
				aiTaken = true;
			}
			else if (logicController.canTakeDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, gameBoard)) {
				interactionPiece = hit.collider.gameObject;
				gameContonroller.takeDownAndLeft ((int)hit.transform.position.x, (int)hit.transform.position.z, hit.collider.gameObject);
				gameContonroller.destroyPieceAI ((int)hit.transform.position.x, (int)hit.transform.position.z, 2.0f, -2.0f);
				hit.collider.gameObject.transform.position = (new Vector3 ((int)hit.transform.position.x + 1, 0.1f, (int)hit.transform.position.z - 1));
				aiTaken = true;
			}
		}
	}
}**/