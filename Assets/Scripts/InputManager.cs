using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public  Camera mainCamera;
	private SwipeController swipeController;
	public static float cameraZDistance = 0;
	public float magicNumber = (float)-14.2815361; // don't ask.
	private RaycastHit hit;
	private Cat DraggedCat;
	bool dragging
	{
		get {
			if (DraggedCat == null) return false;
			else return true;
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

	private void DraggingCat()
	{
		if (Input.touchCount > 0)
		{
			// follow touch with cat
			Vector3 touch = TouchPosition();
			Vector3 newPosition = new Vector3(touch.x, touch.y, magicNumber);
			DraggedCat.transform.position = newPosition;

			// move camera when reaching sides
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
			// save cat instance in cat variable through cat method
			DraggedCat = hit.collider.GetComponent<Cat>().OnDragStart();
			
			// even if the drag has already started, we don't actually move anything until next frame
		}
		else
		{
			swipeController.SwipeCamera(TouchPosition().x);
		}
	}

	public Vector3 TouchPosition()	
	{
		//get screen position in pixel resolution of touch, then transform it into gameworld coordinates
		Touch touch = Input.GetTouch(0);
		Vector3 ScreenPosition = new Vector3(touch.position.x, touch.position.y, cameraZDistance);
		Vector3 WorldPosition = mainCamera.ScreenToWorldPoint(ScreenPosition);
		return WorldPosition;
	}
}
