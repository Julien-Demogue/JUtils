using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// JDialog is a base class for creating dialog windows in Unity.
/// It provides functionality to show and manage dialogs with confirm and cancel actions.
/// </summary>
public abstract class JDialog : MonoBehaviour
{
    private static Queue<JDialog> dialogQueue = new();
    private static JDialog currentDialog;

    [SerializeField] protected TextMeshProUGUI dialogTitle;
    [SerializeField] protected TextMeshProUGUI dialogMessage;

    private Action onConfirm;
    private Action onCancel;

    private void Init(string title, string message, Action onConfirm = null, Action onCancel = null)
    {
        if (dialogTitle != null)
        {
            dialogTitle.text = title;
        }

        if (dialogMessage != null)
        {
            dialogMessage.text = message;
        }

        this.onConfirm = onConfirm;
        this.onCancel = onCancel;
    }

    public void OnClickConfirm()
    {
        onConfirm?.Invoke();
        CloseCurrentDialog();
    }

    public void OnClickCancel()
    {
        onCancel?.Invoke();
        CloseCurrentDialog();
    }

    /// <summary>
    /// Called when the dialog is opened.
    /// This method should be overridden to implement custom behavior when the dialog is opened.
    /// </summary>
    protected abstract void OnOpen();

    /// <summary>
    /// Called when the dialog is closed.
    /// This method should be overridden to implement custom behavior when the dialog is closed.
    /// </summary>
    protected abstract void OnClose();

    // ---------------------------------------------------------------------------------------------------

    /// <summary>
    /// Shows a dialog with the specified parameters.
    /// </summary>
    /// <param name="dialogPrefab">The dialog prefab to instantiate.</param>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="message">The message to display in the dialog.</param>
    /// <param name="onConfirm">Action to execute when the confirm button is clicked.</param>
    /// <param name="onCancel">Action to execute when the cancel button is clicked (optional).</param>
    public static void ShowDialog(JDialog dialogPrefab, string title, string message, Action onConfirm, Action onCancel = null)
    {
        if (dialogPrefab == null)
        {
            Debug.LogError("Dialog prefab is null.");
            return;
        }

        bool isFirstDialog = currentDialog == null;

        JDialog newDialog = Instantiate(dialogPrefab);
        newDialog.Init(title, message, onConfirm, onCancel);
        newDialog.gameObject.SetActive(isFirstDialog);

        if (isFirstDialog)
        {
            currentDialog = newDialog;
            currentDialog.OnOpen();
        }
        else
        {
            dialogQueue.Enqueue(newDialog);
        }
    }

    /// <summary>
    /// Closes the current dialog and opens the next one in the queue if available.
    /// </summary>
    public static void CloseCurrentDialog()
    {
        if (currentDialog)
        {
            currentDialog.OnClose();

            Destroy(currentDialog.gameObject);
            currentDialog = null;

            if (dialogQueue.Count > 0)
            {
                currentDialog = dialogQueue.Dequeue();
                currentDialog.gameObject.SetActive(true);
                currentDialog.OnOpen();
            }
        }
    }

    /// <summary>
    /// Closes all opened dialogs.
    /// </summary>
    public static void CloseAllDialogs()
    {
        while (currentDialog != null)
        {
            CloseCurrentDialog();
        }
        dialogQueue.Clear();
    }
}
