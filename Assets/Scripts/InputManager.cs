using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	#region VARS
	// state variables
	public enum InputState { NULL, DRAG, SWIPE, UI };
	public InputState state = InputState.NULL;
	InputState prevState;

	// camera and touch variables
	public Camera mainCamera;
	private CameraControl cameraController;//swipeController;
	public static float cameraZDistance = 0;
	private RaycastHit hit;

	//cat drag variables
	private Cat DraggedCat;

	#endregion

	#region UPDATE

	private void Start()
	{
		cameraController = mainCamera.GetComponent<CameraControl>();
	}

	void Update()
	{
		switch (state)
		{
			case InputState.NULL:
				Update_NULL();
				break;

			case InputState.DRAG:
				Update_DRAG();
				break;

			case InputState.SWIPE:
				Update_SWIPE();
				break;

			case InputState.UI:
				Update_UI();
				break;
		}
	}

	void Update_NULL()
	{
		//listen to new touches
		if (Input.touchCount > 0) {
			// if touch on cat
			if (Physics.Raycast(TouchPosition(), Vector3.forward, out hit, 100, 1 << 6))
			{
				ChangeState(InputState.DRAG);
			}

			// TODO else if raycast on UI --> uiManager and changestate(UI)

			// else user is swiping camera
			else
			{
				ChangeState(InputState.SWIPE);
			}
		}
	}

	void Update_DRAG()
	{
		if (Input.touchCount > 0)
		{
			// follow touch with cat
			Vector3 touch = TouchPosition();
			Vector3 newPosition = new Vector3(touch.x, touch.y, 0);
			DraggedCat.transform.position = newPosition;

			// move camera when reaching sides
			cameraController.DragCamera(TouchPosition());
		}
		else
		{
			DraggedCat.OnDragEnd();
			DraggedCat = null;
			ChangeState(InputState.NULL);
		}
	}

	void Update_SWIPE()
	{
		if (Input.touchCount > 0) { cameraController.SwipeCamera(TouchPosition().x); }
		else { ChangeState(InputState.NULL); }
	}

	void Update_UI()
	{
		// UI stuff
	}

	#endregion

	#region CHANGESTATE
	private void ChangeState(InputState newState)
	{
		if (state == newState) return;

		prevState = state;
		state = newState;

		switch (state)
		{
			case InputState.NULL:
				SetState_NULL();
				break;

			case InputState.DRAG:
				SetState_DRAG();
				break;

			case InputState.SWIPE:
				SetState_SWIPE();
				break;

			case InputState.UI:
				SetState_UI();
				break;
		}
	}

	void SetState_NULL()
	{
		
	}

	void SetState_DRAG()
	{
		// save cat instance in cat variable through cat method
		DraggedCat = hit.collider.GetComponent<Cat>().OnDragStart();
	}

	void SetState_SWIPE()
	{
		
	}

	void SetState_UI()
	{

	}

	#endregion

	#region METHODS

	public Vector3 TouchPosition()	
	{
		//get screen position in pixel resolution of touch, then transform it into gameworld coordinates
		Touch touch = Input.GetTouch(0);
		Vector3 ScreenPosition = new Vector3(touch.position.x, touch.position.y, cameraZDistance);
		Vector3 WorldPosition = mainCamera.ScreenToWorldPoint(ScreenPosition);
		return WorldPosition;
	}

	#endregion
}
