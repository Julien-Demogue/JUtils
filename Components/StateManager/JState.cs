using System;
using System.Collections;
using UnityEngine;

/// <summary>   
/// JState is an abstract base class for managing application states.
/// It provides methods for entering, exiting, and initializing states.
/// </summary>
[RequireComponent(typeof(Canvas))]
public abstract class JState : MonoBehaviour
{
    public void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the state is entered. This method should be overridden in derived classes to define
    /// the behavior that should occur when the state is entered. It is called when the state is activated.
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// Called when the state is exited. This method should be overridden in derived classes to define
    /// the behavior that should occur when the state is entered. It is called when the state is exited.
    /// </summary>
    public abstract void OnExit();

    /// <summary>
    /// Initializes the state. This method should be overridden in derived classes to set up the state
    /// and prepare it for use. It is called when the state is activated for the first time.
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// Waits for a specified number of seconds before executing the provided callback.
    /// This method is useful for delaying actions or transitions in the state.
    /// </summary>
    protected void Wait(float seconds, Action callback)
    {
        StartCoroutine(WaitCoroutine(seconds, callback));
    }

    private IEnumerator WaitCoroutine(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }

    /// <summary>   
    /// Gets the name of the state.
    /// </summary>
    public string GetStateName()
    {
        return GetType().Name;
    }
}
