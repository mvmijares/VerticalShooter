#if UNITY_EDITOR
// Copyright © 2014 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Pixelnest
{
  /// <summary>
  /// ScriptableObject helper
  /// </summary>
  /// <remarks>Source: http://www.jacobpennock.com/Blog/?page_id=715 </remarks>
  public static class ScriptableObjectUtility
  {
    public static void CreateAsset<T>() where T : ScriptableObject
    {
      T asset = ScriptableObject.CreateInstance<T>();

      string path = AssetDatabase.GetAssetPath(Selection.activeObject);
      if (path == "")
      {
        path = "Assets";
      }
      else if (Path.GetExtension(path) != "")
      {
        path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
      }

      string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

      AssetDatabase.CreateAsset(asset, assetPathAndName);

      AssetDatabase.SaveAssets();
      EditorUtility.FocusProjectWindow();
      Selection.activeObject = asset;
    }
  }
}
#endif