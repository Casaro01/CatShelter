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
			if (Input.touchCount > 0)
			{
				// continue drag saved cat instance
				DraggedCat.GetComponent<Cat>.Move(TouchPosition);
				
			}
			else
			{
				dragCat = false;
				// place
			}
		}
		else if (Input.touchCount > 0)
		{
			// todo if raycast.hit = UI  --> uiManager

			// save cat instance in cat variable
			
			if (hit.collider.GetComponent<Cat>() == true)
			{
				dragCat = true;
				DraggedCat = hit.collider.gameObject;
				// make cat save previous position to return to if Place() fails
				DraggedCat.GetComponent<Cat>.Move(TouchPosition);
				// if touch input at edges of the screen { DragCamera() }
			}
			// else mainCamera.SwipeCamera();
		}
	}

	public Vector3 TouchPosition()
	{
		//get screen position in pixel resolution of touch, then transform it into gameworld coordinates
		Touch touch = Input.GetTouch(0);
		Vector3 ScreenPosition = new Vector3(touch.position.x, touch.position.y, CameraZDistance);
		return mainCamera.ScreenToWorldPoint(ScreenPosition);
	}
}
