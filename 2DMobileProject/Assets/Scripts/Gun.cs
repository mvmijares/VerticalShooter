using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    //Will edit this for a generic gun class
    Player player;
    float bulletTimer;
    public float bulletDelay;
    private bool _canFire;
    public bool canFire { get { return _canFire; } set { _canFire = value; } }

    private void Awake() {
        _canFire = true;
        player = FindObjectOfType<Player>();
    }
    private void OnEnable() {
        player.OnShootEvent += ShootEvent;
        player.OnDeathEvent += DeathEvent;
    }
    private void OnDisable() {
        player.OnShootEvent -= ShootEvent;
        player.OnDeathEvent -= DeathEvent;
    }

    void ShootEvent(Player player) {
        if (_canFire) {
            bulletTimer += Time.deltaTime;
            if (bulletTimer > bulletDelay) {
                Transform clone = Instantiate(player.bulletPrefab,transform.position, transform.rotation);
                clone.GetComponent<Bullet>().direction = transform.up;
                clone.GetComponent<Bullet>().damage = 5;
                bulletTimer = 0.0f;
            }
        } else {
            bulletTimer = 0.0f;
        }
    }
    void DeathEvent(Player player) {
        _canFire = false;
    }
}
