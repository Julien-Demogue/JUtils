using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// JStateManager is a singleton class that manages the current state of the application.
/// It allows changing states by type name or type, caching states for performance,
/// and provides functionality to return to the previous state.
/// </summary>
public class JStateManager
{
    public static JStateManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }
    }

    private static JStateManager instance;

    private Dictionary<Type, JState> stateCache = new Dictionary<Type, JState>();
    private JState currentState;
    private JState previousState;

    /// <summary>
    /// Changes the current state to the specified type name.
    /// </summary>
    /// <param name="stateTypeName">The fully qualified name of the state type.</param>
    public void ChangeState(string stateTypeName)
    {
        var type = Type.GetType(stateTypeName);
        if (type == null || !typeof(JState).IsAssignableFrom(type))
        {
            Debug.LogError($"State type not found or invalid: {stateTypeName}");
            return;
        }
        SetState(type);
    }

    /// <summary>
    /// Sets the current state to the specified type.
    /// </summary>
    /// <param name="stateType">The type of the state to set.</param>
    public void SetState(Type stateType)
    {
        if (stateType == null || !typeof(JState).IsAssignableFrom(stateType))
        {
            Debug.LogError("Invalid state type.");
            return;
        }

        // Check if the current state is already the desired state
        if (currentState != null && currentState.GetType() == stateType)
            return;

        // If the current state is not null, exit it and deactivate its GameObject
        if (currentState != null)
        {
            currentState.OnExit();
            currentState.gameObject.SetActive(false);
            previousState = currentState;
        }

        // Try to get the state from the cache
        if (!stateCache.TryGetValue(stateType, out var newState))
        {
            newState = FindStateInScene(stateType);
            if (newState == null)
            {
                Debug.LogError($"State {stateType.Name} not found in scene or Resources.");
                return;
            }
            stateCache[stateType] = newState;
            newState.Init();
        }

        currentState = newState;
        currentState.gameObject.SetActive(true);
        currentState.OnEnter();
        JDebug.LogMagenta($"State switched to: {currentState.GetStateName()}");
    }

    /// <summary>
    /// Returns to the previous state if it exists.
    /// </summary>
    public void BackToPreviousState()
    {
        if (previousState != null)
        {
            SetState(previousState.GetType());
        }
    }

    /// <summary>
    /// Returns the current state.
    /// </summary>
    public JState GetCurrentState()
    {
        return currentState;
    }

    /// <summary>
    /// Finds a state in the current scene by type.
    /// </summary>
    /// <param name="stateType">The type of the state to find.</param>
    private JState FindStateInScene(Type stateType)
    {
        foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            var state = root.GetComponentInChildren(stateType, true) as JState;
            if (state != null)
                return state;
        }
        return null;
    }
}
