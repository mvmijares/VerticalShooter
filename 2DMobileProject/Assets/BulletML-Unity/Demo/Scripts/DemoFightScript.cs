// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;
using System.Linq;

namespace Pixelnest.BulletML.Demo
{
  /// <summary>
  /// Little boss fight
  /// </summary>
  public class DemoFightScript : MonoBehaviour
  {
    public bool showGUI = true;

    void Awake()
    {
    }

    void OnGUI()
    {
      if (showGUI)
      {
        // Stats
        GUI.Label(new Rect(Screen.width - 75, 0, 150, 20), (1.0f / Time.deltaTime).ToString("00") + " FPS");
        GUI.Label(new Rect(Screen.width - 75, 20, 150, 20), (FindObjectsOfType<BulletScript>().Length + " bullets"));

#if !UNITY_EDITOR
        if (GUI.Button(new Rect(5, Screen.height - 30, 200, 25), "Back to the showcase"))
        {
          Application.LoadLevel("Demo_Showcase");
        }
#endif
      }
    }

  }
}