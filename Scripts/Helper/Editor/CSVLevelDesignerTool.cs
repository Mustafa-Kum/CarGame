using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using _Game.Scripts;

public class CSVLevelDesignerTool : EditorWindow
{
    private float levelTime = 30f; // Total level time in seconds
    private float characterSpeed = 4f; // Character's speed
    private string csvFilePath = "Assets/level_designs.csv";
    private string prefabsBasePath = "Assets/_Game/Prefabs/_InGame/";
    private string splineLevelPath = "Assets/_Game/Prefabs/_InGame/Level/Ref_SplineLevel.prefab"; // Path to the SplineLevel prefab

    [MenuItem("Tools/CSV Level Designer Tool")]
    public static void ShowWindow()
    {
        GetWindow<CSVLevelDesignerTool>("CSV Level Designer Tool");
    }

    void OnGUI()
    {
        GUILayout.Label("CSV Level Designer Tool", EditorStyles.boldLabel);
        levelTime = EditorGUILayout.FloatField("Level Time (Seconds)", levelTime);
        characterSpeed = EditorGUILayout.FloatField("Character Speed", characterSpeed);

        if (GUILayout.Button("Create Level From CSV"))
        {
            LoadLevel();
        }
    }

    private void LoadLevel()
    {
        GameObject splineLevelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(splineLevelPath);
        if (splineLevelPrefab != null)
        {
            PrefabUtility.InstantiatePrefab(splineLevelPrefab);
        }
        else
        {
            Debug.LogError("Spline Level prefab not found at path: " + splineLevelPath);
            return;
        }

        string[] lines = File.ReadAllLines(csvFilePath);
        var levelLines = lines.Skip(1); // Skip header line

        float levelDistance = levelTime * characterSpeed; // Total distance of the level
        float defaultTimeIncrement = levelTime / levelLines.Count(); // Default time increment for each object
        float currentTime = 0f; // Initialize current time

        foreach (var line in levelLines)
        {
            var cells = line.Split(',');
            if (cells.Length < 2)
            {
                Debug.LogError($"Invalid line in CSV: {line}");
                continue; // Skip this line if it doesn't have enough data
            }

            string position = cells[0].Trim();
            string prefabName = cells[1].Trim();
            string modifier = cells.Length > 2 ? cells[2].Trim() : null;
            float time;

            if (cells.Length >= 3 && float.TryParse(cells[2].Trim(), out float parsedTime))
            {
                time = parsedTime;
            }
            else
            {
                time = currentTime; // Use default time increment
                currentTime += defaultTimeIncrement;
            }

            GameObject prefab = FindPrefabInDirectory(prefabsBasePath, prefabName);
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found: {prefabName}");
                continue;
            }

            GameObject instantiatedObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            float zPosition = time / levelTime * levelDistance; // Calculate Z position based on time
            Vector3 instantiatePosition = new Vector3(GetXPosition(position), 0, zPosition);

            if (prefabName == "Grill_Machine")
            {
                instantiatePosition.x = -3.75f;
                instantiatePosition.y = 0.6f;
            }

            instantiatedObject.transform.position = instantiatePosition;

            if (modifier != null && instantiatedObject.GetComponentInChildren<ILevelObject>() != null)
            {
                ApplyModifierToGate(instantiatedObject, modifier);
            }
        }
    }

    private void ApplyModifierToGate(GameObject gateObject, string modifier)
    {
        var levelObject = gateObject.GetComponentInChildren<ILevelObject>();
        if (levelObject != null)
        {
            levelObject.ApplyModifier(modifier);
        }
    }

    private GameObject FindPrefabInDirectory(string basePath, string prefabName)
    {
        var files = Directory.GetFiles(basePath, prefabName + ".prefab", SearchOption.AllDirectories);
        if (files.Length == 0) return null;
        return AssetDatabase.LoadAssetAtPath<GameObject>(files[0]);
    }

    private float GetXPosition(string position)
    {
        switch (position)
        {
            case "Left":
                return -1f;
            case "Middle":
                return 0f;
            case "Right":
                return 1f;
            default:
                Debug.LogError("Invalid position: " + position);
                return 0f;
        }
    }
}
