using UnityEditor;
using UnityEngine;

public class CameraControl : MonoBehaviour
    {
    public float swipeSpeed = 0.5f; // Velocità di transizione
    public float dragSpeed = 0.2f; //velocità di drag
    public Vector2 lastPosition; // Ultima posizione toccata
    public float moveTowardsSpeed = 0.5f;//Frame al secondo
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
        float deltaX = (input - lastPosition.x)*swipeSpeed;
        // Calcola la nuova posizione della camera sull'asse X
          newPosition = new Vector3(
            InBounds(transform.position.x - deltaX),
            transform.position.y,
            transform.position.z);
        // Applica la transizione graduale alla nuova posizione
        transform.position = Vector3.Lerp(transform.position, newPosition, moveTowardsSpeed*Time.deltaTime);
        //si salva l'ultima per il deltaX all'inizio
        lastPosition = enterPosition;
    }

    public void DragCamera(Vector3 drag) {
        Vector3 viewPoint = cam.WorldToViewportPoint(drag);
        if (viewPoint.x <= 0.2) {
            //drag verso sinistra
            Vector3 newPosition = new Vector3(InBounds(transform.position.x + Vector3.left.x * dragSpeed), transform.position.y, transform.position.z);
            // Applica la transizione graduale alla nuova posizione
            transform.position = Vector3.Lerp(transform.position, newPosition, moveTowardsSpeed*Time.deltaTime);

            } else if (viewPoint.x >= 0.8) {
                Vector3 newPosition = new Vector3(InBounds(transform.position.x + Vector3.right.x * dragSpeed), transform.position.y, transform.position.z);
                 // Applica la transizione graduale alla nuova posizione
                 transform.position = Vector3.Lerp(transform.position, newPosition, moveTowardsSpeed * Time.deltaTime);
            }
    }
        }

    
    
