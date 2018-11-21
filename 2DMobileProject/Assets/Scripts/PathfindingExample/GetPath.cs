using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPath : MonoBehaviour {

    public GameObject[] allPaths;

	// Use this for initialization
	void Start () {
        int num = Random.Range(0, allPaths.Length);
        transform.position = allPaths[num].transform.position;
        MoveOnPath yourPath = GetComponent<MoveOnPath>();
        yourPath.pathName = allPaths[num].name;
	}
	
}
