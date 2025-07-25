#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class RaycastDisabler : EditorWindow
{
    [MenuItem("Tools/RaycastDisabler")]
    private static void ShowWindow()
    {
        var window = GetWindow<RaycastDisabler>();
        window.titleContent = new GUIContent("Disable Raycast on UI Components");
        window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Disable Raycast Target on UI Components"))
        {
            DisableRaycastOnUIComponents();
        }
    }

    private static void DisableRaycastOnUIComponents()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            ProcessGameObject(obj);
        }
    }

    private static void ProcessGameObject(GameObject obj)
    {
        if (obj.GetComponent<Button>() == null)
        {
            Image image = obj.GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = false;
            }
            
            TextMeshProUGUI tmpText = obj.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.raycastTarget = false;
            }
        }
        
        foreach (Transform child in obj.transform)
        {
            ProcessGameObject(child.gameObject);
        }
    }
}
#endif