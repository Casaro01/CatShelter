using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public Camera mainCamera;
	private float CameraZDistance = 0;
	private RaycastHit hit;
	bool dragCat = false;
	private Cat DraggedCat;

	public void Update()
	{
		
		if (dragCat)
		{
			DraggingCat();
			Debug.Log("Dragging rilevato.");
		}
		else if (Input.touchCount > 0)
		{
			Debug.Log("Touch rilevato.");
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
			Vector3 newPosition = new Vector3(TouchPosition().x, TouchPosition().y, 11);
			DraggedCat.Move(newPosition);

		}
		else
		{
			dragCat = false;
			DraggedCat = null;
			// todo place
		}
	}

	private void NewTouch()
	{
		// todo if raycast.hit = UI  --> uiManager
		if (Physics.Raycast(TouchPosition(), Vector3.forward, out hit, 100, 1 << 6))
		{
			// save cat instance in cat variable
			DraggedCat = hit.collider.GetComponent<Cat>();

			if (DraggedCat == true)
			{
				Debug.Log("Draggedcat");
				dragCat = true;
				// todo make cat save previous position to return to if Place() fails
				DraggedCat.Move(TouchPosition());
				// todo if touch input at edges of the screen { DragCamera() }
			}
		}
		else
		{
			Debug.Log("Cameramove");
			mainCamera.GetComponent<SwipeController>().SwipeCamera(TouchPosition().x);
		}

	}
}
