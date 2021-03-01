using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class GestureTracker : MonoBehaviour
{
    public Player player;
    public GameObject obj;

    public float closeDistance = 1.5f;

    private const float lifeTime = 3.0f;

    public List<GesturePosition> gesturePositions; //TODO change gameobject to transform position?

    private Vector3 handPosition;
    private Vector3 oldSpawnPosition;
    

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
                obj.SetActive(false);
            }
        }        
    }
   
    // Update is called once per frame
    void Update()
    {
        handPosition = player.GetHand(0).transform.position;
        float distance = (oldSpawnPosition - handPosition).sqrMagnitude;

        if (distance >= closeDistance * closeDistance)
        {
            var temp = Instantiate(obj, handPosition, Quaternion.identity, gameObject.transform);
            
            oldSpawnPosition = handPosition;

            var gp = new GesturePosition(temp);
            gesturePositions.Add(gp);
        }

        UpdateGesturePositions(Time.deltaTime);

    }


    void UpdateGesturePositions(float dt)
    {
        for (int i = gesturePositions.Count - 1; i >= 0; i--)
        {
            if (gesturePositions[i].obj.activeInHierarchy)
            {
                gesturePositions[i].Update_GP(dt);
            }
            else
            {
                gesturePositions.RemoveAt(i);
            }
        }
    }
}
