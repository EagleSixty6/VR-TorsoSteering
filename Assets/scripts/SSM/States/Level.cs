using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class Level : StateBase
{
    private string _levelName;
    private float _CollectingTime = 0.05f;
    private GameObject _collecting = null;
    private float _collectingTimeLeft;
    private TravelBase _activeTravelMethod;


    public Level(string name)
    {
        _levelName = name;
    }

    public override void OnEntry()
    {
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, 2);
        Debug.Log("Start " + _levelName + " ...");
        SceneManager.LoadScene(sceneName: _levelName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SessionManager.instance.InitTravel();
            Debug.Log("Reinitilised Travel Parameter ...");
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Restart Level ...");
            OnEntry();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            StudyStateMachine.instance.MakeTransition();
        }

        CheckForCoinCollection();
    }

    public override void OnExit()
    {
        DataLogger.instance.StopMeasurement();
        DataLogger.instance.WriteMainLog();
        SessionManager.instance.DisableTravel();
        SessionManager.instance.CleanUpTravel();
    }

    private void CheckForCoinCollection()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Collectable")
            {
                if (hit.collider.gameObject == _collecting)
                {

                    _collectingTimeLeft -= Time.deltaTime;
                    if (_collectingTimeLeft <= 0)
                    {
                        _collecting.GetComponent<CoinCollection>().Collect();
                        DataLogger.instance.CoinCollected();
                        _collecting = null;
                        _collectingTimeLeft = _CollectingTime;
                    }
                }
                else
                {
                    _collecting = hit.collider.gameObject;
                    _collectingTimeLeft = _CollectingTime;
                }
            }
            else
            {
                _collecting = null;
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SessionManager.instance.ResetPlayerPosition();
        DataLogger.instance.SetPlayerStartingPosition();
        SessionManager.instance.EnableTravel();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DataLogger.instance.InitMeasuring();
        DataLogger.instance.StartMeasurement();
    }
}
