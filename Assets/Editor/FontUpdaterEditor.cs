using UnityEditor;
using TMPro;
using UnityEngine;

public class FontUpdaterEditor : EditorWindow
{
    public TMP_FontAsset newFontAsset;

    [MenuItem("Tools/Update All TextMeshPro Fonts")]
    public static void ShowWindow()
    {
        GetWindow<FontUpdaterEditor>("Update TMP Fonts");
    }

    private void OnGUI()
    {
        GUILayout.Label("Update All TextMeshPro Fonts", EditorStyles.boldLabel);
        newFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font Asset", newFontAsset, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Update Fonts"))
        {
            UpdateAllTextMeshProFonts();
        }
    }

    private void UpdateAllTextMeshProFonts()
    {
        TMP_Text[] allTextMeshPro = GameObject.FindObjectsOfType<TMP_Text>(true);

        foreach (TMP_Text text in allTextMeshPro)
        {
            text.font = newFontAsset;
            text.fontSharedMaterial = newFontAsset.material; 
            text.SetAllDirty();
        }

        Debug.Log("Updated all TextMeshPro fonts to: " + newFontAsset.name);
    }
}
