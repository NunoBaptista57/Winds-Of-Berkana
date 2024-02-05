using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    private enum CutSceneType
    {
        PreRecorded,
        Ship,
    }

    private PlayableDirector playableDirector;
    private bool isCutscenePlaying = false;

    public BoatMovement player; //To be replaced by a proper manager sys

    [Header("General Options")]
    [SerializeField] CutSceneType type;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        
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

    private void OnCutsceneFinished(PlayableDirector director)
    {
        player.AllowPlayerControl(true);

        director.stopped -= OnCutsceneFinished;

        isCutscenePlaying = false;
    }
}
