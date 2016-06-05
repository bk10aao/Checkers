using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	public Vector3 rotation_speed;

	private bool isMoving = false;

	// Update is called once per frame
	//public bool rotate = true;

	void Update () {
		//Debug.Log (transform.rotation);
		//transform.Rotate (rotation_speed * Time.deltaTime);

		if (Input.GetKey (KeyCode.Space)) {
			StartCoroutine(move());
		}
	}

	public IEnumerator move()
	{
		if (!isMoving) {

			float time = 0f;

			Vector3 startRot = transform.localRotation.eulerAngles;
			Vector3 endRot = startRot;

			endRot.z-= 90f;

			while(time < 3f) {
				yield return new WaitForEndOfFrame();
				time += Time.deltaTime;
				//transform.localRotation.eulerAngles = Vector3.Lerp(startRot, endRot, time / 3f);
				//transform.Rotate(Vector3.Lerp(startRot, endRot, time / 3f));
				transform.Rotate(Vector3.Lerp(Vector3.zero, new Vector3(0f,0f,-0.25f), time / 3f));
				Debug.Log(transform.rotation.z);
			}
		}

		yield return this;
	}

}
