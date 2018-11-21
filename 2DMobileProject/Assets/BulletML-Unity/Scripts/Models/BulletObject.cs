// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;

namespace Pixelnest.BulletML
{
  /// <summary>
  /// Extented BulletML bullet object
  /// </summary>
  public class BulletObject : BulletMLLib.Bullet
  {
    /// <summary>
    /// Raise event on creation
    /// </summary>
    public event System.Action<BulletObject, string> OnBulletSpawned;

    /// <summary>
    /// Position, updated by BulletML
    /// </summary>
    public Vector2 position;

    /// <summary>
    /// New bullet creation
    /// </summary>
    /// <param name="bulletManager"></param>
    public BulletObject(BulletMLLib.IBulletManager bulletManager, GameObject parent)
      : base(bulletManager)
    {
      this.Parent = parent;
    }

    /// <summary>
    /// Bullet spawn initialization
    /// </summary>
    public override void InitBullet()
    {
      // Raise event
      if (OnBulletSpawned != null) OnBulletSpawned(this, this.Label);
    }

    /// <summary>
    /// Parent GameObject.
    /// </summary>
    public GameObject Parent { get; set; }

    /// <summary>
    /// Position X
    /// </summary>
    public override float X
    {
      get
      {
        return position.x;
      }
      set
      {
        position.x = value;
      }
    }

    /// <summary>
    /// Position Y
    /// </summary>
    public override float Y
    {
      get
      {
        return position.y;
      }
      set
      {
        position.y = value;
      }
    }
  }
}