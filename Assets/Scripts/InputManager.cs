using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static Camera mainCamera;
	private SwipeController swipeController;
	public static float CameraZDistance = 0;
	private RaycastHit hit;
	private Cat DraggedCat;
	bool dragging
	{
		get {
			if (DraggedCat == null) return false;
			else {
				return DraggedCat.isDragged;
			}
		}
	}

	private void Start()
	{
		swipeController = mainCamera.GetComponent<SwipeController>();
	}

	public void Update()
	{
		if (dragging)
		{
			DraggingCat();
		}
		else if (Input.touchCount > 0)
		{
			NewTouch();
		}
	}

	public static Vector3 TouchPosition()
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
			// cat should be in state MOVE and therefore be following touch input

			swipeController.DragCamera(TouchPosition());
		}
		else
		{
			DraggedCat.OnDragEnd();
			DraggedCat = null;
		}
	}

	private void NewTouch()
	{
		// todo if raycast.hit = UI  --> uiManager

		if (Physics.Raycast(TouchPosition(), Vector3.forward, out hit, 100, 1 << 6))
		{
			// save cat instance in cat variable
			hit.collider.GetComponent<Cat>().OnDragStart(DraggedCat);

			swipeController.DragCamera(TouchPosition());
		}
		else
		{
			swipeController.SwipeCamera(TouchPosition().x);
		}
	}
}
