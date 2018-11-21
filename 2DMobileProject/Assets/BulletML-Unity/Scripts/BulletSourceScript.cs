// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BulletMLLib;

namespace Pixelnest.BulletML
{
  /// <summary>
  /// Projectile launcher.
  /// </summary>
#if UNITY_EDITOR
  [AddComponentMenu("BulletML/Bullet Source")]
#endif
  public class BulletSourceScript : MonoBehaviour
  {
    private static Dictionary<TextAsset, BulletMLLib.BulletPattern> patternCache = new Dictionary<TextAsset, BulletMLLib.BulletPattern>();

    /// <summary>
    /// The XML pattern we want to use.
    /// </summary>
    public TextAsset xmlFile;

    private TextAsset currentXmlFile;

    private TopBullet rootBullet;
    private BulletMLLib.BulletPattern pattern;
    private BulletMLLib.IBulletManager bulletManager;

    void Start()
    {
      // Note: we use start and not Awake so the BulletManager has some time to initialiez BulletML properly.

      // Find the manager
      bulletManager = FindObjectOfType<BulletManagerScript>();
      if (bulletManager == null)
      {
        throw new System.Exception("Cannot find a BulletManagerScript in the scene!");
      }

      // Parse pattern
      if (xmlFile == null)
      {
        throw new System.Exception("No pattern (Xml File) assigned to the emitter.");
      }

      ParsePattern(false);

      Initialize();
    }

    /// <summary>
    /// Force the XML file to be reloaded
    /// </summary>
    /// <param name="forceCacheReload"></param>
    public void ParsePattern(bool forceCacheReload)
    {
      pattern = LoadPattern(xmlFile, forceCacheReload);
      currentXmlFile = xmlFile;
    }

    /// <summary>
    /// Setup the emitter with the current pattern
    /// </summary>
    public void Initialize()
    {
      if (rootBullet != null)
      {
        bulletManager.RemoveBullet(rootBullet);
        rootBullet = null;
      }

      rootBullet = new TopBullet(bulletManager, this);
      rootBullet.X = this.transform.position.x;
      rootBullet.Y = this.transform.position.y;
      rootBullet.InitTopNode(pattern.RootNode);
    }

    void Update()
    {
      // Changing source?
      if (currentXmlFile != xmlFile)
      {
        pattern = LoadPattern(xmlFile);
        currentXmlFile = xmlFile;

        Initialize();
      }

      rootBullet.X = transform.position.x;
      rootBullet.Y = transform.position.y;
      rootBullet.Update();
    }

    /// <summary>
    /// Restart the whole pattern
    /// </summary>
    public void Reset()
    {
      if (rootBullet != null)
      {
        foreach (var task in rootBullet.Tasks)
        {
          task.HardReset(rootBullet);
        }
      }
    }

    /// <summary>
    /// The pattern is ended
    /// </summary>
    public bool IsEnded
    {
      get
      {
        if (rootBullet == null)
        {
          return false;
        }

        bool ended = true;
        foreach (var t in rootBullet.Tasks)
        {
          ended &= t.TaskFinished;
        }
        return ended;
      }
    }

    /// <summary>
    /// Load the pattern and store it in cache
    /// </summary>
    public static BulletPattern LoadPattern(TextAsset xmlFile)
    {
      return LoadPattern(xmlFile, false);
    }

    /// <summary>
    /// Load the pattern if necessary and store it in cache
    /// </summary>
    public static BulletPattern LoadPattern(TextAsset xmlFile, bool reloadCacheToo)
    {
      BulletPattern loadedPattern = null;

      // Cache the pattern to avoid reparsing everytime
      if (reloadCacheToo || patternCache.TryGetValue(xmlFile, out loadedPattern) == false)
      {
        System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(new System.IO.StringReader(xmlFile.text));
        reader.Normalization = false;
        reader.XmlResolver = null;

        loadedPattern = new BulletMLLib.BulletPattern();
        loadedPattern.ParseXML(xmlFile.name, reader);

        if (patternCache.ContainsKey(xmlFile))
        {
          patternCache[xmlFile] = loadedPattern;
        }
        else
        {
          patternCache.Add(xmlFile, loadedPattern);
        }
      }

      return loadedPattern;
    }
  }
}