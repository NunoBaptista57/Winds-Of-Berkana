using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float delayBeforeFadeOut = 1f;

    private MaskableGraphic graphic;
    private float currentAlpha = 1f; // Start fully visible
    private float fadeSpeed;

    private bool isFadingOut = false;
    private float fadeOutStartTime;

    void Start()
    {
        graphic = GetComponent<MaskableGraphic>();
        fadeSpeed = 1f / fadeDuration;

        // Start the delay before fading out
        Invoke("StartFadeOut", delayBeforeFadeOut);
    }

    void Update()
    {
        if (isFadingOut)
        {
            // Calculate time elapsed since fade out started
            float elapsedTime = Time.time - fadeOutStartTime;

            // Calculate current alpha based on elapsed time
            currentAlpha = 1f - Mathf.Clamp01(elapsedTime * fadeSpeed);

            // Update image alpha
            Color newColor = graphic.color;
            newColor.a = currentAlpha;
            graphic.color = newColor;

            // Check if fade out is complete
            if (elapsedTime >= fadeDuration)
            {
                isFadingOut = false;
                enabled = false;
            }
        }
    }

    void StartFadeOut()
    {
        isFadingOut = true;
        fadeOutStartTime = Time.time;
    }
}
