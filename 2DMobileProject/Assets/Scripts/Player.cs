using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    [SerializeField]
    private int _health;
    public int health { get { return _health; } set { _health = value; } }

    BoxCollider2D col;
    SpriteRenderer graphic;
    CharacterController charController;
    public Transform playerDeath;

    public event Action<Player> OnShootEvent;
    public event Action<Player> OnDeathEvent;
    public event Action<Player> OnHealthEvent;

    public Transform bulletPrefab;

    bool alive;
	// Use this for initialization
	void Start () {
        col = GetComponent<BoxCollider2D>();
        graphic= GetComponentInChildren<SpriteRenderer>();
        charController = GetComponent<CharacterController>();

        alive = true;
	}
	public void TakeDamage(int damage) {
        //UI stuff
        health -= damage;
    }
	// Update is called once per frame
	void Update () {
        CheckHealth();
        if (health <= 0) {
            Death();
        }
        Shoot();
       
	}
    void CheckHealth() {
        if(OnHealthEvent != null) {
            OnHealthEvent(this);
        }
    }
    void Shoot() {
        if (OnShootEvent != null)
            OnShootEvent(this);
    }
    void Death() {
        Transform clone = Instantiate(playerDeath, transform.position, transform.rotation);
        // Any death logic
        col.enabled = false;
        graphic.enabled = false;
        alive = false;
        if(OnDeathEvent != null) {
            OnDeathEvent(this);
        }
    }

    void Alive() {

    }
   

}
