using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using UnityStandardAssets.CrossPlatformInput;

public class FinalRecreationRoom : StateBase
{
    private int _iSelectedObj;
    private int _iCurrDialog = -1;
    private float _fTimeLocked;
    private float _timeToLock = 0.4f;
    private AudioSource _audioSource;
    private GameObject _activeDialog;
    private GameObject _currAnswerKnobs;

    public override void OnEntry()
    {
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, 2);
        SceneManager.LoadScene(sceneName: "WaitingRoom");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void Update()
    {
        // on confirm
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            DataLogger.instance.WriteQuestAnswerLog(_iSelectedObj);

            Vector3 pos = _activeDialog.transform.position;
            Quaternion rot = _activeDialog.transform.rotation;
            GameObject.Destroy(_activeDialog);
            _activeDialog = SessionManager.instance.SpawnNextRecreationDialog(_iCurrDialog, pos, rot);
            _iCurrDialog++;

            if (_activeDialog != null)
            {
                if (_activeDialog.transform.Find("Answers") != null)
                {
                    _fTimeLocked = _timeToLock;
                    _currAnswerKnobs = _activeDialog.transform.Find("Answers").gameObject;
                    _iSelectedObj = _currAnswerKnobs.transform.childCount / 2;
                    _currAnswerKnobs.transform.GetChild(_iSelectedObj).GetChild(0).GetComponent<Renderer>().material = SessionManager.instance.GetDialogSelectionMaterial();
                }
                else
                {
                    _currAnswerKnobs = null;
                }
            }
            else
            {
                DataLogger.instance.CloseMainLog();
                Debug.Log("Aus die Maus!");
            }
        }

        // on selection change
        if (_currAnswerKnobs != null && _fTimeLocked < 0)
        {
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

            if (horizontal + 0.1f < 0)
            {
                DecreaseSelection();
                _fTimeLocked = _timeToLock;
            }
            else if (horizontal - 0.1f > 0)
            {
                IncreaseSelection();
                _fTimeLocked = _timeToLock;
            }
        }
        else
        {
            _fTimeLocked -= Time.deltaTime;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SessionManager.instance.ResetPlayerPosition();
        _activeDialog = SessionManager.instance.SpawnNextRecreationDialog(_iCurrDialog);
        SessionManager.instance.SpawnDiaglogSpotLight();
        _iCurrDialog++;
        SceneManager.sceneLoaded -= OnSceneLoaded;

        _fTimeLocked = _timeToLock;
        _currAnswerKnobs = _activeDialog.transform.Find("Answers").gameObject;
        _iSelectedObj = 1;
        _currAnswerKnobs.transform.GetChild(_iSelectedObj).GetChild(0).GetComponent<Renderer>().material = SessionManager.instance.GetDialogSelectionMaterial();

        _audioSource = PlayerPlatform.instance.GetComponent<AudioSource>();
    }
    private void IncreaseSelection()
    {
        ChangeSelectionTo(_iSelectedObj + 1);
    }

    private void DecreaseSelection()
    {
        ChangeSelectionTo(_iSelectedObj - 1);
    }

    private void ChangeSelectionTo(int i)
    {
        if (i >= 0 & i < _currAnswerKnobs.transform.childCount)
        {
            _currAnswerKnobs.transform.GetChild(_iSelectedObj).GetChild(0).GetComponent<Renderer>().material = SessionManager.instance.GetDialogNotSelectedMaterial();
            _iSelectedObj = i;
            _currAnswerKnobs.transform.GetChild(_iSelectedObj).GetChild(0).GetComponent<Renderer>().material = SessionManager.instance.GetDialogSelectionMaterial();
            _audioSource.clip = SessionManager.instance.GetSelectionSound();
            _audioSource.Play();
        }
    }
}
