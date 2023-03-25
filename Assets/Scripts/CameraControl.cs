using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Windows;

public class CameraControl : MonoBehaviour
    {
    public float swipeSpeed = 0.5f; // Velocità di transizione
    public float dragSpeed = 0.2f; //velocità di drag
    public Vector3 lastPosition; // Ultima posizione toccata
    public float moveSpeed = 0.5f;//Frame al secondo
    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer bkg;
    [SerializeField]float bkgMinX, bkgMaxX, bkgMaxY,bkgMinY;
    Vector3 newPosition;
    [SerializeField] float currentSpeed;
    private void Awake()
        {
        bkgMinX=bkg.transform.position.x-bkg.bounds.size.x/2f;
        bkgMinY=bkg.transform.position.y-bkg.bounds.size.y/2f;
        bkgMaxX=bkg.transform.position.x+bkg.bounds.size.x/2f;
        bkgMaxY=bkg.transform.position.y+bkg.bounds.size.y/2f;
        }
    private void Start()
	{
        lastPosition = new Vector2(transform.position.x, transform.position.y);
	}

    public Vector3 InBounds(Vector3 input)
    {
        float camWidht = cam.orthographicSize * cam.aspect;
        float camAlt = cam.orthographicSize;
        float maxX = bkgMaxX - camWidht;
        float maxY = bkgMaxY - camAlt;
        float minX = bkgMinX + camWidht;
        float minY = bkgMinY + camAlt;
        return new Vector3(
            Mathf.Clamp(input.x,minX,maxX),
            Mathf.Clamp(input.y,minY,maxY),
            Mathf.Clamp(input.z,transform.position.z,transform.position.z));
	}
    #region QUESTA E' LA VERSIONE FATTA INSIEME A MASSIMO
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
        transform.position = Vector3.Lerp(transform.position, targetPos, moveTowardsSpeed * Time.deltaTime);*/
    #endregion
    #region QUESTO LO AVEVO FATTO IO, MA NON FUNZIONAVA COME DOVEVA (UNA VOLTA TESTATO SU TELEFONO NON FUNZIONAVA COME DA SIMULATOR)
    public void SwipeCamera(Vector3 input, TouchPhase fase, Vector3 startPos) {
        Vector2 enterPosition = new Vector2(input.x, input.y);
        if (Mathf.Abs(enterPosition.x - lastPosition.x) > 0.5) //controllo nell'input per evitare che la camera dal tocco precedente al nuovo facesse uno scatto
        lastPosition = enterPosition;
        Vector3 delta = (input - lastPosition)*swipeSpeed; //variazione sull'asse x
        Debug.Log("Delta: " + delta);
        // Calcola la nuova posizione della camera sull'asse X
        newPosition =
            InBounds(transform.position - delta);
        // Applica la transizione graduale alla nuova posizione
        if (fase == TouchPhase.Ended) {
            Debug.Log("fine tocco");
            if (Mathf.Sign(startPos.x-newPosition.x) == 0) {
                transform.DOMove(InBounds(transform.position + Vector3.right * 0.5f), 0.5f, false);
                } else if (Mathf.Sign(startPos.x - newPosition.x) < 0) {
                transform.DOMove(InBounds(transform.position + Vector3.left * 0.5f), 0.5f, false);
                }
            newPosition = new Vector3();
            lastPosition = new Vector3();
            delta = new Vector3();
            } else if (fase == TouchPhase.Moved || fase == TouchPhase.Stationary) {
            Debug.Log("mi sto muovendo");
            transform.DOMove(newPosition,0.5f, false);
            }
    //si salva l'ultima per il deltaX all'inizio
        lastPosition = enterPosition;
    }
    #endregion

    /*public void SwipeCamera(Vector3 input, TouchPhase fase) {
        if (fase == TouchPhase.Began) {
            currentSpeed = 0;
            if (Mathf.Abs(lastPosition.x - input.x) <1f)
                lastPosition = input;
            else if (Mathf.Abs(lastPosition.x - input.x) < 0.1f)
                return;
            } else if (fase == TouchPhase.Moved || fase == TouchPhase.Stationary) {
            currentSpeed = 0;
            if (Mathf.Abs(lastPosition.x - input.x) < 1f)
                lastPosition = input;
            else if (Mathf.Abs(lastPosition.x - input.x) < 0.1f)
                return;
            Debug.Log(Mathf.Abs(lastPosition.x - input.x));
            Vector3 delta = (input - lastPosition)*swipeSpeed;
            transform.DOMove(InBounds(transform.position - delta), moveSpeed, false);
            lastPosition = transform.position;
            currentSpeed = delta.x * moveSpeed;
            } else if (fase == TouchPhase.Ended) {
            transform.DOMove(InBounds(transform.position-new Vector3(currentSpeed,0f,0f)),0.666f, false);
            }
        }*/
    #region NON MI PIACE, MAGARI DOMANI LO RIGUARDO E POI LO SVILUPPO MEGLIO
    /*public void SwipeCamera(float input) {
        Vector3 screenCenter = new Vector3((cam.orthographicSize * cam.aspect) * 0.5f, cam.orthographicSize * 0.5f,transform.position.z);
        Vector3 screenTouch = screenCenter + new Vector3(input, screenCenter.y, transform.position.z);
        Vector3 worldCenterPosition = cam.ScreenToWorldPoint(screenCenter);
        Vector3 worldTouchPosition = cam.ScreenToWorldPoint(screenTouch);
        Vector3 worldDeltaPosition =new Vector3(InBounds( worldTouchPosition.x - worldCenterPosition.x),transform.position.y,transform.position.z) ;
        transform.position = Vector3.Lerp(transform.position,transform.position-worldDeltaPosition,moveTowardsSpeed*Time.deltaTime);
                }*/
    #endregion
    //il drag funziona come deve, lo si richiama quando il gatto, in fase di dragging, viene spostato verso i lati del telefono
    public void DragCamera(Vector3 drag) {
    Vector3 viewPoint = cam.WorldToViewportPoint(drag);
    if (viewPoint.x <= 0.2) {
        //drag verso sinistra
        //Vector3 newPosition =InBounds(transform.position+Vector3.left);
        // Applica la transizione graduale alla nuova posizione
        transform.DOMove(InBounds(transform.position + Vector3.left * dragSpeed), moveSpeed, false);
        //transform.position = Vector3.Lerp(transform.position, newPosition, moveSpeed*Time.deltaTime);

        } else if (viewPoint.x >= 0.8) {
            //Vector3 newPosition =InBounds(transform.position+Vector3.right);
            // Applica la transizione graduale alla nuova posizione
            transform.DOMove(InBounds(transform.position + Vector3.right * dragSpeed), moveSpeed, false);
            //transform.position = Vector3.Lerp(transform.position, newPosition, moveSpeed*Time.deltaTime);
        }
    }
}

    
    
