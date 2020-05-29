using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMovement : MonoBehaviour
{
    /*private Transform target;
    private Transform anotherTarget;
    private int wavepointIndex = 0;

    public float speed=0.5f;
    public float rotSpeed = 20f;
    public Animator anim;
     * */
    public Transform player;
    public Transform head;
    public Animator anim;

    string state = "patrol";
    public GameObject[] waypoints;
   public int currentWP = 0;
    public float rotSpeed = 0.2f;
    float speed = 0.5f;
    float accuracyWP = 0.5f;
    public int  walkdir = 1;
    void Start()
    {
        //target = WaypointScripts.points[0];
        anim = GetComponent<Animator>();
        int tempRan = Random.Range(0, 2);
        if (tempRan == 1)
        {
            walkdir = 1;
        }
        else
        {
            walkdir = -1;
        }
    }

    void Update()
    {
        Vector3 direction = player.position - this.transform.position;
       float angle = Vector3.Angle(direction, head.up);

        if (state == "patrol" && waypoints.Length > 0)
        {
            anim.SetBool("isWalking", true);

            if (Vector3.Distance(waypoints[currentWP].transform.position, transform.position) < accuracyWP)
            {
                currentWP = currentWP + walkdir;//Random.Range(0, waypoints.Length);

                if (currentWP < 0)
                {
                    currentWP = waypoints.Length-1;
                }
                else if (currentWP >= waypoints.Length)
                {
                    currentWP = 0;
                }
            }
            direction = waypoints[currentWP].transform.position - transform.position;
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            this.transform.Translate(0, 0, Time.deltaTime * speed);
        }

        /*Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized *speed * Time.deltaTime, Space.World);
        anim.SetBool("isWalking",true);
        if (Vector3.Distance(transform.position, target.position) <= .8f)
        {
            GetNextWaypoint();
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(anotherTarget.position), rotSpeed * Time.deltaTime);
         */
    }

    /*
    void GetNextWaypoint()
    {
        if (wavepointIndex >= WaypointScripts.points.Length - 1)
        {
            EndPath();

            return;
        }
        wavepointIndex++;
        target = WaypointScripts.points[wavepointIndex];

        anotherTarget = WaypointScripts.points[wavepointIndex+1];
    }

    void EndPath()
    {
        //PlayerStats.Lives--;
        Destroy(gameObject);
    }
     * */
}
