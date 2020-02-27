using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class DataLogger : MonoBehaviour
{
    // logging util
    private bool _measuring = false;
    private FileStream _fsTrajectory;
    private FileStream _fsMain;
    private string _startingPath = @"D:\paper\2019_HMDNav2\study\";

    // temp
    private Vector3 _lastPlatformPos = Vector3.zero;
    private Vector3 _lastPlayerPos = Vector3.zero;
    private float _lastPlayerEuler = 0;
    private float _lastPlatformEuler = 0;
    private Vector3 _playerStartingPosition = Vector3.zero;
    private int _numOfActiveCollisions = 0;
    private bool _colliding = false;
    private float _startingTime = 0f;
    private float _countToSecond = 0f;

    // results
    private float _distancePhysicallyTraveled = 0;
    private float _distanceVirtuallyTraveledForward = 0;
    private float _distanceVirtuallyTraveledBackward = 0;
    private float _virtuallyRotated = 0;
    private float _physicallyRotated = 0;
    private int _iCollectedCoins = 0;
    private int _numOfCollisions = 0;
    private float _totalCollisionTime = 0f;
    private float _totalTime = 0;

    public static DataLogger instance = null;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_measuring)
        {
            // virtual travel distance
            Vector3 currPlayerPos = PlayerPlatform.instance.GetPlatformPosition();
            Vector3 tempPosDiff = currPlayerPos - _lastPlatformPos;

            if (SessionManager.instance.GetCurrentTravelMethod().IsMovingForward())
            {
                _distanceVirtuallyTraveledForward += tempPosDiff.magnitude;
            }
            else if(SessionManager.instance.GetCurrentTravelMethod().IsMovingBackward())
            {
                _distanceVirtuallyTraveledBackward += tempPosDiff.magnitude;
            }
            _lastPlatformPos = currPlayerPos;

            // physical travel distance
            Vector3 currCameraPos = PlayerPlatform.instance.GetPlayerLocalPosition();
            _distancePhysicallyTraveled += (_lastPlayerPos - currCameraPos).magnitude;
            _lastPlayerPos = currCameraPos;

            // virtual rotation
            float currPlayerEuler = PlayerPlatform.instance.GetPlatform().rotation.eulerAngles.y;
            float rotDiff = Mathf.Abs(_lastPlatformEuler - currPlayerEuler) % 360f;
            if (rotDiff > 180f)
            {
                rotDiff = 360f - rotDiff;
            }
            _virtuallyRotated += rotDiff;
            _lastPlatformEuler = currPlayerEuler;

            // physical rotation
            float currCameraEuler = PlayerPlatform.instance.GetPlayer().localRotation.eulerAngles.y;
            rotDiff = Mathf.Abs(_lastPlayerEuler - currCameraEuler) % 360f;
            if (rotDiff > 180f)
            {
                rotDiff = 360f - rotDiff;
            }
            _physicallyRotated += rotDiff;
            _lastPlayerEuler = currCameraEuler;

            // collision time
            if(_colliding)
            {
                if(_numOfActiveCollisions == 0)
                {
                    _colliding = false;
                }
                else
                {
                    _totalCollisionTime += Time.deltaTime;
                }
            }
            else if(_numOfActiveCollisions > 0)
            {
                _colliding = true;
            }

            // log trajectory
            if (_countToSecond > 0.5f)
            {
                AddText(_fsTrajectory, PlayerPlatform.instance.GetPlayerPosition() + " " + PlayerPlatform.instance.GetPlayer().transform.eulerAngles + " " + PlayerPlatform.instance.GetPlatform().transform.eulerAngles.y + "\n");
                _countToSecond = 0f;
            }
            else
            {
                _countToSecond += Time.deltaTime;
            }          
        }  
    }

    public void InitMeasuring()
    {
        _lastPlatformPos = PlayerPlatform.instance.GetPlatformPosition();
        _lastPlayerPos = PlayerPlatform.instance.GetPlayerLocalPosition();
        _lastPlatformEuler = PlayerPlatform.instance.GetPlatform().rotation.eulerAngles.y;
        _lastPlayerEuler = PlayerPlatform.instance.GetPlayer().localRotation.eulerAngles.y;

        _numOfActiveCollisions = 0;
        _colliding = false;
        _distancePhysicallyTraveled = 0;
        _distanceVirtuallyTraveledForward = 0;
        _distanceVirtuallyTraveledBackward = 0;
        _virtuallyRotated = 0;
        _physicallyRotated = 0;
        _iCollectedCoins = 0;
        _numOfCollisions = 0;
        _totalCollisionTime = 0f;
        _startingTime = Time.time;
        _totalTime = 0;

        string path = _startingPath + SessionManager.instance.GetPsydonym();
        Directory.CreateDirectory(path);
        path += "\\trac_" + System.DateTime.Now.ToString("yyyy-MM-dd H-mm-ss") + "_" + SessionManager.instance.GetCurrentTravelMethod().GetName() + ".txt";
        _fsTrajectory = File.Create(path);
        AddText(_fsTrajectory, "# Format = (PlayerPosition) (PlayerRotation) PlatformYaw\n");
    }

    public void SetPlayerStartingPosition()
    {
        _playerStartingPosition = PlayerPlatform.instance.GetPlayerPosition();
    }

    public Vector3 GetPlayerStartingPositon()
    {
        return _playerStartingPosition;
    }

    public void Collided()
    {
        _numOfCollisions++;
        _numOfActiveCollisions++;
    }

    public void UnCollided()
    {
        _numOfActiveCollisions--;
    }

    public void CoinCollected()
    {
        _iCollectedCoins++;
    }

    public void StartMeasurement()
    {
        _measuring = true;
    }

    public void StopMeasurement()
    {
        _totalTime = Time.time - _startingTime;
        _measuring = false;
        if(_fsTrajectory != null)
        {
            _fsTrajectory.Close();
            _fsTrajectory = null;
        }
    }

    public void WriteMainLog()
    {
        string path = _startingPath + SessionManager.instance.GetPsydonym();
        path += "\\main_" + System.DateTime.Now.ToString("yyyy-MM-dd H-mm-ss") + "_" + SessionManager.instance.GetCurrentTravelMethod().GetName() + ".txt";
        _fsMain = File.Create(path);
        AddText(_fsMain, _totalTime.ToString() + "; ");
        AddText(_fsMain, _distancePhysicallyTraveled.ToString() + "; ");
        AddText(_fsMain, _distanceVirtuallyTraveledForward.ToString() + "; ");
        AddText(_fsMain, _distanceVirtuallyTraveledBackward.ToString() + "; ");
        AddText(_fsMain, _virtuallyRotated.ToString() + "; ");
        AddText(_fsMain, _physicallyRotated.ToString() + "; ");
        AddText(_fsMain, _iCollectedCoins + "; ");
        AddText(_fsMain, _numOfCollisions + "; ");
        AddText(_fsMain, _totalCollisionTime.ToString() + "; ");
    }

    public void WriteOrientationErrorLog(float error)
    {
        AddText(_fsMain, error.ToString() + "; ");
    }

    public void WriteQuestAnswerLog(int answer)
    {
        AddText(_fsMain, answer.ToString() + "; ");
    }

    public void CloseMainLog()
    {
        _fsMain.Close();
    }

    private static void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }
}
