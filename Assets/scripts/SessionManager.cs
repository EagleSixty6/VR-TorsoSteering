using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance = null;

    [SerializeField] private List<string> _listOfLevel;
    [SerializeField] private List<int> _orderOfMethods;
    [SerializeField] private Material _dialogSelectionMat;
    [SerializeField] private Material _dialogNotSelectionMat;
    [SerializeField] private GameObject _welcomeDialog;
    [SerializeField] private GameObject _breakDialog;
    [SerializeField] private List<GameObject> _recreationDialogs;
    [SerializeField] private AudioClip _selectionSound;
    

    private StudyStateMachine _SSM;
    private TravelBase _activeTravelMethod;
    private Vector3 _lastStableCameraForward;
    private Vector3 _lastStableCameraRight;
    private Vector3 _lastStableCameraPosition;
    private float _lastStableCameraYaw;
    private int _currLevel = -1;
    private string _psydonym = "noName";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // setting up study state machine
        _SSM = GetComponent<StudyStateMachine>();

        // creating states
        StateBase entrance = new Entrance();
        StateBase welcome = new WaitingRoom();
        StateBase training = new TrainingGround();
        StateBase finish = new LevelAfterFinish();
        StateBase finalFinish = new LevelAfterFinish();
        StateBase recreation = new RecreationRoom();
        StateBase finalRecreation = new FinalRecreationRoom();
        StateBase breaking = new Break();

        // creating state transitions
        entrance.AddSuccessor(welcome, "");
        welcome.AddSuccessor(training, "");

        StateBase level;
        int i = 0;
        for (; i < _listOfLevel.Count - 1; ++i)
        {
            level = new Level(_listOfLevel[i]);
            training.AddSuccessor(level, _listOfLevel[i]);
            level.AddSuccessor(finish, "");
        }
        level = new Level(_listOfLevel[i]);
        training.AddSuccessor(level, _listOfLevel[i]);
        level.AddSuccessor(finalFinish, "");

        finish.AddSuccessor(recreation, "");
        finalFinish.AddSuccessor(finalRecreation, "");
        recreation.AddSuccessor(breaking, "");
        breaking.AddSuccessor(training, "");

        _SSM.InitAndStart(entrance);
    }

    private void Update()
    {
        if (Camera.main.transform.localPosition != Vector3.zero || SteamVR.instance == null)
        {
            _lastStableCameraPosition = Camera.main.transform.position;
            _lastStableCameraYaw = Camera.main.transform.eulerAngles.y;
            _lastStableCameraForward = Camera.main.transform.forward;
            _lastStableCameraRight = Camera.main.transform.right;
        }
    }

    public TravelBase GetCurrentTravelMethod()
    {
        return _activeTravelMethod;
    }

    public void PickTravelMethod()
    {
        TravelBase[] travelMethods = PlayerPlatform.instance.GetComponents<TravelBase>();
        _activeTravelMethod = travelMethods[_orderOfMethods[_currLevel + 1]];
        _activeTravelMethod.enabled = false;
    }

        public void InitTravel()
    {   
        _activeTravelMethod.Init();
    }

    public void EnableTravel()
    {
        _activeTravelMethod.enabled = true;
    }

    public void DisableTravel()
    {
        _activeTravelMethod.enabled = false;     
    }

    public void CleanUpTravel()
    {
        _activeTravelMethod.CleanUp();
    }

    public void ResetPlayerPosition()
    {
        Vector3 pos = _lastStableCameraPosition - PlayerPlatform.instance.GetPlatformPosition();
        pos.y = 0;
        PlayerPlatform.instance.transform.position = -pos;
        _lastStableCameraPosition.x = 0;
        _lastStableCameraPosition.z = 0;
    }

    public void CoinCollected(GameObject coin)
    {
        Destroy(coin, 0.5f);
    }

    public void SpawnWelcomeDialog()
    {
        SpawnDialog(_welcomeDialog);
    }

    public void SpawnBreakDialog()
    {
        SpawnDialog(_breakDialog);
    }

    public GameObject SpawnNextRecreationDialog(int curr, Vector3 pos, Quaternion rot)
    {
        if (curr + 1 < _recreationDialogs.Count)
        {
            return SpawnDialog(_recreationDialogs[curr + 1], pos, rot);
        }
        else
        {
            return null;
        }

    }

    public GameObject SpawnNextRecreationDialog(int curr)
    {
        if (curr + 1 < _recreationDialogs.Count)
        {
            return SpawnDialog(_recreationDialogs[curr + 1]);
        }
        else
        {
            return null;
        }

    }

    public Material GetDialogSelectionMaterial()
    {
        return _dialogSelectionMat;
    }

    public Material GetDialogNotSelectedMaterial()
    {
        return _dialogNotSelectionMat;
    }

    public AudioClip GetSelectionSound()
    {
        return _selectionSound;
    }

    private GameObject SpawnDialog(GameObject obj)
    {
        Vector3 pos = _lastStableCameraPosition + Vector3.ProjectOnPlane(_lastStableCameraForward, Vector3.up) * 1.5f + _lastStableCameraRight * 0.2f;
        Quaternion rot = Quaternion.Euler(new Vector3(90, _lastStableCameraYaw + 90, 90));
        return GameObject.Instantiate(obj, pos, rot);
    }

    private GameObject SpawnDialog(GameObject obj, Vector3 pos, Quaternion rot)
    {
        return GameObject.Instantiate(obj, pos, rot);
    }

    public void SpawnDiaglogSpotLight()
    {
        GameObject lightParent = new GameObject("SpotLight");
        lightParent.transform.position = Vector3.zero;
        lightParent.transform.rotation = Quaternion.Euler(50, _lastStableCameraYaw, 0);
        Light light = lightParent.AddComponent<Light>();
        light.type = LightType.Directional;
    }

    public string GetPsydonym()
    {
        return _psydonym;
    }

    public string GetNextLevelName()
    {
        _currLevel++;
        if (_currLevel < _listOfLevel.Count)
        {
            return _listOfLevel[_currLevel];
        }
        else
        {
            return "";
        }
    }

    public void SetSubjectAlias(string val)
    {
        _psydonym = val;
        _SSM.MakeTransition();
    }
}
