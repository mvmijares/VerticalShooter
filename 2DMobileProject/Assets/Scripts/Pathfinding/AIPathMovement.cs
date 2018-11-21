using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathMovement : MonoBehaviour {


    public EditorPath pathToFollow;

    public int currentWayPointID = 0;
    public float speed;
    [Tooltip("Distance from objects pivot point to waypoint")]
    public float reachDistance = 1.0f; 
    public string pathName;

    Vector3 last_position;
    Vector3 current_position;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
