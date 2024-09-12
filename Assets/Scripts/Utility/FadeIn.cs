using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeIn : MonoBehaviour
{
    public float fadeDuration = 1f;

    private MaskableGraphic graphic;
    private float currentAlpha = 0f;
    private float fadeSpeed;

    void Start()
    {
        graphic = GetComponent<MaskableGraphic>();
        fadeSpeed = 1f / fadeDuration;
        StartFadeIn();
    }

    void Update()
    {
        currentAlpha += fadeSpeed * Time.deltaTime;
        currentAlpha = Mathf.Clamp01(currentAlpha);

        Color newColor = graphic.color;
        newColor.a = currentAlpha;
        graphic.color = newColor;

        if (currentAlpha >= 1f)
        {
            enabled = false;
        }
    }

    public void StartFadeIn()
    {
        currentAlpha = 0f;
        enabled = true;
    }
}
