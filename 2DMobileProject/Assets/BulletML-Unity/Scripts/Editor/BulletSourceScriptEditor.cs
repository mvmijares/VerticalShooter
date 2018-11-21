#if UNITY_EDITOR
// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEditor;
using UnityEngine;

namespace Pixelnest.BulletML
{
  /// <summary>
  /// Editor for bullet patterns
  /// </summary>
  [CustomEditor(typeof(BulletSourceScript))]
  [CanEditMultipleObjects]
  public class BulletSourceScriptEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      // Add a button to reload the file
      if (Application.isPlaying)
      {
        if (GUILayout.Button("Reload pattern" + (targets.Length > 1 ? "s" : "")))
        {
          foreach (var target in targets)
          {
            BulletSourceScript pattern = (BulletSourceScript)target;

            if (pattern.xmlFile != null && Application.isPlaying)
            {
              Debug.Log("Reloading pattern " + pattern.xmlFile.name + " for " + pattern.gameObject);
              pattern.ParsePattern(true);
              pattern.Initialize();
            }
          }
        }
      }
    }
  }
}
#endif
