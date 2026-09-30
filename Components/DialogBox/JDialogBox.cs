using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class JDialogBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogTextField;
    public float typingSpeed = 0.03f;
    private List<string> dialogTexts = new List<string>();
    private int currentDialogIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine = null;

    /// <summary>
    /// Shows the dialog box and starts displaying the dialog texts for the given dialogTextId.
    /// </summary>
    /// <param name="dialogTextId"></param>
    public void ShowDialog(string dialogTextId)
    {
        gameObject.SetActive(true);

        FetchDialogTexts(dialogTextId);

        if (dialogTexts.Count == 0)
        {
            Debug.LogWarning($"No dialog texts found for ID: {dialogTextId}");
            CloseDialog();
            return;
        }

        currentDialogIndex = 0;
        StartTyping(dialogTexts[currentDialogIndex]);
    }

    /// <summary>
    /// Fetches dialog texts from JTranslations based on the provided dialogTextId and populates the dialogTexts list.
    /// </summary>
    /// <param name="dialogTextId"></param>
    private void FetchDialogTexts(string dialogTextId)
    {
        // Get all texts from JTranslations starting with DIALOG_ and ending with _x (x as the order number)
        dialogTexts.Clear();

        int maxIndex = 20;
        int index = 1;
        while (true)
        {
            string key = $"DIALOG_{dialogTextId}_{index}";

            if (!JTranslations.DoesKeyExist(key))
                break;

            dialogTexts.Add(JTranslations.Get(key));
            index++;

            if (index > maxIndex)
            {
                Debug.LogWarning($"Maximum dialog index reached for {dialogTextId}.");
                break;
            }
        }
    }

    /// <summary>
    /// Starts the typing effect for the given dialog text in the dialogTextField. If a typing effect is already in progress, it stops it before starting a new one.
    /// </summary>
    /// <param name="dialogText"></param>
    private void StartTyping(string dialogText)
    {
        if(typingSpeed <= 0f)
        {
            dialogTextField.text = dialogText;
            Debug.LogWarning("Typing speed is set to 0 or less. Displaying full text without typing effect.");
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(dialogText));
    }

    /// <summary>
    /// Types the dialog text character by character in the dialogTextField.
    /// </summary>
    /// <param name="dialogText"></param>
    private IEnumerator TypeText(string dialogText)
    {
        isTyping = true;

        dialogTextField.text = dialogText;
        dialogTextField.maxVisibleCharacters = 0;
        dialogTextField.ForceMeshUpdate();
        int total = dialogTextField.textInfo.characterCount;

        float elapsed = 0f;
        while (dialogTextField.maxVisibleCharacters < total)
        {
            elapsed += Time.unscaledDeltaTime;
            dialogTextField.maxVisibleCharacters = Mathf.Min(total, (int)(elapsed / typingSpeed));
            yield return null;
        }

        isTyping = false;
    }

    /// <summary>
    /// Moves to the next dialog text or closes the dialog box if all texts have been displayed
    /// </summary>
    public void NextDialog()
    {
        if (isTyping)
        {
            SkipTyping();
            return;
        }

        currentDialogIndex++;

        if (currentDialogIndex < dialogTexts.Count)
        {
            StartTyping(dialogTexts[currentDialogIndex]);
        }
        else
        {
            CloseDialog();
        }
    }

    /// <summary>
    /// Skips the typing effect and immediately displays the full dialog text in the dialogTextField.
    /// </summary>
    private void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogTextField.maxVisibleCharacters = int.MaxValue;
        isTyping = false;
    }

    /// <summary>
    /// Closes the dialog box and stops any ongoing typing effect.
    /// </summary>
    public void CloseDialog()
    {
        if (gameObject.activeSelf)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            gameObject.SetActive(false);
        }
    }
}