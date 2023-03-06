using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cat : MonoBehaviour, IPlaceable
{
	// todo int rarity (new object? cat subclass?) : decides how rare the cat is to find and how much energy it gets
	// todo int energy (int with complex management, to do after rarity) : can grow with time, how much and how often depends on rarity; static modifier to how much time an action takes
	private Item myBed = null;
	private Item myItem = null;
	private RaycastHit hit;

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
}
