using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class CameraControl : MonoBehaviour
    {
    public float swipeSpeed = 0.5f; // Velocità di transizione
    public float dragSpeed = 0.2f; //velocità di drag
    public Vector2 lastPosition; // Ultima posizione toccata
    public float moveTowardsSpeed = 0.5f;//Frame al secondo
    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer bkg;
    [SerializeField]float bkgMinX, bkgMaxX;
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
    public void SwipeCamera(float input, TouchPhase fase) {
        Vector2 enterPosition = new Vector2(input, transform.position.y);
        if (Mathf.Abs(enterPosition.x - lastPosition.x) > 0.1) //controllo nell'input per evitare che la camera dal tocco precedente al nuovo facesse uno scatto
        lastPosition = enterPosition;
        float deltaX = (input - lastPosition.x)*swipeSpeed; //variazione sull'asse x
        Debug.Log("Delta: " + deltaX);
        // Calcola la nuova posizione della camera sull'asse X
        newPosition = new Vector3(
            InBounds(transform.position.x - deltaX),
            transform.position.y,
            transform.position.z);
        // Applica la transizione graduale alla nuova posizione
        if (fase == TouchPhase.Ended) {
            Debug.Log("fine tocco");
            transform.DOMoveX(InBounds(newPosition.x * 1.1f ), 0.5f, false).SetEase(Ease.OutCubic);
            /*Vector3 spintaFinale = new Vector3(DOVirtual.EasedValue(lastPosition,newPosition,me));
            transform.Translate();*/
            } else if (fase == TouchPhase.Moved || fase == TouchPhase.Stationary) {
            Debug.Log("mi sto muovendo");
            transform.DOMove(newPosition,0.5f, false);
            //transform.position = Vector3.Lerp(transform.position, newPosition, moveTowardsSpeed*Time.deltaTime);
            }
    //si salva l'ultima per il deltaX all'inizio
        lastPosition = enterPosition;
    }
    #endregion

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

    
    
