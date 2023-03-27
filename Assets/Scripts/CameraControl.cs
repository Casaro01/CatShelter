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
    public bool firstTouch = true;
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
            transform.position.z);
	}
    public void SwipeCamera(Vector3 input, TouchPhase fase, Vector3 startPos, float inizio) {
        Vector2 enterPosition = new Vector2(input.x, input.y);
        if (Mathf.Abs(enterPosition.x - lastPosition.x) > 0.5 && firstTouch) { //controllo nell'input per evitare che la camera dal tocco precedente al nuovo facesse uno scatto
            firstTouch = false;
            lastPosition = enterPosition;
            }
        firstTouch = false;
        Vector3 delta = (input - lastPosition) * swipeSpeed; //variazione sull'asse x
        Debug.Log("Delta: " + delta);
        // Calcola la nuova posizione della camera sull'asse X
        newPosition = InBounds(transform.position - delta);
        float endTime = Time.time;
        float speed = Mathf.Abs(delta.x) / (inizio - endTime);
        // Applica la transizione graduale alla nuova posizione
        if (fase == TouchPhase.Ended) {
            Debug.Log("fine tocco");
            if (Mathf.Sign(startPos.x - newPosition.x) == 0) {
                Vector3 temp = InBounds(transform.position + Vector3.one * (Mathf.Abs(delta.x) * speed));
                transform.DOMove(temp, inizio - endTime, false).SetEase(Ease.OutSine);
                } else if (Mathf.Sign(startPos.x - newPosition.x) < 0) {
                Vector3 temp = InBounds(transform.position - Vector3.one * (Mathf.Abs(delta.x) * speed));
                transform.DOMove(temp, inizio - endTime, false).SetEase(Ease.OutSine);
                }
            firstTouch = true;
            newPosition = new Vector3();
            lastPosition = new Vector3();
            delta = new Vector3();
            } else if (fase == TouchPhase.Moved) {
            Debug.Log("mi sto muovendo");
            transform.DOMove(newPosition, 0.5f, false);            }
        //si salva l'ultima per il deltaX all'inizio
        //lastPosition = enterPosition;
        }

    public void DragCamera(Vector3 drag) {
    Vector3 viewPoint = cam.WorldToViewportPoint(drag);
    if (viewPoint.x <= 0.2) {
            //drag verso sinistra
            // Applica la transizione graduale alla nuova posizione
            Vector3 temp = InBounds(transform.position + Vector3.left * dragSpeed);
        transform.DOMove(temp, moveSpeed, false);

        } else if (viewPoint.x >= 0.8) {
            //Vector3 newPosition =InBounds(transform.position+Vector3.right);
            // Applica la transizione graduale alla nuova posizione
            Vector3 temp = InBounds(transform.position + Vector3.right * dragSpeed);
            transform.DOMove(temp, moveSpeed, false);
        }
    }
}