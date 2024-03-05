using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float fadeDuration = 1f; 

    private Image image;
    private float currentAlpha = 0f;
    private float fadeSpeed;

    void Start()
    {
        image = GetComponent<Image>();
        fadeSpeed = 1f / fadeDuration;
    }

    void Update()
    {
        currentAlpha += fadeSpeed * Time.deltaTime;
        currentAlpha = Mathf.Clamp01(currentAlpha);

        Color newColor = image.color;
        newColor.a = currentAlpha;
        image.color = newColor;

        if (currentAlpha >= 1f)
        {
            enabled = false;
        }
    }
}

