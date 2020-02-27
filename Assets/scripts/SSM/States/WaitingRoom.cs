using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class WaitingRoom : StateBase
{
    public override void OnEntry()
    {
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, 2);
        SceneManager.LoadScene(sceneName: "WaitingRoom");
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            StudyStateMachine.instance.MakeTransition(); 
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       SessionManager.instance.ResetPlayerPosition();
        SessionManager.instance.SpawnWelcomeDialog();
        SessionManager.instance.SpawnDiaglogSpotLight();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
