using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cat : MonoBehaviour, IPlaceable
{
	// todo int rarity (new object? cat subclass?) : decides how rare the cat is to find and how much energy it gets
	// todo int energy (int with complex management, to do after rarity) : can grow with time, how much and how often depends on rarity; static modifier to how much time an action takes
	// todo assigned bed item : the cat's personal bed where he stays to when idle

	// variables to make drag movement work
	private Camera mainCamera;
	private float CameraZDistance;
	private RaycastHit hit;

	public void Move(Vector3 newPosition) {
		transform.position = newPosition;
		/*
		//if you get a touch input
		Vector3 NewWorldPosition = GetWorldTouch();
		//if touch position in gameworld coordinates is touching object
		if (Physics.Raycast(NewWorldPosition - new Vector3(0, 0, 20), Vector3.forward, out hit, 100)) {
			isDragging = true;
			//if object touched is this one
			if (hit.transform.gameObject == this.gameObject) {
				//move to gameworld coordinates
				hit.transform.position = NewWorldPosition;
			}
			//vvv if you want to move any object remove "if object is this one" part and keep this vvv
			//hit.transform.position = NewWorldPosition;
		}
		else {
			isDragging = false;
		}
		*/
	}

	public void Place() {
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
			// return to previous position
		}
	}
}
