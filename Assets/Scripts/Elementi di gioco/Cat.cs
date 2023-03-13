using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		// do nothing
		// no animation
		// exit state if gets a bed assigned or is dragged
		if (myBed)
		{
			transform.position = myBed.transform.position;
			SetState_REST();
		}
	}

	void Update_REST()
	{
		// (?) meow if clicked
		// (?) idle routine (roam around the bed, change idle animations...)

		// exits if OnDragStart() is called from InputManager
	}

	void Update_DRAG()
	{
		// dragged animation

		Vector3 touch = InputManager.TouchPosition();
		Vector3 newPosition = new Vector3(touch.x, touch.y, InputManager.CameraZDistance);
		Move(newPosition);

		// exits state when OnDragEnd() is called from InputManager
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

		//TODO: if has item go back to item; else if has bed go back to bed

		// moves at set speed

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

	public void OnDragStart(Cat TouchedCat)
	{
		//immune to drag if in BACKTOBED
		if (state == CatState.BACKTOBED) { return; }

		TouchedCat = this;
		SetState_DRAG();
		return;
	}

	public void OnDragEnd()
	{
		Place();
	}

	public void Move(Vector3 newPosition) {
		transform.position = newPosition;
	}

	public void Place()
	{

		// sphere cast ray to see if anything is touching
		Collider[] sphereHits = Physics.OverlapSphere(hit.transform.position, 2f, 1 << 7);

		// se è vuoto esci
		if (sphereHits.Length == 0)
		{
			SetState_BACKTOBED();
			return;
		}

		//mettili in ordine di distanza
		sphereHits = sphereHits.OrderBy(
			x => Vector2.Distance(x.transform.position, transform.position)
			).ToArray();

		//deriva una lista di item dalla lista di collider
		List<Item> items = GetItemsFromColliders(sphereHits);

		foreach (Item item in items)
		{
			//if it's a bed
			if (item.GetType() == typeof(Bed))
			{
				//se il letto non ha già un gatto assegnato
				if (item.myCat == null)
				{
					CoupleTo(item);

					// allinea gatto al letto e rest
					transform.position = myBed.transform.position;
					SetState_REST();
					return;
				}
			}
			// if it's an item
			else if (item.GetType() == typeof(Item))
			{
				//se item non ha già un gatto assegnato
				if (item.myCat == null)
				{
					CoupleTo(item);
					hit.transform.position = item.transform.position;
					SetState_WORK();
					return;
				}
				
			}
		}

		//se non è successo nulla allora ha fallito, torna a letto
		SetState_BACKTOBED();
	}

	private List<Item> GetItemsFromColliders(Collider[] colliders) {
		List<Item> items = new List<Item>();
		foreach (Collider collider in colliders) {
			Item i = collider.GetComponent<Item>();
			if (i == null) continue;
			items.Add(i);
		}
		return items;
	}

	void CoupleTo(Item newItem)
	{
		if (newItem.GetType() == typeof(Bed))
		{
			newItem.myCat = this;
			myBed = (Bed)newItem;
		}
		else
		{
			newItem.myCat = this;
			myItem = newItem;
		}
	}
	#endregion
}
