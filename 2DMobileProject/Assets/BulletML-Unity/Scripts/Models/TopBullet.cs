// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;

namespace Pixelnest.BulletML
{
  /// <summary>
  /// Root node of a bullet pattern
  /// </summary>
  public class TopBullet : BulletMLLib.Bullet
  {
    /// <summary>
    /// Position, updated by BulletML
    /// </summary>
    public Vector2 position;

    /// <summary>
    /// New top bullet
    /// </summary>
    /// <param name="bulletManager"></param>
    public TopBullet(BulletMLLib.IBulletManager bulletManager, BulletSourceScript parent)
      : base(bulletManager)
    {
      this.position = Vector2.zero;
      this.Parent = parent;
    }

    public override void InitBullet()
    {
    }

    /// <summary>
    /// Parent script that should be handling this object.
    /// </summary>
    public BulletSourceScript Parent { get; private set; }

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