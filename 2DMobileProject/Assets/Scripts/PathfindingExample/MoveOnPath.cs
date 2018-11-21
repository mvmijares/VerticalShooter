using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour {

    public EditorPathScript pathToFollow;

    public int currentWayPointID = 0;
    public float speed;
    private float reachDistance = 1.0f;
    public float rotationSpeed = 0.5f;
    public string pathName;

    Vector3 last_position;
    Vector3 current_position;

	// Use this for initialization
	void Start () {
        pathToFollow = GameObject.Find(pathName).GetComponent<EditorPathScript>();
        last_position = transform.position;	
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(pathToFollow.path_objs[currentWayPointID].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, pathToFollow.path_objs[currentWayPointID].position, Time.deltaTime * speed);

        Quaternion rotation = Quaternion.LookRotation(pathToFollow.path_objs[currentWayPointID].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        if(distance <= reachDistance) {
            currentWayPointID++;
        }

        if(currentWayPointID >= pathToFollow.path_objs.Count) {
            currentWayPointID = 0;
        }
	}
}
