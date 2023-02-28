	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private Camera mainCamera;
	private float CameraZDistance = 0;
	private RaycastHit hit;
	bool dragCat = false;
	public Cat DraggedCat;

	public void Update()
	{
		if (dragCat)
		{
			DraggingCat();
		}
		else if (Input.touchCount > 0)
		{
			NewTouch();
		}
	}

	private Vector3 TouchPosition()
	{
		//get screen position in pixel resolution of touch, then transform it into gameworld coordinates
		Touch touch = Input.GetTouch(0);
		Vector3 ScreenPosition = new Vector3(touch.position.x, touch.position.y, CameraZDistance);
		return mainCamera.ScreenToWorldPoint(ScreenPosition);
	}

	private void DraggingCat()
	{
		if (Input.touchCount > 0)
		{
			// continue drag saved cat instance
			DraggedCat.Move(TouchPosition());

		}
		else
		{
			dragCat = false;
			// place
		}
	}

	private void NewTouch()
	{
		// todo if raycast.hit = UI  --> uiManager

		// save cat instance in cat variable
		DraggedCat = hit.collider.GetComponent<Cat>();

		if (DraggedCat == true)
		{
			dragCat = true;
			// todo make cat save previous position to return to if Place() fails
			DraggedCat.Move(TouchPosition());
			// todo if touch input at edges of the screen { DragCamera() }
		}
		// else if raycast empty -> mainCamera.SwipeCamera();
	}
}
