using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private Camera mainCamera;
	private float CameraZDistance;
	private RaycastHit hit;

	public Vector3 GetWorldTouch() {
		//get screen position in pixel resolution of touch, then transform it into gameworld coordinates
		Touch touch = Input.GetTouch(0);
		Vector3 ScreenPosition = new Vector3(touch.position.x, touch.position.y, CameraZDistance);
		return mainCamera.ScreenToWorldPoint(ScreenPosition);
	}
}
