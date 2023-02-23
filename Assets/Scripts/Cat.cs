using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cat : MonoBehaviour, IPlaceable
{
	//todo int rarity (new object? cat subclass?) : decides how rare the cat is to find and how much energy it gets
	//todo int energy (int with complex management, to do after rarity) : can grow with time, how much and how often depends on rarity; static modifier to how much time an action takes
	//todo assigned bed item : the cat's personal bed where he stays to when idle

	//variables to make drag movement work
	private Camera mainCamera;
	private float CameraZDistance;
	private RaycastHit hit;

	//variables for snapping
	static bool isDragging;

	void Start() {
		//camera stuff to make drag movement work
		mainCamera = Camera.main;
		CameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z; //z axis of the game object for screen view
		isDragging = false;
	}
	public Vector3 GetWorldTouch() {
		//get screen position in pixel resolution of touch, then transform it into gameworld coordinates
		Touch touch = Input.GetTouch(0);
		Vector3 ScreenPosition = new Vector3(touch.position.x, touch.position.y, CameraZDistance);
		return mainCamera.ScreenToWorldPoint(ScreenPosition);
	}

	public void Move() {
		//if you get a touch input
		if (Input.touchCount > 0) {

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
		}
	}

	public void Place() {
		//if a drag action occurred but is no longer happening
		if (isDragging && Input.touchCount == 0) { 
			//sphere cast ray to see if anything is touching
			Collider[] sphereHits = Physics.OverlapSphere(hit.transform.position, 2f);
			
			if (sphereHits.Length > 0 ) {
				foreach (Collider sphereHit in sphereHits) {
					//if it's touching an item
					if (sphereHit.gameObject.GetComponent<Item>() != null) {
						hit.transform.position = sphereHit.transform.position;
						//start task
					}
					//if it's touching a slot
					else if (sphereHit.gameObject.GetComponent<Slot>() != null) {
						hit.transform.position = sphereHit.transform.position;
						//change bed
					}
				}
			}

			isDragging = false;
		}
	}

	void Update() {
		//movement on touch drag
		Move();
		Place();
	}
}
