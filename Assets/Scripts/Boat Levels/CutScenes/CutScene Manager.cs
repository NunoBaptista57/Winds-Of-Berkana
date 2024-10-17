using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    private PlayableDirector playableDirector;
    private bool isCutscenePlaying = false;

    public BoatMovement player; //Might be replaced by a manager sys with the needed references
    public bool playOnAwake = false;

    [Header("Optional reference to load scene after the cutscene")]
    public string sceneToLoad;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
        if (playOnAwake) {beginCutScene();}
    }

    void Update()
    {
        if (isCutscenePlaying && Input.GetKeyDown(KeyCode.Space))
        {
            SkipCutScene();
        }
    }

    public void beginCutScene()
    {
        if (!isCutscenePlaying)
        {
            player.AllowPlayerControl(false);

            playableDirector.Play();

            isCutscenePlaying = true;

            playableDirector.stopped += OnCutsceneFinished;
        }
    }

    private void SkipCutScene()
    {
        if (isCutscenePlaying)
        {
            playableDirector.time = playableDirector.duration;

            playableDirector.Evaluate();

            OnCutsceneFinished(playableDirector);
        }
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        player.AllowPlayerControl(true);

        director.stopped -= OnCutsceneFinished;

        isCutscenePlaying = false;

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
