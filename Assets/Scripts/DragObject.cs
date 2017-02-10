using UnityEngine;

public class DragObject : MonoBehaviour  {
	private Vector3 screenPoint;

	private void OnMouseDown() {
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
	}

	private void OnMouseDrag(){
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		transform.position = new Vector3(curPosition.x, 1.0f, curPosition.z);
	}
}