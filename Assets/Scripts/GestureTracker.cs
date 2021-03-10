using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using PDollarGestureRecognizer;

public class GestureTracker : MonoBehaviour
{
    
    public GameObject LeftHand, RightHand, visualAid;

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

    //Types of gestures, names must match the gesture name in /Gestures/...xml 
    enum EnumGesture
    {
        none, //default
        circle,
        cross,
        horizontalline,
        verticalline,
        square,
        s
    };
    
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
        TimeSinceGuess += Time.deltaTime;

        //Check wether or not the back buttons are pressed
        bool stateLeft = SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.LeftHand);
        bool stateRight = SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.RightHand);
        bool leftReleased = SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.LeftHand);
        bool rightReleased = SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand);

        //TODO:
        //Check fallback

        //Reset spawn point after releasing button  (so that a gesture can be started at the same place as last one)
        if (leftReleased)
        {
            oldSpawnPositionLeft = Vector3.zero;
        }
        if (rightReleased)
        {
            oldSpawnPositionRight = Vector3.zero;
        }

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

        //Update gesture guess every 0.1s
        if(TimeSinceGuess >= 0.1f)
        {
            TransformToPoints();
            GuessGesture();
            TimeSinceGuess = 0.0f;
        }

        if (SteamVR_Actions.default_GrabGrip.GetStateDown(SteamVR_Input_Sources.Any))
        {
            AddGesture();
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
        if (points.Count > 1) //Single point can not be a gesture.
        {
            Gesture candidate = new Gesture(points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
            //TODO Display gesture result ? 
            //TODO Send confirmed gesture to rest of system. 
            Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);
            EnumGesture gest;
            if (gestureResult.Score >= 0.85f)
            {
                gest = (EnumGesture)System.Enum.Parse(typeof(EnumGesture), gestureResult.GestureClass);
            }
            else
            {
                gest = EnumGesture.none;
            }
            Debug.Log(gest);
        }
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
