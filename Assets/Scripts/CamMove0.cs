using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove0 : MonoBehaviour
{
    public float speed,minX,maxX, clampY, clampZ;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = QualitySettings.vSyncCount > 0 ? -1: 60;
        QualitySettings.antiAliasing=0;
        Screen.sleepTimeout=SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
            InBounds();
            }
    }
    private void InBounds() {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), clampY, clampZ);
        }
    }
