using UnityEngine;
using System.Collections;

enum AIState
{
    Wander,
    Seek,
    Flee

}

public class BotMovement : MonoBehaviour {

    public Transform target;

    public float speed;
    public float turnrate;

    public float mass;
    public float maxForce;
    public float MaxVelocity;

    public float range;

    [SerializeField]
    AIState state;


    public Transform myTransform;

    void Awake()
    {
        myTransform = transform;
    }

	
	// Update is called once per frame
	void Update () {

        switch (state)
        {
            case AIState.Wander:
                Wander();
                break;
            case AIState.Seek:
                Seek();
                break;
            case AIState.Flee:
                break;
            default:
                break;
        }
    }

    void Wander()
    {
        if (target != null)
        {
            if(Vector3.Distance(transform.position, target.position) < range)
            {
                FindNewTarget(target);
            }
            else
            {
                //Seek Target
                Vector3 desired_velocity = Vector3.Normalize(target.transform.position - transform.position) * MaxVelocity;

                Vector3 Steering = desired_velocity - myTransform.up;
                Steering = Vector3.ClampMagnitude(Steering, maxForce);
                Steering = Steering / mass;

                myTransform.up = Vector3.ClampMagnitude((myTransform.up + Steering), speed);
                transform.Translate(myTransform.up * speed * Time.deltaTime);
                myTransform = transform;
            }
            
        }
        else
        {
            FindNewTarget(null);
        }
    }

    void Seek()
    {
        if (target != null)
        {
            //Seek Target
            Vector3 desired_velocity = Vector3.Normalize(target.transform.position - transform.position) * MaxVelocity;

            Vector3 Steering = desired_velocity - myTransform.up;
            Steering = Vector3.ClampMagnitude(Steering, maxForce);
            Steering = Steering / mass;

            myTransform.up = Vector3.ClampMagnitude((myTransform.up + Steering), speed);
            transform.Translate(myTransform.up * speed * Time.deltaTime);
            myTransform = transform;
        }
        else
        {
            FindNewTarget(null);
        }
    }



    void FindNewTarget(Transform oldTarget)
    {
        if(oldTarget == null)
        {
            int randomSelection = Random.Range(0, Waypoints.waypoints.Length);

            target = Waypoints.waypoints[randomSelection];
        }
        else
        {
            while (target == oldTarget)
            {
                int randomSelection = Random.Range(0, Waypoints.waypoints.Length);

                target = Waypoints.waypoints[randomSelection];
            }
        }        
    }
}
