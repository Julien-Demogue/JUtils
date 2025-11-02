using TMPro;
using UnityEditor;
using UnityEngine;

public class JUtilsMenu
{
    [MenuItem("JUtils/Cleaning/Clear User Data")]
    public static void ClearUserDataMenu()
    {
        JFileSystem.ClearUserData();
    }

    [MenuItem("JUtils/Cleaning/Clear Game Settings")]
    public static void ClearGameSettingsMenu()
    {
        JFileSystem.ClearGameSettings();
    }

    [MenuItem("JUtils/Cleaning/Clear All Logs")]
    public static void ClearLogsMenu()
    {
        JFileSystem.ClearLogs();
    }

    [MenuItem("JUtils/Cleaning/Clear All Data")]
    public static void ClearAllDataMenu()
    {
        JFileSystem.ClearAllData();
    }

    [MenuItem("JUtils/Data/Open User Data File")]
    public static void OpenUserDataFile()
    {
        string userDataPath = JFileSystem.GetDataFilePath(JFileSystem.USER_DATA_FILE);
        if (System.IO.File.Exists(userDataPath))
        {
            System.Diagnostics.Process.Start(userDataPath);
        }
        else
        {
            EditorUtility.DisplayDialog("User Data", "User data file not found.", "OK");
        }
    }

    [MenuItem("JUtils/Data/Open Game Settings File")]
    public static void OpenGameSettingsFile()
    {
        string gameSettingsPath = JFileSystem.GetDataFilePath(JFileSystem.GAME_SETTINGS_FILE);
        if (System.IO.File.Exists(gameSettingsPath))
        {
            System.Diagnostics.Process.Start(gameSettingsPath);
        }
        else
        {
            EditorUtility.DisplayDialog("Game Settings", "Game settings file not found.", "OK");
        }
    }

    [MenuItem("JUtils/Log/Open Game Log File")]
    public static void OpenGameLogFile()
    {
        string gameLogPath = JFileSystem.GetLogFilePath(JFileSystem.GAME_LOG_FILE);
        if (System.IO.File.Exists(gameLogPath))
        {
            System.Diagnostics.Process.Start(gameLogPath);
        }
        else
        {
            EditorUtility.DisplayDialog("Game Log", "Log file not found.", "OK");
        }
    }

    [MenuItem("JUtils/Log/Open Error Log File")]
    public static void OpenErrorLogFile()
    {
        string errorLogPath = JFileSystem.GetLogFilePath(JFileSystem.ERROR_LOG_FILE);
        if (System.IO.File.Exists(errorLogPath))
        {
            System.Diagnostics.Process.Start(errorLogPath);
        }
        else
        {
            EditorUtility.DisplayDialog("Error Log", "Log file not found.", "OK");
        }
    }

    [MenuItem("JUtils/Audio/Compile audio elements")]
    public static void CompileAudioItemsMenu()
    {
        JAudioEditor.CompileAudioItems();
        EditorUtility.DisplayDialog("Audio Compilation", "Audio items compiled successfully.", "OK");
    }

    [MenuItem("JUtils/Audio/Refresh Audio Settings")]
    public static void RefreshAudioSettingsMenu()
    {
        JAudioEditor.RefreshAudioSettings();
        EditorUtility.DisplayDialog("Audio Settings", "Audio settings refreshed successfully.", "OK");
    }

    [MenuItem("JUtils/Translations/Fetch Translations")]
    public static async void FetchTranslationsMenu()
    {
        bool success = await JTranslations.FetchTranslations();

        string message = success
            ? "Translations fetched and saved successfully."
            : "An error occurred while fetching translations.";

        EditorUtility.DisplayDialog("Translations", message, "OK");
    }

    [MenuItem("DIY/Translations/Convert Selected to JLocalizedText")]
    public static void ConvertSelectedToJLocalizedText()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("No Selection", "Please select at least one GameObject.", "OK");
            return;
        }

        int convertedCount = 0;
        foreach (GameObject obj in selectedObjects)
        {
            TextMeshProUGUI[] tmpComponents = obj.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI tmp in tmpComponents)
            {
                if (tmp is JLocalizedText) continue;

                if (ConvertComponent(tmp))
                {
                    convertedCount++;
                }
            }
        }

        EditorUtility.DisplayDialog("Conversion Complete",
            $"Converted {convertedCount} TextMeshProUGUI component(s) to JLocalizedText.", "OK");
    }

    [MenuItem("DIY/Translations/Convert All in Scene to JLocalizedText")]
    public static void ConvertAllInSceneToJLocalizedText()
    {
        if (!EditorUtility.DisplayDialog("Convert All in Scene",
            "This will convert all TextMeshProUGUI components in the active scene to JLocalizedText. Continue?",
            "Yes", "Cancel"))
        {
            return;
        }

        TextMeshProUGUI[] allTMP = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int convertedCount = 0;

        foreach (TextMeshProUGUI tmp in allTMP)
        {
            if (tmp is JLocalizedText) continue;

            if (ConvertComponent(tmp))
            {
                convertedCount++;
            }
        }

        EditorUtility.DisplayDialog("Conversion Complete",
            $"Converted {convertedCount} TextMeshProUGUI component(s) to JLocalizedText in the active scene.", "OK");
    }

    [MenuItem("DIY/Translations/Convert All Prefabs to JLocalizedText")]
    public static void ConvertAllPrefabsToJLocalizedText()
    {
        if (!EditorUtility.DisplayDialog("Convert All Prefabs",
            "This will convert all TextMeshProUGUI components in all prefabs. This may take a while. Continue?",
            "Yes", "Cancel"))
        {
            return;
        }

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        int convertedCount = 0;
        int totalPrefabs = prefabGuids.Length;
        int currentPrefab = 0;

        foreach (string guid in prefabGuids)
        {
            currentPrefab++;
            string path = AssetDatabase.GUIDToAssetPath(guid);

            EditorUtility.DisplayProgressBar("Converting Prefabs",
                $"Processing {currentPrefab}/{totalPrefabs}: {path}",
                (float)currentPrefab / totalPrefabs);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            TextMeshProUGUI[] tmpComponents = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
            bool prefabModified = false;

            foreach (TextMeshProUGUI tmp in tmpComponents)
            {
                if (tmp is JLocalizedText) continue;

                if (ConvertComponentInPrefab(tmp, path))
                {
                    convertedCount++;
                    prefabModified = true;
                }
            }

            if (prefabModified)
            {
                AssetDatabase.SaveAssets();
            }
        }

        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("Conversion Complete",
            $"Converted {convertedCount} TextMeshProUGUI component(s) in {totalPrefabs} prefab(s).", "OK");
    }

    private static bool ConvertComponent(TextMeshProUGUI original)
    {
        if (original == null) return false;

        GameObject obj = original.gameObject;

        CopyTextMeshProProperties(original, out var properties);

        Undo.DestroyObjectImmediate(original);

        JLocalizedText newComponent = Undo.AddComponent<JLocalizedText>(obj);

        ApplyTextMeshProProperties(newComponent, properties);

        EditorUtility.SetDirty(obj);
        return true;
    }

    private static bool ConvertComponentInPrefab(TextMeshProUGUI original, string prefabPath)
    {
        if (original == null) return false;

        GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

        TextMeshProUGUI[] tmpComponents = prefabRoot.GetComponentsInChildren<TextMeshProUGUI>(true);
        TextMeshProUGUI targetComponent = null;

        foreach (var tmp in tmpComponents)
        {
            if (tmp.gameObject.name == original.gameObject.name && tmp.text == original.text)
            {
                targetComponent = tmp;
                break;
            }
        }

        if (targetComponent == null)
        {
            PrefabUtility.UnloadPrefabContents(prefabRoot);
            return false;
        }

        GameObject obj = targetComponent.gameObject;

        CopyTextMeshProProperties(targetComponent, out var properties);
        Object.DestroyImmediate(targetComponent);
        JLocalizedText newComponent = obj.AddComponent<JLocalizedText>();
        ApplyTextMeshProProperties(newComponent, properties);

        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
        PrefabUtility.UnloadPrefabContents(prefabRoot);

        return true;
    }

    private static void CopyTextMeshProProperties(TextMeshProUGUI source, out TMPProperties properties)
    {
        properties = new TMPProperties
        {
            text = source.text,
            font = source.font,
            fontSize = source.fontSize,
            fontStyle = source.fontStyle,
            color = source.color,
            alignment = source.alignment,
            characterSpacing = source.characterSpacing,
            wordSpacing = source.wordSpacing,
            lineSpacing = source.lineSpacing,
            paragraphSpacing = source.paragraphSpacing,
            textWrappingMode = source.textWrappingMode,
            overflowMode = source.overflowMode,
            margin = source.margin,
            raycastTarget = source.raycastTarget,
            maskable = source.maskable,
            enableAutoSizing = source.enableAutoSizing,
            fontSizeMin = source.fontSizeMin,
            fontSizeMax = source.fontSizeMax,
            richText = source.richText
        };
    }

    private static void ApplyTextMeshProProperties(TextMeshProUGUI target, TMPProperties properties)
    {
        target.text = properties.text;
        target.font = properties.font;
        target.fontSize = properties.fontSize;
        target.fontStyle = properties.fontStyle;
        target.color = properties.color;
        target.alignment = properties.alignment;
        target.characterSpacing = properties.characterSpacing;
        target.wordSpacing = properties.wordSpacing;
        target.lineSpacing = properties.lineSpacing;
        target.paragraphSpacing = properties.paragraphSpacing;
        target.textWrappingMode = properties.textWrappingMode;
        target.overflowMode = properties.overflowMode;
        target.margin = properties.margin;
        target.raycastTarget = properties.raycastTarget;
        target.maskable = properties.maskable;
        target.enableAutoSizing = properties.enableAutoSizing;
        target.fontSizeMin = properties.fontSizeMin;
        target.fontSizeMax = properties.fontSizeMax;
        target.richText = properties.richText;
    }

    private struct TMPProperties
    {
        public string text;
        public TMP_FontAsset font;
        public float fontSize;
        public FontStyles fontStyle;
        public Color color;
        public TextAlignmentOptions alignment;
        public float characterSpacing;
        public float wordSpacing;
        public float lineSpacing;
        public float paragraphSpacing;
        public TextWrappingModes textWrappingMode;
        public TextOverflowModes overflowMode;
        public Vector4 margin;
        public bool raycastTarget;
        public bool maskable;
        public bool enableAutoSizing;
        public float fontSizeMin;
        public float fontSizeMax;
        public bool richText;
    }
}