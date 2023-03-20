using UnityEditor;
using UnityEngine;

public class CameraControl : MonoBehaviour
    {
    public float swipeSpeed = 0.5f; // Velocit� di transizione
    public float smoothTime = 0.3f; // Tempo di transizione
    public float dragSpeed = 0.2f; //velocit� di drag
    public Vector2 lastPosition; // Ultima posizione toccata
    public float moveTowardsSpeed = 0.5f;
    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer bkg;
    float bkgMinX, bkgMaxX;
    Vector3 newPosition;
    private void Awake()
        {
        bkgMinX=bkg.transform.position.x-bkg.bounds.size.x/2f;
        bkgMaxX=bkg.transform.position.x+bkg.bounds.size.x/2f;
        }
    private void Start()
	{
        lastPosition = new Vector2(transform.position.x, transform.position.y);
	}

    float InBounds(float input)
    {
        float camWidht = cam.orthographicSize * cam.aspect;
        float maxX = bkgMaxX - camWidht;
        float minX = bkgMinX + camWidht;
        return Mathf.Clamp(input, minX, maxX);
	}

    public void SwipeCamera(float input) {
        Vector2 enterPosition = new Vector2(input, transform.position.y);
        if (Mathf.Abs(enterPosition.x - lastPosition.x) > 0.1)
            lastPosition = enterPosition;  
		Debug.Log("Swiping: " + input);
        float deltaX = input - lastPosition.x;
        Debug.Log(deltaX);
        // Calcola la nuova posizione della camera sull'asse X
            newPosition = new Vector3(
                InBounds((transform.position.x - deltaX)*swipeSpeed),
                transform.position.y,
                transform.position.z);

        // Applica la transizione graduale alla nuova posizione
        /*transform.position = new Vector3(
            Mathf.MoveTowards(transform.position.x, newPosition.x, moveTowardsSpeed),
            Mathf.MoveTowards(transform.position.y, newPosition.y, moveTowardsSpeed),
            Mathf.MoveTowards(transform.position.z, newPosition.z, moveTowardsSpeed)
        );*/
        transform.position = Vector3.Lerp(transform.position, newPosition, moveTowardsSpeed);
        
        //si salva l'ultima per il deltaX all'inizio
        lastPosition = enterPosition;
    }

    public void DragCamera(Vector3 drag) {
        Vector3 viewPoint = cam.WorldToViewportPoint(drag);
        if (viewPoint.x <= 0.2) {
            //drag verso sinistra
            Vector3 newPosition = new Vector3(InBounds(transform.position.x + Vector3.left.x * dragSpeed), transform.position.y, transform.position.z);
            // Applica la transizione graduale alla nuova posizione
            transform.position = Vector3.Lerp(transform.position, newPosition, smoothTime);

            } else if (viewPoint.x >= 0.8) {
                Vector3 newPosition = new Vector3(InBounds(transform.position.x + Vector3.right.x * dragSpeed), transform.position.y, transform.position.z);
                 // Applica la transizione graduale alla nuova posizione
                 transform.position = Vector3.Lerp(transform.position, newPosition, smoothTime);
            }
    }
        }

    
    