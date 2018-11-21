using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

    public Transform player;
    public Transform fingerTouch;

    

	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            switch (touch.phase) {
                case TouchPhase.Began:
                    fingerTouch.position = new Vector2(touchPos.x, touchPos.y);
                    player.parent = fingerTouch;
                    break;
                case TouchPhase.Moved:
                    fingerTouch.position = new Vector2(touchPos.x, touchPos.y);
                    break;
                case TouchPhase.Ended:
                    player.parent = null;
                    break;
            }
        } else if (Input.touchCount == 0) { // Unity Editor Testing purposes
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0)) {
                fingerTouch.position = new Vector2(mousePos.x, mousePos.y);
                player.parent = fingerTouch;
            }
            if (Input.GetMouseButton(0)) {
          
                fingerTouch.position = new Vector2(mousePos.x, mousePos.y);
            }
            if (Input.GetMouseButtonUp(0)) {
                player.parent = null;
            }
        }
	}
}
