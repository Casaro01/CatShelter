using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public Camera mainCamera;
	private SwipeController swipeController;
	private float CameraZDistance = 0;
	private RaycastHit hit;
	bool dragCat = false;
	private Cat DraggedCat;

	private void Start()
	{
		swipeController = mainCamera.GetComponent<SwipeController>();
	}

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
		Vector3 WorldPosition = mainCamera.ScreenToWorldPoint(ScreenPosition);
		return WorldPosition;
	}

	private void DraggingCat()
	{
		if (Input.touchCount > 0)
		{
			// continue drag saved cat instance
			Vector3 touch = TouchPosition();
			Vector3 newPosition = new Vector3(touch.x, touch.y, CameraZDistance);
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
			dragCat = true;
			// save cat instance in cat variable
			DraggedCat = hit.collider.GetComponent<Cat>();
			DraggedCat.Move(TouchPosition());
			// todo make cat save previous position to return to if Place() fails - maybe better fitting in the cat.move() methid
			// todo if touch input at edges of the screen { DragCamera() }
		}
		else
		{
			swipeController.SwipeCamera(TouchPosition().x);
		}

	}
}
