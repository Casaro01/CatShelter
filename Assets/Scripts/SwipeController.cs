using UnityEngine;

public class SwipeController : MonoBehaviour
    {
    public float minX, maxX, clampY, clampZ;
    public float swipeSpeed = 0.5f; // Velocità di transizione
    public float smoothTime = 0.3f; // Tempo di transizione
    private Vector2 lastPosition; // Ultima posizione toccata
    private Vector3 velocity = Vector3.zero; // Velocità di transizione

    void Update() {
        // Se viene rilevato un tocco
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            // Se il tocco inizia
            if (touch.phase == TouchPhase.Began) {
                lastPosition = touch.position;
                }

            // Se il dito si muove
            if (touch.phase == TouchPhase.Moved) {
                // Calcola la differenza di posizione sull'asse X rispetto all'ultimo tocco
                float deltaX = touch.position.x + lastPosition.x;

                // Calcola la nuova posizione della camera sull'asse X
                Vector3 newPosition = new Vector3(transform.position.x + deltaX * swipeSpeed, transform.position.y, transform.position.z);

                // Applica la transizione graduale alla nuova posizione
                transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

                lastPosition = touch.position;
                InBounds();
                }
            }
        }
    void InBounds() {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), clampY, clampZ);
            }
        }
    
    
