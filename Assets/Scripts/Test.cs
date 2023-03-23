using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
    {
    public float swipeSpeed = 0.5f; // adjust this to change the speed of the swipe
    public float deceleration = 0.01f; // adjust this to change the speed of deceleration
    public float minSpeed = 0.01f; // the minimum speed the camera will move at

    private Vector2 lastPosition;
    private float currentSpeed;

    void Update() {
        if (Input.touchCount == 1) // check for single touch
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                currentSpeed = 0; // reset current speed
                lastPosition = touch.position;
                } else if (touch.phase == TouchPhase.Moved) {
                Vector2 delta = touch.position - lastPosition;
                transform.position += new Vector3(-delta.x * swipeSpeed, 0, 0); // invert direction
                lastPosition = touch.position;
                currentSpeed = -delta.x * swipeSpeed; // set current speed
                } else if (touch.phase == TouchPhase.Ended) {
                // start decelerating
                transform.position += new Vector3(currentSpeed, 0, 0) * Time.deltaTime;
                currentSpeed -= deceleration * Time.deltaTime;
                //StartCoroutine(Decelerate());
                }
            }
        }
    /*IEnumerator Decelerate() {
        float easingFactor = 0.05f; // adjust this to change the easing factor
        float direction=Mathf.Sign(currentSpeed);
        float currentDeceleration = deceleration;
        float targetSpeed = minSpeed * direction; // set the target speed based on direction
        while (Mathf.Abs(currentSpeed) > Mathf.Abs(targetSpeed)) {
            transform.position += new Vector3(currentSpeed, 0, 0) * Time.deltaTime;
            currentSpeed -= currentDeceleration * Time.deltaTime;
            currentDeceleration += currentDeceleration * easingFactor * Time.deltaTime;
            yield return null;
            }
        }*/
    }