using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private Text volumentTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [SerializeField]
    private GameObject confirmationPrompt = null;

    [Header("Gameplay Settings")]
    [SerializeField] private Text controllerSentTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private float defaultSen = 1.0f;
    public float mainControllerSen = 1;
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private Text brightnessTexValue = null;
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private float defaultBrightness = 1.0f;

    public int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("Levels to Load")]
    public string _newGameLevel;
    public string levelToLoad;

    [Header("Resolution Dropdowns")]
    public Dropdown resolutionDropDown;
    private Resolution[] resolutions;

    [Space(10)]
    [SerializeField] private Dropdown _qualityDropDown;
    [SerializeField] private Toggle fulScreenToggle;

    [SerializeField] private GameObject noSavedGameDialog = null;

    public AudioClip mainMenuSoundtrack;
    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

        AudioManager.Instance.PlayMusic("MainMenuTheme", true);

    }

    // In case we want a prompt for a new game
    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void LoadGameDialogYes()
    {

        SceneManager.LoadScene(levelToLoad);

        /*if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }*/
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        // Check if we are using TMP.pro
        volumentTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        switch (MenuType)
        {
            case "Audio":
                AudioListener.volume = defaultVolume;
                volumeSlider.value = defaultVolume;
                volumentTextValue.text = defaultVolume.ToString("0.0");
                VolumeApply();
                break;

            case "Gameplay":
                controllerSentTextValue.text = defaultSen.ToString("0");
                controllerSenSlider.value = defaultSen;
                mainControllerSen = defaultSen;
                invertYToggle.isOn = false;
                GameplayApply();
                break;

            case "Graphics":
                brightnessSlider.value = defaultBrightness;
                brightnessTexValue.text = defaultBrightness.ToString("0.0");

                _qualityDropDown.value = 1;
                QualitySettings.SetQualityLevel(1);

                fulScreenToggle.isOn = false;
                Screen.fullScreen = false;

                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
                resolutionDropDown.value = resolutions.Length;
                GraphicsApply();

                break ;
        }

           
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        controllerSentTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInverY", 1);
            //InvertY
        }
        else
        {
            PlayerPrefs.SetInt("masterInverY", 0);
            //No Invert 
;        }

        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTexValue.text = brightness.ToString("0.0");
    }

     
    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, _isFullScreen);
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        //Apply these settings
#if !UNITY_EDITOR
        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
        StartCoroutine(ConfirmationBox());
#endif
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
