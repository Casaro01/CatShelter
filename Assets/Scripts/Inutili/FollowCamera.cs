using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float followSpeed = 10f;
    private Transform mainCameraTransform;

    void Start() {
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate() {
        transform.position = mainCameraTransform.position;
        transform.rotation = mainCameraTransform.rotation;
        
    }
}
