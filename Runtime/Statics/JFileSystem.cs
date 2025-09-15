using UnityEngine;
using System.IO;

/// <summary>
/// JFileSystem manages saving, loading, and deleting data.
/// Also handles log management (add, delete).
/// All data is stored in the application's persistent folder.
/// </summary>
public class JFileSystem
{
    private const string DATA_PATH = "DATA";
    private const string LOG_PATH = "LOGS";

    public const string USER_DATA_FILE = "USER_DATA";
    public const string GAME_SETTINGS_FILE = "SETTINGS";
    public const string GAME_LOG_FILE = "game.log";
    public const string ERROR_LOG_FILE = "error.log";

    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Returns the data folder path.
    /// </summary>
    public static string GetDataPath() => Path.Combine(Application.persistentDataPath, DATA_PATH);

    /// <summary>
    /// Returns the log folder path.
    /// </summary>
    public static string GetLogPath() => Path.Combine(Application.persistentDataPath, LOG_PATH);

    /// <summary>
    /// Returns the path to data file.
    /// </summary>
    public static string GetDataFilePath(string fileName) => Path.Combine(GetDataPath(), fileName);

    /// <summary>
    /// Returns the path to log file.
    /// </summary>
    public static string GetLogFilePath(string fileName) => Path.Combine(GetLogPath(), fileName);

    // ----------------------------------------------------------------------------------------------

    public static void SaveUserData(string jsonData) => SaveData(USER_DATA_FILE, jsonData);

    public static string LoadUserData() => LoadData(USER_DATA_FILE);

    public static void ClearUserData() => ClearData(USER_DATA_FILE);

    // ----------------------------------------------------------------------------------------------

    public static void SaveGameSettings(string jsonData) => SaveData(GAME_SETTINGS_FILE, jsonData);

    public static string LoadGameSettings() => LoadData(GAME_SETTINGS_FILE);

    public static void ClearGameSettings() => ClearData(GAME_SETTINGS_FILE);

    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Saves arbitrary data to a file (encrypted).
    /// </summary>
    public static void SaveData(string fileName, string data)
    {
        string filePath = GetDataFilePath(fileName);
        Directory.CreateDirectory(GetDataPath());
        string encryptedData = JSecurity.Encrypt(data);
        File.WriteAllText(filePath, encryptedData);
        JDebug.LogGreen($"Data saved to: {filePath}");
    }

    /// <summary>
    /// Loads arbitrary data from a file (decrypted).
    /// </summary>
    public static string LoadData(string fileName)
    {
        string filePath = GetDataFilePath(fileName);
        if (File.Exists(filePath))
        {
            JDebug.LogCyan($"Data loaded from: {filePath}");
            string encryptedData = File.ReadAllText(filePath);
            return JSecurity.Decrypt(encryptedData);
        }
        return null;
    }

    /// <summary>
    /// Deletes a data file.
    /// </summary>
    public static void ClearData(string fileName)
    {
        string filePath = GetDataFilePath(fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            JDebug.LogRed($"Data cleared: {filePath}");
        }
    }

    /// <summary>
    /// Deletes all data files in the data folder.
    /// </summary>
    public static void ClearAllData()
    {
        string dataPath = GetDataPath();
        if (Directory.Exists(dataPath))
        {
            Directory.Delete(dataPath, true);
            JDebug.LogRed($"All data cleared from: {dataPath}");
        }
    }

    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds a log entry to a log file.
    /// </summary>
    public static void AddLog(string fileName, string logData)
    {
        string filePath = GetLogFilePath(fileName);
        Directory.CreateDirectory(GetLogPath());
        File.AppendAllText(filePath, $"{System.DateTime.Now} - {logData}\n");
        JDebug.LogGreen($"Log added to: {filePath}");
    }

    /// <summary>
    /// Retrieve the content of log file.
    /// </summary>
    public static string GetLogs(string fileName, int lineCount = 0)
    {
        string filePath = GetLogFilePath(fileName);
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            if (lineCount <= 0 || lineCount >= lines.Length)
                return string.Join("\n", lines);
            int startLine = Mathf.Max(0, lines.Length - lineCount);
            return string.Join("\n", lines, startLine, lines.Length - startLine);
        }
        return "";
    }

    public static void ClearLog(string fileName)
    {
        string filePath = GetLogFilePath(fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            JDebug.LogRed($"Log cleared: {filePath}");
        }
    }

    /// <summary>
    /// Deletes all log files.
    /// </summary>
    public static void ClearLogs()
    {
        string logPath = GetLogPath();
        if (Directory.Exists(logPath))
        {
            Directory.Delete(logPath, true);
            JDebug.LogRed($"Logs cleared from: {logPath}");
        }
    }

    public static void AddGameLog(string logData) => AddLog(GAME_LOG_FILE, logData);

    public static void AddErrorLog(string logData) => AddLog(ERROR_LOG_FILE, logData);

    public static string GetGameLogs(int lineCount = 0) => GetLogs(GAME_LOG_FILE, lineCount);

    public static string GetErrorLogs(int lineCount = 0) => GetLogs(ERROR_LOG_FILE, lineCount);

    public static void ClearGameLogs() => ClearLog(GAME_LOG_FILE);

    public static void ClearErrorLogs() => ClearLog(ERROR_LOG_FILE);
}
