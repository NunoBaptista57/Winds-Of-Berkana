using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelMenu : Menu
{
    [SerializeField] private GameObject _button;

    private void Awake()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            GameObject buttonObject = Instantiate(_button, transform);
            buttonObject.transform.SetSiblingIndex(i - 1);
            if (i == 1)
            {
                FirstSelected = buttonObject;
                UpdateFirstSelected();
            }

            Button button = buttonObject.GetComponent<Button>();

            Text buttonText = buttonObject.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = sceneName;
            }

            button.onClick.AddListener(() => {SceneManager.LoadScene(sceneName);
                                             AudioManager.Instance.StopMusic();});
        }
    }
}