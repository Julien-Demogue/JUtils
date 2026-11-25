using UnityEngine;
using System;

/// <summary>
/// A manager class for handling Discord integration and status updates.
/// Uses the Discord Game SDK bundled in the DiscordGameSDK folder (Windows only).
/// </summary>
public class JDiscordManager : MonoBehaviour
{
    [SerializeField] private long applicationId;
    private Discord.Discord discord;
    private DateTime startTime;
    private JDiscordStatus currentStatus = null;

    private float checkDiscordInterval = 10f; // Interval in seconds to check if Discord is running
    private float timeSinceLastCheck = 0f;

    public static JDiscordManager Instance { get; private set; }

    private void TryInit()
    {
        if (discord != null)
        {
            return;
        }

        try
        {
            discord = new Discord.Discord(
                applicationId,
                (ulong)Discord.CreateFlags.NoRequireDiscord
            );
            RefreshStatus(currentStatus ?? new JDiscordStatus(null), false);
        }
        catch (Exception)
        {
            discord = null;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        startTime = DateTime.UtcNow;
        TryInit();
    }

    private void Update()
    {
        if (discord == null)
        {
            timeSinceLastCheck += Time.deltaTime;
            if (timeSinceLastCheck >= checkDiscordInterval)
            {
                timeSinceLastCheck = 0f;
                TryInit();
            }
        }

        if (discord == null)
        {
            // If still null after trying to initialize, skip the rest of the update
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

    public void RefreshStatus(JDiscordStatus status, bool resetStartTime = false)
    {
        currentStatus = status;

        if (resetStartTime)
        {
            startTime = DateTime.UtcNow;
        }

        if (discord == null)
        {
            Debug.LogWarning("Discord is not initialized");
            return;
        }

        Discord.ActivityManager activityManager = discord.GetActivityManager();
        Discord.Activity activity = new Discord.Activity
        {
            Details = status.Details,
            State = status.State,
            Timestamps = new Discord.ActivityTimestamps
            {
                Start = ((DateTimeOffset)startTime).ToUnixTimeSeconds()
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
            else
            {
                currentStatus = null;
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
