using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MainMenuController menuController;

    [Header("Volume Setting")]
    [SerializeField] private Text volumentTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Setting")]
    [SerializeField] private Text brightnessTexValue = null;
    [SerializeField] private Slider brightnessSlider = null;

    [Header("Quality Setting")]
    [SerializeField] private Dropdown _qualityDropDown;

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle fullScreenToogle = null;

    [Header("Sensitivity Setting")]
    [SerializeField] private Text controllerSentTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumentTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int qualityIndex  = PlayerPrefs.GetInt("masterQuality");
                _qualityDropDown.value = qualityIndex;
                QualitySettings.SetQualityLevel(qualityIndex);
            }
            else
            {
                menuController.ResetButton("Graphics");
            }


#if !UNITY_EDITOR
            if (PlayerPrefs.HasKey("masterFullScreen"))
            {

                int localFullScreen = PlayerPrefs.GetInt("masterFullScreen");

                if(localFullScreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToogle.isOn = true;
                }
                else {
                    Screen.fullScreen = false;
                    fullScreenToogle.isOn = false;
                }
           }
#endif
            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetInt("masterBrightness");
                brightnessSlider.value = localBrightness;
                brightnessTexValue.text = localBrightness.ToString("0.0");

                // Change Brightness
            }


            if (PlayerPrefs.HasKey("masterSen"))
            {
                float localSen = PlayerPrefs.GetInt("masterSen");
                controllerSenSlider.value = localSen;
                controllerSentTextValue.text = localSen.ToString("0.0");

                // Change Brightness
            }

            if (PlayerPrefs.HasKey("masterInverY"))
            {
                int localInvertY = PlayerPrefs.GetInt("masterInverY");

                if (localInvertY == 1)
                {
                    invertYToggle.isOn = true;
                }
                else
                {
                    invertYToggle.isOn = false;
                }
            }
        }
    }
}
