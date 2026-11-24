using UnityEngine;
using System;

/// <summary>
/// A manager class for handling Discord integration and status updates.
/// Requires the Discord Game SDK to be set up in the Unity project.
/// </summary>
public class JDiscordManager : MonoBehaviour
{
    [SerializeField] private long applicationId;
    private Discord.Discord discord;
    private DateTime startTime;

    public static JDiscordManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        try
        {
            discord = new Discord.Discord(
                applicationId,
                (ulong)Discord.CreateFlags.NoRequireDiscord
            );
            startTime = DateTime.UtcNow;
        }
        catch (Discord.ResultException e)
        {
            discord = null;
            Debug.LogWarning("Discord SDK initialization failed: " + e.Message);
        }
    }

    private void Update()
    {
        if (discord == null)
        {
            return;
        }

        try
        {
            discord.RunCallbacks();
        }
        catch (Discord.ResultException e)
        {
            Debug.LogWarning("Discord connection lost: " + e.Message);
            KillDiscord();
        }
    }

    public void RefreshStatus(string details, string state, bool resetStartTime = false)
    {
        if (discord == null)
        {
            Debug.LogWarning("Discord SDK is not initialized. Cannot refresh status.");
            return;
        }

        if (resetStartTime)
        {
            startTime = DateTime.UtcNow;
        }

        Discord.ActivityManager activityManager = discord.GetActivityManager();
        Discord.Activity activity = new Discord.Activity
        {
            details = details,
            state = state,
            timestamps = new Discord.ActivityTimestamps
            {
                start = ((DateTimeOffset)startTime).ToUnixTimeSeconds()
            }
        };

        activityManager.UpdateActivity(activity, (result) =>
        {
            if (result != Discord.Result.Ok)
            {
                Debug.LogWarning("Failed to update Discord activity: " + result);
            }
        });
    }

    public void ClearStatus()
    {
        if (discord == null)
        {
            Debug.LogWarning("Discord SDK is not initialized. Cannot clear status.");
            return;
        }

        Discord.ActivityManager activityManager = discord.GetActivityManager();
        activityManager.ClearActivity((result) =>
        {
            if (result != Discord.Result.Ok)
            {
                Debug.LogWarning("Failed to clear Discord activity: " + result);
            }
        });
    }

    private void KillDiscord()
    {
        discord?.Dispose();
        discord = null;
    }

    private void OnApplicationQuit() => KillDiscord();

    private void OnDestroy()
    {
        if (Instance != this)
        {
            return;
        }

        KillDiscord();
        Instance = null;
    }
}