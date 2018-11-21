using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

//Class handles enemy behavior
public class Enemy : MonoBehaviour {
    Player player; // reference to player
    [SerializeField]
    EnemyController enemyController; //Reference to our controller
    [Tooltip("A number between 1 and 10.")]
    public int enemyType;

    [SerializeField]
    int health;
    public float speed;
    public float distance;

    //Information we need from Enemy
    public int score;
    public bool canHover;
    public float hoverTime;

    Transform sprite;
    public Transform projectile;
    public Transform deathExplosion;

    public float fireDelay;
    bool canFire;
    bool fire;
    float fireTimer;

    bool canMove;

    public event Action<Enemy> OnDeathEvent;

    GameObject bulletSource;
    private void Awake() {
        enemyController = GetComponent<EnemyController>();
    }
    void Start () {
        health = 30;
        canMove = true;
        fire = false;
        canFire = true;
        player = FindObjectOfType<Player>();
        sprite = transform.GetChild(0); // need a better way to grab sprite
        enemyType = Mathf.Clamp(enemyType, 1, 10);


        if(enemyType > 1) {
            bulletSource = GetComponentInChildren<Pixelnest.BulletML.BulletSourceScript>().gameObject;
            bulletSource.SetActive(false);
        }
	}
	// Update is called once per frame
	void Update () {
        if (player.health > 0 ) {
            Shoot();
            FacePlayer();
        }
		if(health <= 0) {
            Transform clone = Instantiate(deathExplosion, transform.position, transform.rotation);

            if (OnDeathEvent != null)
                OnDeathEvent(GetComponent<Enemy>());

            Destroy(this.gameObject);
        }
	}
    public void OnHoverBehavior() {
        bulletSource.SetActive(true);
    }
    public void SetPath(List<Transform> path) {
        enemyController.path = path;
    }

    void Shoot() {
        if (canFire && fire) {
            Transform clone = Instantiate(projectile, transform.position, transform.rotation);
            Vector2 targetDir = (player.transform.position - clone.position).normalized;
            clone.GetComponent<EnemyBullet>().direction = targetDir;
            fire = false;
            fireTimer = 0.0f;
        }
        if (canFire && !fire) {
            fireTimer += Time.deltaTime;
            if (fireTimer > fireDelay) {
                fire = true;
            }
        }
    }
    //Sprite behaviour
    void FacePlayer() {
        Vector2 direction = (player.transform.position - sprite.position).normalized;
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sprite.eulerAngles = new Vector3(0, 0, rotation);
    }
    public void TakeDamage(int damage) {
        health -= damage;
    }
}
