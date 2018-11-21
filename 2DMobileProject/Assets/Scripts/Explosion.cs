using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Explosion : MonoBehaviour {

    Animator anim;
    public Text text;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Explosion") && 
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            Destroy(this.gameObject);
        }
    }
}
