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
	public CatState state = CatState.IDLE;
	CatState prevState;

	private Item myBed = null;
	private Item myItem = null;
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

	}

	void Update_REST()
	{

	}

	void Update_DRAG()
	{

	}

	void Update_WORK()
	{

	}

	void Update_BACKTOBED()
	{

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

	}

	void SetState_REST()
	{

	}

	void SetState_DRAG()
	{

	}

	void SetState_WORK()
	{

	}

	void SetState_BACKTOBED()
	{

	}
	#endregion

	#region METHODS

	public void Move(Vector3 newPosition) {
		transform.position = newPosition;
	}

	public void PlaceIn() {
		// PROBABLY THIS WILL TURN INTO A FUNCTION RETURNING AN ITEM TO SNAP TO

		// sphere cast ray to see if anything is touching
		Collider[] sphereHits = Physics.OverlapSphere(hit.transform.position, 2f);
		
		if (sphereHits.Length > 0 ) {
			foreach (Collider sphereHit in sphereHits) {
				// if it's touching an item
				if (sphereHit.gameObject.GetComponent<Item>() != null) {
					hit.transform.position = sphereHit.transform.position;
					// start task
				}
				// if it's touching a slot
				else if (sphereHit.gameObject.GetComponent<Slot>() != null) {
					hit.transform.position = sphereHit.transform.position;
					// change bed
				}
			}
		}
		else
		{
			PlaceFailed();
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
