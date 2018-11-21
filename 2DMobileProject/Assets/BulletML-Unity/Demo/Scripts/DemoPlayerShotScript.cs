// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;
using System.Collections;

namespace Pixelnest.BulletML.Demo
{
  public class DemoPlayerShotScript : MonoBehaviour
  {
    public Vector2 speed = Vector2.zero;

    private Rigidbody2D rbody2d;
    private SpriteRenderer sprite;

    void Awake()
    {
      rbody2d = GetComponent<Rigidbody2D>();
      sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
      // Destroy when outside the screen
      if (sprite != null && sprite.isVisible == false)
      {
        Destroy(this.gameObject);
      }
    }

    void FixedUpdate()
    {
      rbody2d.velocity = speed;
    }
  }
}