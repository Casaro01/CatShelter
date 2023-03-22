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

    /*public void SwipeCamera(float input) {
        lastPosition = transform.position;
        Vector2 enterPosition = new Vector2(input, transform.position.y);
        float rawdelta = lastPosition.x - enterPosition.x;
        //if (Mathf.Abs(rawdelta) > 2f)
        //    return;

        float deltaX = rawdelta;
        Vector3 targetPos = new Vector3(
            InBounds(transform.position.x + deltaX),
            transform.position.y,
            transform.position.z            
            );
        Debug.Log($"Lerping {lastPosition} to {targetPos}");
        transform.position =targetPos;
        //transform.position = Vector3.Lerp(transform.position, targetPos, moveTowardsSpeed * Time.deltaTime);

        //if (Mathf.Abs(enterPosition.x - lastPosition.x) > 0.1)
        //    lastPosition = enterPosition;
        //float deltaX = (input - lastPosition.x)*swipeSpeed;
        //Debug.Log("Delta: " + deltaX);
        //// Calcola la nuova posizione della camera sull'asse X
        //  newPosition = new Vector3(
        //    InBounds(transform.position.x - deltaX),
        //    transform.position.y,
        //    transform.position.z);
        //// Applica la transizione graduale alla nuova posizione
        //transform.position = Vector3.Lerp(transform.position, newPosition, moveTowardsSpeed*Time.deltaTime);
        ////si salva l'ultima per il deltaX all'inizio
        //lastPosition = enterPosition;
    }*/
    public void SwipeCamera(float input) {
        Vector3 screenCenter = new Vector3((cam.orthographicSize * cam.aspect) * 0.5f, cam.orthographicSize * 0.5f,transform.position.z);
        Vector3 screenTouch = screenCenter + new Vector3(input, screenCenter.y, transform.position.z);
        Vector3 worldCenterPosition = cam.ScreenToWorldPoint(screenCenter);
        Vector3 worldTouchPosition = cam.ScreenToWorldPoint(screenTouch);
        Vector3 worldDeltaPosition =new Vector3(InBounds( worldTouchPosition.x - worldCenterPosition.x),worldTouchPosition.y-worldCenterPosition;
        transform.position = Vector3.Lerp(transform.position,transform.position-worldDeltaPosition,moveTowardsSpeed*Time.deltaTime);
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

    
    
