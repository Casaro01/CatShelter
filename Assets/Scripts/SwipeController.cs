using UnityEngine;

public class SwipeController : MonoBehaviour
    {
    public float minX, maxX;
    public float swipeSpeed = 0.5f; // Velocità di transizione
    public float smoothTime = 0.3f; // Tempo di transizione
    public float dragSpeed = 0.2f; //velocità di drag
    private Vector2 lastPosition; // Ultima posizione toccata
    private Vector3 velocity = Vector3.zero; // Velocità di transizione
    [SerializeField] private float moveTowardsSpeed = 1f;
	private void Start()
	{
        lastPosition = new Vector2(transform.position.x, transform.position.y);
	}

	void Update() {
        /*// Se viene rilevato un tocco
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            // Se il tocco inizia
            if (touch.phase == TouchPhase.Began) {
                lastPosition = touch.position;
                }

            // Se il dito si muove
            if (touch.phase == TouchPhase.Moved) {
                // Calcola la differenza di posizione sull'asse X rispetto all'ultimo tocco
                float deltaX = touch.position.x - lastPosition.x;

                // Calcola la nuova posizione della camera sull'asse X
                Vector3 newPosition = new Vector3(transform.position.x - deltaX * swipeSpeed, transform.position.y, transform.position.z);

                // Applica la transizione graduale alla nuova posizione
                transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

                lastPosition = touch.position;
                InBounds();
                }
            }*/
        }

    float InBounds(float input)
    {
        return Mathf.Clamp(input, minX, maxX);
	}

    public void SwipeCamera(float input) {
        Debug.Log("Swiping: " + input);
        float deltaX = input - lastPosition.x;
        // Calcola la nuova posizione della camera sull'asse X
        Vector3 newPosition = new Vector3(
            InBounds(transform.position.x - deltaX * swipeSpeed),
            transform.position.y,
            transform.position.z);
        // Applica la transizione graduale alla nuova posizione
        //transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        transform.position = new Vector3(
            Mathf.MoveTowards(transform.position.x, newPosition.x, moveTowardsSpeed),
            Mathf.MoveTowards(transform.position.y, newPosition.y, moveTowardsSpeed),
            Mathf.MoveTowards(transform.position.z, newPosition.z, moveTowardsSpeed)
        );
        //transform.position = Vector3.MoveTowards(transform.position, newPosition, moveTowardsSpeed);
        //si salva l'ultima per il deltaX all'inizio
        lastPosition.x = input;
    }

    /*public void DragCamera(Vector3 drag) {
            if(drag)
            // Calcola la nuova posizione della camera sull'asse X
            Vector3 newPosition = new Vector3(transform.position.x - deltaX * swipeSpeed, transform.position.y, transform.position.z);
            // Applica la transizione graduale alla nuova posizione
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
            InBounds();
            }*/
        }

    
    
