using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SocialPlatforms;

public class HierarchyGUI
{
    #region --- VAR ---

    private const int MAX_ICON_NUM = 4;

    private static List<System.Type> HideTypes =
        new List<System.Type>() {typeof(Transform), typeof(ParticleSystemRenderer), typeof(CanvasRenderer),};

    private static Transform OffsetObject = null;
    private static int Offset = 0;

    #endregion

    #region --- MSG ---

    [InitializeOnLoadMethod]
    public static void Init()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    public static void OnHierarchyGUI(int instanceID, Rect rect)
    {
        // Check
        Object tempObj = EditorUtility.InstanceIDToObject(instanceID);
        if (!tempObj)
        {
            return;
        }

        // fix rect
        rect.width += rect.x;
        rect.x = 0;

        // Logic
        GameObject obj = tempObj as GameObject;
        // Debug.LogFormat("{0}:{1}", instanceID, obj.name);
        // if (obj.name == "Test")
        // {
            // int a = GetLocalIdentfierInFile(tempObj);
            // int b = GetLocalIdentfierInFile(obj);
            // Debug.Log(a);
            // Debug.Log(b);
        // }
        // List<Component> coms = new List<Component>(obj.GetComponents<Component>());
        // for (int i = 0; i < coms.Count; i++)
        // {
        //     if (!coms[i])
        //     {
        //         continue;
        //     }
        //
        //     if (TypeCheck(coms[i].GetType()))
        //     {
        //         coms.RemoveAt(i);
        //         i--;
        //     }
        // }
        //
        // int iconSize = 16;
        // int y = 1;
        // int offset = obj.transform == OffsetObject ? Offset : 0;
        //
        // // Main
        // for (int i = 0; i + offset < coms.Count && i < MAX_ICON_NUM; i++)
        // {
        //     Component com = coms[i + offset];
        //
        //     // Logic
        //     Texture2D texture = AssetPreview.GetMiniThumbnail(com);
        //
        //     if (texture)
        //     {
        //         GUI.DrawTexture(new Rect(rect.width - (iconSize + 1) * (i + 1), rect.y + y, iconSize, iconSize),
        //             texture);
        //     }
        // }
        //
        // // More Button
        // if (coms.Count == MAX_ICON_NUM + 1)
        // {
        //     Texture2D texture = AssetPreview.GetMiniThumbnail(coms[coms.Count - 1]);
        //     if (texture)
        //     {
        //         GUI.DrawTexture(
        //             new Rect(rect.width - (iconSize + 1) * (coms.Count - 1 + 1), rect.y + y, iconSize, iconSize),
        //             texture);
        //     }
        // }
        // else if (coms.Count > MAX_ICON_NUM)
        // {
        //     GUIStyle style = new GUIStyle(GUI.skin.label);
        //     style.fontSize = 9;
        //     style.alignment = TextAnchor.MiddleCenter;
        //
        //     if (GUI.Button(new Rect(rect.width - (iconSize + 2) * (MAX_ICON_NUM + 1), rect.y + y, 22, iconSize), "•••",
        //         style))
        //     {
        //         if (OffsetObject != obj.transform)
        //         {
        //             OffsetObject = obj.transform;
        //             Offset = 0;
        //         }
        //
        //         Offset += MAX_ICON_NUM;
        //         if (Offset >= coms.Count)
        //         {
        //             Offset = 0;
        //         }
        //     }
        // }
    }

    #endregion

    #region --- LGC ---
    
    private static int GetLocalIdentfierInFile(UnityEngine.Object obj)
    {
        PropertyInfo info =
            typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
        SerializedObject sObj = new SerializedObject(obj);
        info.SetValue(sObj, InspectorMode.Debug, null);
        SerializedProperty localIdProp = sObj.FindProperty("m_LocalIdentfierInFile");
        return localIdProp.intValue;
    }
    // private static bool TypeCheck(System.Type type)
    // {
    //     for (int i = 0; i < HideTypes.Count; i++)
    //     {
    //         if (type == HideTypes[i] || type.IsSubclassOf(HideTypes[i]))
    //         {
    //             return true;
    //         }
    //     }
    //
    //     return false;
    // }

    #endregion
}