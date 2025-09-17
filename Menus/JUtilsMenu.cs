using UnityEditor;

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

    [MenuItem("JUtils/Translations/Fetch Translations")]
    public static async void FetchTranslationsMenu()
    {
        bool success = await JTranslations.FetchTranslations();

        string message = success
            ? "Translations fetched and saved successfully."
            : "An error occurred while fetching translations.";

        EditorUtility.DisplayDialog("Translations", message, "OK");
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
}