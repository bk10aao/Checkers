using UnityEngine;
using System.Collections.Generic;
using System;

namespace Application
{
	using UnityEngine;
	using System.Collections;

	public class CameraTest2 : MonoBehaviour
	{
		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction * 100, Color.red);
			RaycastHit hit;

			if (Input.GetMouseButtonDown (0)) {
				if (Physics.Raycast (ray, out hit)) {
					Debug.DrawRay (ray.origin, ray.direction * hit.distance, Color.yellow);
					Debug.Log ("Ray hit a " + hit.transform.gameObject.tag);
					//Destroy (hit.transform.gameObject);
				}
			}
		}
	}
}