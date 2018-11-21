using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPath : MonoBehaviour {

    public Color rayColor = Color.white;
    public List<Transform> path = new List<Transform>(); // our list of wayPoints create a path
    Transform[] wayPoints; // waypoints are stored as children of this object

    private void OnDrawGizmos() {
        Gizmos.color = rayColor;

        wayPoints = GetComponentsInChildren<Transform>();
        path.Clear();

        foreach(Transform wayPoint in wayPoints) {
            if(wayPoint != this.transform) {
                path.Add(wayPoint);
            }
        }

        for(int i = 0; i < path.Count; i++) {
            Vector3 position = path[i].position;
            if(i > 0) {
                Vector3 previous = path[i - 1].position;
                Gizmos.DrawLine(previous, position);
                Gizmos.DrawWireSphere(position, 0.2f);
            }
        }
    }
}
