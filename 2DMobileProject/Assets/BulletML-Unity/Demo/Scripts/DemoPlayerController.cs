using System.Collections;
// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;

namespace Pixelnest.BulletML.Demo
{
  /// <summary>
  /// Move the player ship with arrows
  /// </summary>
  public class DemoPlayerController : MonoBehaviour
  {
    public float speed = 30f;
    public float maxSpeed = 10f;

    public GameObject projectilePrefab;

    private Vector2 movement;
    private int damageTaken;
    private DemoFightScript demo;
    private Rigidbody2D rbody2d;

    void Awake()
    {
      damageTaken = 0;
      demo = FindObjectOfType<DemoFightScript>();

      rbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
      float inputX = Input.GetAxis("Horizontal");
      float inputY = Input.GetAxis("Vertical");

      movement = new Vector2(
        inputX * speed,
        inputY * speed
      );

      movement = new Vector2(
        Mathf.Clamp(movement.x, -maxSpeed, maxSpeed),
        Mathf.Clamp(movement.y, -maxSpeed, maxSpeed)
      );

      bool shoot = Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3");

      if (shoot)
      {
        // Create a new projectile
        Shoot();
      }
    }

    void FixedUpdate()
    {
      rbody2d.velocity = movement;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
      // Collision with projectile
      BulletScript bullet = otherCollider.GetComponent<BulletScript>();

      if (bullet != null)
      {
        damageTaken++;

        // Flash red
        StartCoroutine(FlashRed());

        Destroy(bullet.gameObject);
      }
    }

    void OnGUI()
    {
      if (demo.showGUI)
      {
        GUI.Label(new Rect(5, 5, 150, 50), "Player damages: " + damageTaken);
      }
    }

    private void Shoot()
    {
      GameObject shot = Instantiate(projectilePrefab) as GameObject;
      shot.transform.position = this.transform.position;

      DemoPlayerShotScript shotScript = shot.GetComponent<DemoPlayerShotScript>();
      shotScript.speed = new Vector2(25, 0);
    }

    private IEnumerator FlashRed()
    {
      SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

      sprite.color = Color.red;

      yield return new WaitForSeconds(0.05f);

      sprite.color = Color.white;

      yield return null;
    }
  }
}