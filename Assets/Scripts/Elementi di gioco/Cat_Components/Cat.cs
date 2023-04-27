using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Cat : MonoBehaviour
{
	#region VARS

	// todo int rarity (new object? cat subclass?) : decides how rare the cat is to find and how much energy it gets
	// todo int energy (int with complex management, to do after rarity) : can grow with time, how much and how often depends on rarity; static modifier to how much time an action takes
	public enum CatState { IDLE, REST, DRAG, WORK, BACKTOBED };
	public CatState state = CatState.IDLE;
	CatState prevState;

	[SerializeField] Animator myAnimController;
	private AnimCatState catActState;

	public enum AnimCatState
	{
		IDLE,
		REST,
		DRAG,
		WORK,
		BACKTOBED,
	};

	private Bed myBed = null;
	private Item myToy = null;

	private float distToBed { get { return Vector3.Distance(transform.position, myBed.transform.position); } }
	public float backSpeed = 0.01f;

	public float placeRadius = 0.5f;
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
		// do nothing, no animation
			
		// exit state if gets a bed assigned or is dragged
		if (myBed != null)
		{
			ChangeState(CatState.REST);
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

		// exits state when OnDragEnd() is called from InputManager
	}

	void Update_WORK()
	{
		// play animation

		// stays on item for x time
		// if still on item when time runs out give money
		// work can be cancelled but UI prompt pops up kinda like in clash of clans

		// leaves item if dragged away --> DRAG
	}

	void Update_BACKTOBED()
	{
		// this shouldn't happen, but if cat has no bed, go idle instead
		if (myBed == null) { ChangeState(CatState.IDLE); return; }
		// if close enough to bed, go to REST
		if (distToBed <= 0.3) { ChangeState(CatState.REST); }

		transform.position = Vector3.MoveTowards(transform.position, myBed.transform.position, backSpeed);
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
				myAnimController.Play(newState.ToString());
				SetState_IDLE();
				break;

			case CatState.REST:
				myAnimController.Play(newState.ToString());
				SetState_REST();
				break;

			case CatState.DRAG:
				myAnimController.Play(newState.ToString());
				SetState_DRAG();
				break;

			case CatState.WORK:
				myAnimController.Play(newState.ToString());
				SetState_WORK();
				break;

			case CatState.BACKTOBED:
				myAnimController.Play(newState.ToString());
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
		transform.position = myBed.transform.position;
		// rest animation start
	}

	void SetState_DRAG()
	{
		// dragging animation start
	}

	void SetState_WORK()
	{
		transform.position = myToy.transform.position;
		// play animation start
	}

	void SetState_BACKTOBED()
	{
		// walk animation start
	}
	#endregion

	#region METHODS

	public Cat OnDragStart()
	{
		//immune to drag if in BACKTOBED or WORKx	
		if (state == CatState.BACKTOBED || state == CatState.WORK) { return null; }

		ChangeState(CatState.DRAG);
		return this;
	}

	public void OnDragEnd()
	{
		ChangeState(CatState.IDLE);
		Place();
	}

	public void Place()
	{
		// sphere cast to see if any item is touching
		Collider[] sphereHits = Physics.OverlapSphere(transform.position, placeRadius, 1 << 7);

		// if nothing is nearby, backtobed
		if (sphereHits.Length == 0)
		{
			ChangeState(CatState.BACKTOBED);
			return;
		}

		// sort any colliders by proximity
		sphereHits = sphereHits.OrderBy(
			x => Vector2.Distance(x.transform.position, transform.position)
			).ToArray();

		// derives item list from collider list
		List<Item> items = GetItemsFromColliders(sphereHits);

		foreach (Item item in items)
		{
			//if it's a bed AND it doesn't already have a cat assigned
			if (item.GetType() == typeof(Bed) && item.myCat == null)
			{
				CoupleTo(item);
				ChangeState(CatState.REST);
				return;
			}

			// if it's an item AND it doesn't already have a cat assigned
			else if (item.GetType() == typeof(Toy) && item.myCat == null)
			{
				CoupleTo(item);
				ChangeState(CatState.WORK);
				return;
			}
		}

		// if code arrives here, foreach failed and no item in list is available: go back to bed
		ChangeState(CatState.BACKTOBED);
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
		newItem.myCat = this;

		if (newItem.GetType() == typeof(Bed))
		{
			myBed = (Bed)newItem;
		}
		else
		{
			myToy = (Toy)newItem;
		}
	}
	#endregion
}
