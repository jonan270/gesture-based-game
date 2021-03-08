using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using PDollarGestureRecognizer;

public class GestureTracker : MonoBehaviour
{
    public Player player;
    public GameObject obj;

    public float closeDistance = 1.5f;
    private float TimeSinceGuess = 0.0f;

    private const float lifeTime = 5.0f;

    public List<GesturePosition> gesturePositions = new List<GesturePosition>(); //TODO change gameobject to transform position?

    public Camera cam;

    private Vector3 handPositionLeft, handPositionRight;
    private Vector3 oldSpawnPositionLeft, oldSpawnPositionRight;

    private List<Point> points = new List<Point>();
    private List<Gesture> trainingSet = new List<Gesture>();

    public string GestureName = "";


    enum EnumGesture
    {
        none,
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

        //TODO:
        //Check fallback
        //Reset spawn point after some amount of time without movement

        //Left hand
        handPositionLeft = player.GetHand(0).transform.position;
        float distance = (oldSpawnPositionLeft - handPositionLeft).sqrMagnitude;

        if (distance >= closeDistance * closeDistance && stateLeft)
        {
            var temp = Instantiate(obj, handPositionLeft, Quaternion.identity, gameObject.transform);

            oldSpawnPositionLeft = handPositionLeft;

            var gp = new GesturePosition(temp);
            gesturePositions.Add(gp);
        }

        //Right hand
        if (player.GetHand(1) != null)
        {
            handPositionRight = player.GetHand(1).transform.position;
            distance = (oldSpawnPositionRight - handPositionRight).sqrMagnitude;

            if (distance >= closeDistance * closeDistance && stateRight)
            {
                var temp = Instantiate(obj, handPositionRight, Quaternion.identity, gameObject.transform);

                oldSpawnPositionRight = handPositionRight;

                var gp = new GesturePosition(temp);
                gesturePositions.Add(gp);
            }
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

    //Update the lifespan for each GesturePosition and delete invalid GesturePositions.
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

    //Turn GesturePositions into Points for the gesture recognition.
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
    
    //Guess what gesture is being made by the player.
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

    //Add a gesture to a .xml file and save it for further use.
    void AddGesture()
    {
        string fileName = string.Format("{0}/{1}-{2}.xml", "Assets/Gestures/", GestureName, System.DateTime.Now.ToFileTime());

        GestureIO.WriteGesture(points.ToArray(), GestureName, fileName);

        trainingSet.Add(new Gesture(points.ToArray(), GestureName));
        Debug.Log("Adding new gesture: " + GestureName);
    }

}
