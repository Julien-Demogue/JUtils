using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Provides a system for managing custom timers, allowing creation, update,
/// pause, reset, and removal of incremental or decremental timers.
/// </summary>
public class JClock
{
    /// <summary>
    /// Represents an individual timer
    /// </summary>
    public class Timer
    {
        public float Time;
        public float StartTime;
        public float EndTime;
        public bool IsPaused;
        public bool IsEndless;
        public bool IsIncremental;
        public bool IsFinished;

        public Timer(float startTime, float endTime, bool isIncremental, bool isEndless)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.IsEndless = isEndless;
            this.Time = startTime;
            this.IsPaused = false;
            this.IsIncremental = isIncremental;
        }

        /// <summary>
        /// Resets the timer to its starting value.
        /// </summary>
        public void Reset()
        {
            Time = StartTime;
            IsFinished = false;
            IsPaused = false;
        }

        /// <summary>
        /// Updates the timer state (should be called every frame).
        /// </summary>
        public void Update()
        {
            if (!IsPaused && !IsFinished)
            {
                if (IsIncremental)
                {
                    Time += UnityEngine.Time.deltaTime;
                }
                else
                {
                    Time -= UnityEngine.Time.deltaTime;
                }

                if (!IsEndless && ((IsIncremental && Time >= EndTime) || (!IsIncremental && Time <= EndTime)))
                {
                    Time = EndTime;
                    IsFinished = true;
                }
            }
        }
    }

    private static Dictionary<string, Timer> timers = new();

    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Starts a new timer with the specified parameters.
    /// </summary>
    public static string StartTimer(float startTime, float endTime, bool isIncremental = true, bool isEndless = false)
    {
        Timer newTimer = new(startTime, endTime, isIncremental, isEndless);
        string id = JIDGenerator.GenerateIdString();
        timers[id] = newTimer;
        return id;
    }

    /// <summary>
    /// Starts a timer from an existing instance.
    /// </summary>
    public static string StartTimer(Timer timer)
    {
        if (timer == null)
        {
            Debug.LogWarning("Cannot start a null timer.");
            return null;
        }

        return StartTimer(timer.StartTime, timer.EndTime, timer.IsIncremental, timer.IsEndless);
    }

    /// <summary>
    /// Starts a countdown timer for a given duration.
    /// </summary>
    public static string StartCountdownTimer(float duration)
    {
        if (duration <= 0)
        {
            Debug.LogWarning("Duration must be greater than zero for a countdown timer.");
            return null;
        }

        return StartTimer(duration, 0f, false, false);
    }

    /// <summary>
    /// Starts an incremental timer for a given duration.
    /// </summary>
    public static string StartIncrementalTimer(float duration)
    {
        if (duration <= 0)
        {
            Debug.LogWarning("Duration must be greater than zero for an incremental timer.");
            return null;
        }

        return StartTimer(0f, duration, true, false);
    }

    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Stops and removes the timer with the given identifier.
    /// </summary>
    public static void StopTimer(string id)
    {
        if (DoTimersExist(id))
        {
            timers.Remove(id);
        }
    }

    /// <summary>
    /// Resets the timer with the given identifier.
    /// </summary>
    public static void ResetTimer(string id)
    {
        if (DoTimersExist(id))
        {
            timers[id].Reset();
        }
    }

    /// <summary>
    /// Pauses the timer with the given identifier.
    /// </summary>
    public static void PauseTimer(string id)
    {
        if (DoTimersExist(id))
        {
            timers[id].IsPaused = true;
        }
    }

    /// <summary>
    /// Resumes the timer with the given identifier.
    /// </summary>
    public static void ResumeTimer(string id)
    {
        if (DoTimersExist(id))
        {
            timers[id].IsPaused = false;
        }
    }

    /// <summary>
    /// Returns the current time of the timer with the given identifier.
    /// </summary>
    public static float GetTime(string id)
    {
        if (DoTimersExist(id))
        {
            return timers[id].Time;
        }
        return float.NaN;
    }

    /// <summary>
    /// Returns the progress of the timer with the given identifier as a value between 0 and 1.
    /// </summary>
    public static float GetTimerProgress(string id)
    {
        if (DoTimersExist(id))
        {
            Timer timer = timers[id];
            if (timer.IsIncremental)
            {
                return Mathf.Clamp01((timer.Time - timer.StartTime) / (timer.EndTime - timer.StartTime));
            }
            else
            {
                return Mathf.Clamp01((timer.EndTime - timer.Time) / (timer.EndTime - timer.StartTime));
            }
        }
        return float.NaN;
    }

    /// <summary>
    /// Checks if the timer with the given identifier is finished.
    /// </summary>
    public static bool IsFinished(string id)
    {
        if (DoTimersExist(id))
        {
            return timers[id].IsFinished;
        }
        return false;
    }

    /// <summary>
    /// Checks if the timer with the given identifier is paused.
    /// </summary>
    public static bool IsPaused(string id)
    {
        if (DoTimersExist(id))
        {
            return timers[id].IsPaused;
        }
        return false;
    }

    // ----------------------------------------------------------------------------------------------

    private static bool DoTimersExist(string id)
    {
        if (timers.ContainsKey(id))
        {
            return true;
        }
        else
        {
            Debug.LogWarning($"Timer with ID '{id}' does not exist.");
            return false;
        }
    }

    // ----------------------------------------------------------------------------------------------

    /// <summary>
    /// Pauses all timers.
    /// </summary>
    public static void PauseAllTimers()
    {
        foreach (var timer in timers.Values)
        {
            timer.IsPaused = true;
        }
    }

    /// <summary>
    /// Resumes all timers.
    /// </summary>
    public static void ResumeAllTimers()
    {
        foreach (var timer in timers.Values)
        {
            timer.IsPaused = false;
        }
    }

    /// <summary>
    /// Stops and removes all timers.
    /// </summary>
    public static void StopAllTimers()
    {
        timers.Clear();
    }

    /// <summary>
    /// Removes all finished timers.
    /// </summary>
    public static void RemoveEndedTimers()
    {
        List<string> endedTimers = new();

        foreach (var kvp in timers)
        {
            if (kvp.Value.IsFinished)
            {
                endedTimers.Add(kvp.Key);
            }
        }

        foreach (var id in endedTimers)
        {
            timers.Remove(id);
        }
    }

    /// <summary>
    /// Resets all timers.
    /// </summary>
    public static void ResetAllTimers()
    {
        foreach (var timer in timers.Values)
        {
            timer.Reset();
        }
    }
    /// <summary>
    /// Updates all timers (should be called every frame).
    /// </summary>
    public static void Update()
    {
        if (timers.Count == 0)
        {
            return; // No timers to update
        }

        foreach (var timer in timers.Values)
        {
            timer.Update();
        }
    }
}
