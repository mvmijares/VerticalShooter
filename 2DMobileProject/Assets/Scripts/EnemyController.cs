using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

//Class handles enemy movement
public class EnemyController : MonoBehaviour {
    Enemy enemy; //reference to our enemy component
    Player player; // reference to player
    Transform target; //for path movement

    //Path traversing
    public List<Transform> path;
    public int currentWayPointID = 0;
    private float reachDistance = 0.1f;
    public float rotationSpeed = 0.5f;
    public string pathName;
   
    //enemy path behaviours
    bool hover;
    float currHoverTime;

    private void Start() {
        player = FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
    }
    private void Update() {
        if(path.Count > 0)
        Movement();
    }

    void Movement() {
        //check if waypoint has a specific behaviour
        //check if enemy can do specific behaviour
        //if not then continue with path
        float distance = Vector3.Distance(path[currentWayPointID].position, transform.position);
        Vector3 direction = Vector3.zero;

        if (distance <= reachDistance) {
            Waypoint node = path[currentWayPointID].GetComponent<Waypoint>();
            if (enemy.canHover && node.onHover) {
                OnHover();
            } else {
                if (currentWayPointID < path.Count) {
                    currentWayPointID++;
                    if (currentWayPointID >= path.Count) {
                        Destroy(this.gameObject);
                    } else
                        target = path[currentWayPointID];
                }
            }
        }

        //Will have more behaviours but this will suffice
        if (hover) {
            direction = Vector3.zero;
            enemy.OnHoverBehavior();
        } else
            direction = (target.position - transform.position).normalized;

        transform.Translate(direction * enemy.speed * Time.deltaTime);
    }

    //Will edit this for hovering
    void OnHover() {
        currHoverTime += Time.deltaTime;
        hover = true;
        if (currHoverTime >= enemy.hoverTime) {
            currHoverTime = 0.0f;
            hover = false;
            currentWayPointID++;
            target = path[currentWayPointID];
        }
    }
}
