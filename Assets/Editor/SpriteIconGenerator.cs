using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteIconGenerator : EditorWindow
{
    public enum ImageSize
    {
        Size_256 = 256,
        Size_512 = 512,
        Size_1024 = 1024,
        Size_2048 = 2048
    }

    public Camera selectedCamera;
    public ImageSize imageSize = ImageSize.Size_512;
    public string savePath = "Assets";
    private RenderTexture renderTexture;
    private Texture2D previewTexture;

    [MenuItem("Tools/Sprite Icon Generator")]
    public static void ShowWindow()
    {
        SpriteIconGenerator window = GetWindow<SpriteIconGenerator>("Sprite Icon Generator");
        window.Show();
    }

    void OnGUI()
    {
        selectedCamera = (Camera)EditorGUILayout.ObjectField("Render Camera", selectedCamera, typeof(Camera), true);
        imageSize = (ImageSize)EditorGUILayout.EnumPopup("Image Size", imageSize);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Generate Preview"))
        {
            GeneratePreview();
        }

        if (previewTexture != null)
        {
            GUILayout.Label("Preview:");
            GUILayout.Label(new GUIContent(previewTexture), GUILayout.Width(256), GUILayout.Height(256));
        }

        if (GUILayout.Button("Save Icon"))
        {
            SaveIcon();
        }
    }

    void OnDisable()
    {
        CleanupResources();
    }

    void CleanupResources()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            DestroyImmediate(renderTexture);
            renderTexture = null;
        }
        if (previewTexture != null)
        {
            DestroyImmediate(previewTexture);
            previewTexture = null;
        }
    }

    void GeneratePreview()
    {
        if (selectedCamera == null)
        {
            Debug.LogError("No Camera assigned!");
            return;
        }

        int imageWidth = (int)imageSize;
        int imageHeight = (int)imageSize;

        CleanupResources();

        renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        selectedCamera.targetTexture = renderTexture;
        selectedCamera.Render();

        RenderTexture.active = renderTexture;
        previewTexture = new Texture2D(imageWidth, imageHeight, TextureFormat.ARGB32, false);
        previewTexture.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        previewTexture.Apply();

        selectedCamera.targetTexture = null;
        RenderTexture.active = null;
    }

    void SaveIcon()
    {
        if (previewTexture == null)
        {
            Debug.LogError("No preview generated!");
            return;
        }

        GameObject targetObject = GetMainObjectInView();
        if (targetObject == null)
        {
            Debug.LogError("No object detected in camera view!");
            return;
        }

        string fileName = targetObject.name + ".png";
        byte[] bytes = previewTexture.EncodeToPNG();
        string fullPath = Path.Combine(savePath, fileName);
        File.WriteAllBytes(fullPath, bytes);

        Debug.Log("Icon saved to: " + fullPath);
    }

    GameObject GetMainObjectInView()
    {
        if (selectedCamera == null)
            return null;

        RaycastHit hit;
        if (Physics.Raycast(selectedCamera.transform.position, selectedCamera.transform.forward, out hit, Mathf.Infinity))
        {
            return hit.collider != null ? hit.collider.gameObject : null;
        }
        return null;
    }
}
