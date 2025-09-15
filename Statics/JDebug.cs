using UnityEngine;

/// <summary>
/// JDebug provides utility methods to display colored messages in the Unity console.
/// Also allows saving game logs and errors to files via JFileSystem.
/// </summary>
public static class JDebug
{
    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Displays a message in the Unity console with a custom color.
    /// Can also save the message to the game logs if shouldLog is true.
    /// </summary>
    /// <param name="color">Text color.</param>
    /// <param name="message">Message to display.</param>
    /// <param name="shouldLog">If true, saves the message to the game logs.</param>
    public static void Log(Color color, object message, bool shouldLog = false)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        Debug.Log($"<color=#{hexColor}>{message}</color>");

        if (shouldLog)
        {
            SaveGameLog(message);
        }
    }

    public static void LogRed(object message, bool shouldLog = false)
    {
        Log(Color.red, message, shouldLog);
    }

    public static void LogGreen(object message, bool shouldLog = false)
    {
        Log(Color.green, message, shouldLog);
    }

    public static void LogCyan(object message, bool shouldLog = false)
    {
        Log(Color.cyan, message, shouldLog);
    }

    public static void LogBlue(object message, bool shouldLog = false)
    {
        Log(Color.blue, message, shouldLog);
    }

    public static void LogYellow(object message, bool shouldLog = false)
    {
        Log(Color.yellow, message, shouldLog);
    }

    public static void LogMagenta(object message, bool shouldLog = false)
    {
        Log(Color.magenta, message, shouldLog);
    }

    public static void LogWhite(object message, bool shouldLog = false)
    {
        Log(Color.white, message, shouldLog);
    }

    public static void LogBlack(object message, bool shouldLog = false)
    {
        Log(Color.black, message, shouldLog);
    }

    public static void LogGray(object message, bool shouldLog = false)
    {
        Log(Color.gray, message, shouldLog);
    }

    public static void LogOrange(object message, bool shouldLog = false)
    {
        Log(new Color(1.0f, 0.5f, 0.0f), message, shouldLog);
    }

    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Displays an error message in the Unity console and saves it to the error logs.
    /// </summary>
    public static void LogError(object message)
    {
        Debug.LogError(message);
        SaveErrorLog(message);
    }

    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Saves a message to the game log file.
    /// </summary>
    public static void SaveGameLog(object message)
    {
        JFileSystem.AddGameLog(message.ToString());
    }

    /// <summary>
    /// Saves a message to the error log file.
    /// </summary>
    public static void SaveErrorLog(object message)
    {
        JFileSystem.AddErrorLog(message.ToString());
    }
}
