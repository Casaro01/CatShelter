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
	private float startTime;

	//cat drag variables
	private Cat DraggedCat;
	public Vector3 startPos;
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
			if (Input.GetTouch(0).phase == TouchPhase.Began)
				startPos = TouchPosition();



			// if user's touch is on cat
			if (Physics.Raycast(TouchPosition(), Vector3.forward, out hit, 100, 1 << 6)) {
				// save pointer to cat instance in variable
				DraggedCat = hit.collider.GetComponent<Cat>().OnDragStart();

				// if cat validated the OnDragStart, then go to drag; else display message
				if (DraggedCat != null) {
					ChangeState(InputState.DRAG);
					} else {
					Debug.Log("Gatto non disponibile!");
					}
				}

			// else user is swiping camera
			//if (Input.GetTouch(0).phase != TouchPhase.Ended)
			else {
				startTime = Time.time;
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
			ChangeState(InputState.NULL);
		}
	}

	void Update_SWIPE()
	{
		if (Input.touchCount > 0) {
			if (Input.GetTouch(0).phase != TouchPhase.Ended) {
				//mainCamera.GetComponent<Test>();
				cameraController.SwipeCamera(TouchPosition(), Input.GetTouch(0).phase, startPos, startTime);
				}
			//else if(Input.touches[0].phase == TouchPhase.Stationary)
			//	ChangeState(InputState.NULL);
			}
		//if (Input.touchCount > 0) { cameraController.SwipeCamera(TouchPositionScreenSpace().x); }
		else {
			cameraController.firstTouch = true;
			ChangeState(InputState.NULL);
			}
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
		}
	}

	void SetState_NULL()
	{
		DraggedCat = null;
	}


	#endregion

	#region METHODS
	public Vector3 TouchPositionScreenSpace() {
		//get screen position in pixel resolution of touch, then transform it into gameworld coordinates
		Touch touch = Input.GetTouch(0);
		Vector3 ScreenPosition = new Vector3(touch.position.x, touch.position.y, cameraZDistance);
		return ScreenPosition;

		}
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
