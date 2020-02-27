using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class TrainingGround : StateBase
{
    public override void OnEntry()
    {
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, 2);
        Debug.Log("Start Training ...");
        SceneManager.LoadScene(sceneName: "Training");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SessionManager.instance.InitTravel();
            Debug.Log("Reinitilised Travel Parameter ...");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            StudyStateMachine.instance.MakeTransition(SessionManager.instance.GetNextLevelName());
        }
    }

    public override void OnExit()
    {
        SessionManager.instance.DisableTravel();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SessionManager.instance.ResetPlayerPosition();
        SessionManager.instance.PickTravelMethod();      
        SessionManager.instance.EnableTravel();
        SessionManager.instance.InitTravel();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
