using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using PDollarGestureRecognizer;
using TMPro;
//Types of gestures, names must match the gesture name in /Gestures/...xml
/// <summary>
/// Gesture Type represents the type of gesture a user makes. Found in GestureTracker.cs
/// </summary>
public enum GestureType
{
    none, //default
    circle,
    horizontalline,
    verticalline,
    s
};

public class GestureTracker : MonoBehaviour
{
    public AudioClip correctGesture, wrongGesture;
    [SerializeField] private AudioSource audioSource;

    public GameObject LeftHand, RightHand, visualAid;
    
    [SerializeField] private CharacterSelector lCShand, rCShand;

    public float closeDistance = 0.5f;
    private float TimeSinceGuess = 0.0f;

    private const float lifeTime = 5.0f;

    public List<GesturePosition> gesturePositions = new List<GesturePosition>(); //TODO change gameobject to transform position?

    public Camera cam;

    private Vector3 handPositionLeft, handPositionRight;
    private Vector3 oldSpawnPositionLeft, oldSpawnPositionRight;

    private List<Point> points = new List<Point>();
    private List<Gesture> trainingSet = new List<Gesture>();

    public string GestureName = "";

    
    
    //A class that keeps track of positions to analyze as gestures.
    [System.Serializable]
    public class GesturePosition{
        public GesturePosition(GameObject _obj, float _life = lifeTime)
        {
            obj = _obj;
            life = _life;
        }
        public GameObject obj;
        private float life;

        public void Update_GP(float dt)
        {
            life -= dt;
            if (life <= 0)
            {
                Destroy(obj);
            }
        }        
    }
   

    void Start()
    {
        //Load custom gestures
        string[] filePaths = Directory.GetFiles("Assets/Gestures/", "*.xml");
        foreach (string filePath in filePaths)
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
    }

    void Update()
    {
        //If we are not trying to make a gesture dont do anything
        if (PlayerManager.Instance.PlayerState == PlayerState.makeGesture)
        {
            //TimeSinceGuess += Time.deltaTime;
            UIText.Instance.DisplayText("Make a gesture");
            //Check wether or not the back buttons are pressed and that hand is free and not holding anything
            bool stateLeft = SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.LeftHand) && lCShand.IsHandFree;
            bool stateRight = SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.RightHand) && rCShand.IsHandFree;
            bool leftReleased = SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.LeftHand);
            bool rightReleased = SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand);

            //TODO:
            //Check fallback



            //Left hand
            handPositionLeft = LeftHand.transform.position;
            float distance = (oldSpawnPositionLeft - handPositionLeft).sqrMagnitude;

            if (stateLeft && distance >= closeDistance * closeDistance)
            {
                InstantiateGesturePosition(handPositionLeft, ref oldSpawnPositionLeft);
            }

            //Right hand
            handPositionRight = RightHand.transform.position;
            distance = (oldSpawnPositionRight - handPositionRight).sqrMagnitude;

            if (stateRight && distance >= closeDistance * closeDistance)
            {
                InstantiateGesturePosition(handPositionRight, ref oldSpawnPositionRight);
            }


            UpdateGesturePositions(Time.deltaTime);

            ////Update gesture guess every 0.1s
            //if(TimeSinceGuess >= 0.1f)
            //{
            //    TransformToPoints();
            //    GuessGesture();
            //    TimeSinceGuess = 0.0f;
            //}

            //Reset spawn point after releasing button  (so that a gesture can be started at the same place as last one)
            if (leftReleased)
            {
                oldSpawnPositionLeft = Vector3.zero;
                GuessGesture();
            }
            if (rightReleased)
            {
                oldSpawnPositionRight = Vector3.zero;
                GuessGesture();

            }

            ////saves a gesture, used only in development
            //if (SteamVR_Actions.default_GrabGrip.GetStateDown(SteamVR_Input_Sources.Any))
            //{
            //    AddGesture();
            //    uitext.text = " Save a new gesture ";
            //}
        }
    }
    /// <summary>
    /// Instantiate gesturePosition 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="oldPosition"></param>
    void InstantiateGesturePosition(Vector3 position, ref Vector3 oldPosition)
    {
        var temp = Instantiate(visualAid, position, Quaternion.identity, gameObject.transform);

        oldPosition = position;

        var gp = new GesturePosition(temp);
        gesturePositions.Add(gp);
    }

    /// <summary>
    /// Update the lifespan for each GesturePosition and delete invalid GesturePositions.
    /// </summary>
    /// <param name="dt"></param>
    void UpdateGesturePositions(float dt)
    {
        for (int i = gesturePositions.Count - 1; i >= 0; i--)
        {
            gesturePositions[i].Update_GP(dt);
            if(gesturePositions[i].obj == null)
            {
                gesturePositions.RemoveAt(i);
            }
        }
    }

    void RemoveGesturePositions()
    {
        foreach (var gp in gesturePositions)
            Destroy(gp.obj);

        gesturePositions.Clear();
        points.Clear();
    }

    /// <summary>
    /// Turn GesturePositions into Points for the gesture recognition.
    /// </summary>
    void TransformToPoints()
    {
        points.Clear();

        foreach(GesturePosition gp in gesturePositions)
        {
            Vector3 pos = gp.obj.transform.position;
            pos = cam.WorldToScreenPoint(pos);
            
            points.Add(new Point(pos.x, pos.y, 0));
        }
    }

    /// <summary>
    ///  Guess what gesture is being made by the player.
    /// </summary>
    void GuessGesture()
    {
        PlayerManager.Instance.OnPlayerStateChanged(PlayerState.idle);
        
        TransformToPoints();

        if (points.Count > 1) //Single point can not be a gesture.
        {
            Gesture candidate = new Gesture(points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
            //TODO Display gesture result ? 
            //TODO Send confirmed gesture to rest of system. 
            Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);
            GestureType gest;
            if (gestureResult.Score >= 0.85f)
            {
                PlayerManager.Instance.OnPlayerStateChanged(PlayerState.idle);
                gest = (GestureType)System.Enum.Parse(typeof(GestureType), gestureResult.GestureClass);
                UIText.Instance.DisplayText("Gesture recognized as \n " + gest.ToString());

                if (FindObjectOfType<HandCards>().activateCard(gest))
                {
                    audioSource.PlayOneShot(correctGesture);
                }else
                {
                    audioSource.PlayOneShot(wrongGesture);
                }
                //AbilityManager.ManagerInstance.ActivateAbilityFromGesture(gest, PlayerManager.Instance.selectedCharacter.GetComponent<Character>());

                //uitext.enabled = false;
                //TODO: add guess gesture on button release instead of every 0.1s also check so that we are in gesture drawing state!
            }
            else
            {
                gest = GestureType.none;
                audioSource.PlayOneShot(wrongGesture);

                Debug.LogError("No gesture was recognized try again");
                UIText.Instance.DisplayText("No gesture recognized try again");
                //PlayerManager.Instance.OnPlayerStateChanged(PlayerState.makeGesture);
                PlayerManager.Instance.OnPlayerStateChanged(PlayerState.idle);


            }
        }

        RemoveGesturePositions();
    }

    /// <summary>
    /// Add a gesture to a .xml file and save it for further use.
    /// </summary>
    void AddGesture()
    {
        string fileName = string.Format("{0}/{1}-{2}.xml", "Assets/Gestures/", GestureName, System.DateTime.Now.ToFileTime());

        GestureIO.WriteGesture(points.ToArray(), GestureName, fileName);

        trainingSet.Add(new Gesture(points.ToArray(), GestureName));
        Debug.Log("Adding new gesture: " + GestureName);
    }

}
