using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cat : MonoBehaviour, IPlaceable
{
	#region VARS

	// todo int rarity (new object? cat subclass?) : decides how rare the cat is to find and how much energy it gets
	// todo int energy (int with complex management, to do after rarity) : can grow with time, how much and how often depends on rarity; static modifier to how much time an action takes
	public enum CatState { IDLE, REST, DRAG, WORK, BACKTOBED };
	public CatState state = CatState.REST;
	CatState prevState;

	private Bed myBed = null;
	private Item myItem = null;

	public bool isDragged = false;
	private RaycastHit hit;

	#endregion

	#region UPDATE
	void Update()
	{
		switch (state)
		{
			case CatState.IDLE:
				Update_IDLE();
				break;

			case CatState.REST:
				Update_REST();
				break;

			case CatState.DRAG:
				Update_DRAG();
				break;

			case CatState.WORK:
				Update_WORK();
				break;

			case CatState.BACKTOBED:
				Update_BACKTOBED();
				break;

		}
	}

	void Update_IDLE()
	{
		// nothing
		// if cat is spawned into bed, go to REST
	}

	void Update_REST()
	{
		// (?) meow if clicked
		// (?) idle routine (roam around the bed, change idle animations...)

		// go to DRAG if begun to drag, called from InputManager
		if (isDragged) SetState_DRAG();
	}

	void Update_DRAG()
	{
		// dragged animation

		if (isDragged)
		{
			Vector3 touch = InputManager.TouchPosition();
			Vector3 newPosition = new Vector3(touch.x, touch.y, InputManager.CameraZDistance);
			Move(newPosition);
		}
		else
		{
			Place();
		}
		
	}

	void Update_WORK()
	{
		// play animation

		// stays on item for x time
		// if still on item when time runs out give money

		// leaves item if:
		//		time runs out -> BACKTOBED
		//		dragged away --> DRAG
	}

	void Update_BACKTOBED()
	{
		// walk animation

		// moves towards owned bed at set speed

		// not able to be picked up
		// when arrives, REST
	}

	#endregion

	#region CHANGESTATE

	private void ChangeState(CatState newState)
	{
		if (state == newState) return;

		prevState = state;
		state = newState;

		switch (state)
		{
			case CatState.IDLE:
				SetState_IDLE();
				break;

			case CatState.REST:
				SetState_REST();
				break;

			case CatState.DRAG:
				SetState_DRAG();
				break;

			case CatState.WORK:
				SetState_WORK();
				break;

			case CatState.BACKTOBED:
				SetState_BACKTOBED();
				break;
		}
	}

	void SetState_IDLE()
	{
		//nothing
	}

	void SetState_REST()
	{
		// if coming from BACKTOBED is draggable again

		// align with bed
		// rest animation
	}

	void SetState_DRAG()
	{
		isDragged = true;
		// dragging animation
	}

	void SetState_WORK()
	{
		// align with item
		// play animation
	}

	void SetState_BACKTOBED()
	{
		// is not draggable
		// run animation
	}
	#endregion

	#region METHODS
	public void OnDragStart()
	{
		isDragged = true;
	}

	public void OnDragEnd()
	{
		isDragged = false;
	}

	public void Move(Vector3 newPosition) {
		transform.position = newPosition;
	}

	public void Place() {
		// PROBABLY THIS WILL TURN INTO A FUNCTION RETURNING AN ITEM TO SNAP TO

		// sphere cast ray to see if anything is touching
		Collider[] sphereHits = Physics.OverlapSphere(hit.transform.position, 2f);

		//TODO mettere in ordine i hit in modo che snappi a quello più vicino
		
		if (sphereHits.Length > 0 ) {
			foreach (Collider sphereHit in sphereHits) {
				// if it's touching a bed
				if (sphereHit.gameObject.GetComponent<Bed>() == myBed) {
					SetState_REST();
				}
				// if it's touching an item
				else if (sphereHit.gameObject.GetComponent<Slot>() != null) {
					hit.transform.position = sphereHit.transform.position;
					SetState_WORK();
				}
			}
		}
		else
		{
			transform.position = PlaceFailed().transform.position;
		}
	}

	private Item PlaceFailed()
	{
		if (myItem != null)
		{
			// if working on an item, go back to it
			return myItem;
		}
		else
		{
			// go back to bed
			return myBed;
		}
	}
	#endregion
}
