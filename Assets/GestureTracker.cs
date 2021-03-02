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

    private const float lifeTime = 5.0f;

    public List<GesturePosition> gesturePositions = new List<GesturePosition>(); //TODO change gameobject to transform position?

    public Camera cam;

    private Vector3 handPositionLeft, handPositionRight;
    private Vector3 oldSpawnPositionLeft, oldSpawnPositionRight;

    private List<Point> points = new List<Point>();
    private List<Gesture> trainingSet = new List<Gesture>();

    private float TimeSinceGuess = 0.0f;

    public System.String GestureName = "";



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
        //Load user custom gestures
        string[] filePaths = Directory.GetFiles("Assets/Gestures/", "*.xml");
        foreach (string filePath in filePaths)
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));


    }

    void Update()
    {
        TimeSinceGuess += Time.deltaTime;

        bool stateLeft = SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.LeftHand);
        bool stateRight = SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.RightHand);

        //left hand
        handPositionLeft = player.GetHand(0).transform.position;
        float distance = (oldSpawnPositionLeft - handPositionLeft).sqrMagnitude;

        if (distance >= closeDistance * closeDistance && stateLeft)
        {
            var temp = Instantiate(obj, handPositionLeft, Quaternion.identity, gameObject.transform);

            oldSpawnPositionLeft = handPositionLeft;

            var gp = new GesturePosition(temp);
            gesturePositions.Add(gp);
        }

        //right hand //todo check fallback // TODO spawn point at same place as previous when all points are goners
        handPositionRight = player.GetHand(1).transform.position;
        distance = (oldSpawnPositionRight - handPositionRight).sqrMagnitude;

        if (distance >= closeDistance * closeDistance && stateRight)
        {
            var temp = Instantiate(obj, handPositionRight, Quaternion.identity, gameObject.transform);

            oldSpawnPositionRight = handPositionRight;

            var gp = new GesturePosition(temp);
            gesturePositions.Add(gp);
        }

        UpdateGesturePositions(Time.deltaTime);

        //Holding one of the fire buttons
        if(TimeSinceGuess >= 0.1f) // every 0.1s
        {
            TransformToPoints();
            GuessGesture();
            TimeSinceGuess -= 0.1f;
        }

        if (SteamVR_Actions.default_AddGesture.GetStateDown(SteamVR_Input_Sources.Any))
        {
            AddGesture();
        }
    }


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

    void TransformToPoints()
    {
        points.Clear();

        foreach(GesturePosition gp in gesturePositions)
        {
            Vector3 pos = gp.obj.transform.position;
            float x = cam.WorldToScreenPoint(pos).x;
            float y = cam.WorldToScreenPoint(pos).y;
            
            points.Add(new Point(x, y, 0));
        }
    }
    
    void GuessGesture()
    {
        if (points.Count > 1)
        {
            Gesture candidate = new Gesture(points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
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

    void AddGesture()
    {
        string fileName = System.String.Format("{0}/{1}-{2}.xml", "Assets/Gestures/", GestureName, System.DateTime.Now.ToFileTime());

        GestureIO.WriteGesture(points.ToArray(), GestureName, fileName);

        trainingSet.Add(new Gesture(points.ToArray(), GestureName));
        Debug.Log("Adding new gesture: " + GestureName);
    }

}
