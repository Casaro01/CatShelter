using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Item : MonoBehaviour, IPlaceable
{
	private Camera mainCamera;
	private float CameraZDistance;

	void Start() {
		mainCamera = Camera.main;
		CameraZDistance =
			mainCamera.WorldToScreenPoint(transform.position).z; //z axis of the game object for screen view
	}

	public void Move(Vector3 newPosition) {
		/*if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);

			Vector3 ScreenPosition =
				new Vector3(touch.position.x, touch.position.y, CameraZDistance); //z axis added to screen point 
			Vector3 NewWorldPosition =
				mainCamera.ScreenToWorldPoint(ScreenPosition); //Screen point converted to world point

			transform.position = NewWorldPosition;
		}*/
	}

	void Update() {
		// Handle screen touches.
		Move(Vector3.down);
	}
}
