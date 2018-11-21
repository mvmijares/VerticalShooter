// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System.Collections.Generic;
using UnityEngine;

namespace Pixelnest.BulletML
{
  /// <summary>
  /// Define a bullet with the prefab to instantiate and the data to use
  /// </summary>
  [System.Serializable]
  public class BulletBankEntry
  {
    /// <summary>
    /// Bullet name. Should match the XML definition.
    /// </summary>
    public string name;
    
    /// <summary>
    /// Unity prefab
    /// </summary>
    public GameObject prefab;

    /// <summary>
    /// Optional sprite, if you want to replace it dynamically
    /// </summary>
    public Sprite sprite;

    /// <summary>
    /// Time, in seconds, after which the bullet will be destroyed.
    /// </summary>
    public float TimeToLiveInSeconds = 10f;

    /// <summary>
    /// If the bullet is not visible, destroy it
    /// </summary>
    public bool DestroyWhenOutOfScreen = true;
  }

  /// <summary>
  /// List of serialiezd bullet definition 
  /// </summary>
  public class BulletBank : ScriptableObject
  {
    /// <summary>
    /// Bullet definitions
    /// </summary>
    public List<BulletBankEntry> bullets;
  }
}