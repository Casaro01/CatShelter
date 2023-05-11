using UnityEngine;
using UnityEngine.UI;

public class FramerateCounter : MonoBehaviour
    {
    public Text framerateText;
    public float updateInterval = 0.5f;

    private float accum = 0.0f;
    private int frames = 0;
    private float timeLeft;

    void Start() {
        timeLeft = updateInterval;
        }

    void Update() {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeLeft <= 0.0) {
            float fps = accum / frames;
            string fpsText = string.Format("{0:F2} FPS", fps);
            framerateText.text = fpsText;

            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
            }
        }
    }