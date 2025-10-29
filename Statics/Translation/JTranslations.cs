using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// JTranslations manages the retrieval, storage, and access to multilingual translations for your Unity application.
/// <para>Usage:</para>
/// <list type="bullet">
/// <item><description>Create a Google Sheet with the following columns: KEY, EN, FR, etc.</description></item>
/// <item><description>You can add more languages by extending the language enum and updating the PopulateDictionary() method.</description></item>
/// <item><description>Publish this Google Sheet to the web using the "tab-separated values" format.</description></item>
/// <item><description>Update the GOOGLE_SHEET_URL.</description></item>
/// </list>
/// You can add parameters in your translations using the {0}, {1}, ... format.
/// Example: "You have {0} new messages and {1} friend requests."
/// </summary>
public class JTranslations
{
    // Enum for supported languages
    // Add more languages as needed
    public enum Language
    {
        EN,
        FR,
    }

    // This url is used to access Google Sheet for translations.
    private const string GOOGLE_SHEET_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQxoEx0UUcTGg9wHUA4pGMfHzLsFg9Y5jpVTbxJUdd7VPCG-xMTcdX21AtM9RVkHfkUZTsLeEVX7DZ/pub?gid=0&single=true&output=tsv";
    private const string LANG_FOLDER_PATH = "Assets/Language";
    private const string LANG_FILE_NAME = "translations.csv";
    private const string CSV_SEPARATOR = "\t"; // Tab-separated values
    private const string VAR_IDENTIFIER = "{x}";

    private static Language currentLanguage = Language.FR; // Default language
    public static event Action OnLanguageChanged;

    private static Dictionary<string, Dictionary<Language, string>> translations = new();

    /// <summary>
    /// Fetches translations from the Google Sheets URL and saves them to a CSV file.
    /// </summary>
    /// <returns>Returns true if the fetch was successful, otherwise false.</returns>
    public static async Task<bool> FetchTranslations()
    {
        // Send a web request to fetch the CSV file from Google Sheets
        using var www = UnityEngine.Networking.UnityWebRequest.Get(GOOGLE_SHEET_URL);
        var operation = www.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch translations: " + www.error);
            return false;
        }

        string csvText = www.downloadHandler.text;

        // Save the csv file to the specified path
        string filePath = GetTranslationFilePath();
        Directory.CreateDirectory(LANG_FOLDER_PATH);
        File.WriteAllText(filePath, csvText);
        JDebug.LogGreen($"Translations saved to {filePath}");
        return true;
    }

    // Populate the dictionary with translations from the JSON file
    private static void PopulateDictionary()
    {
        translations.Clear();

        string filePath = GetTranslationFilePath();
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Translation file not found at {filePath}");
            return;
        }

        // Read the CSV file
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length <= 1 || string.IsNullOrWhiteSpace(lines[0]))
        {
            Debug.LogError("Translation file is empty or has no valid data.");
            return;
        }

        string[] headers = lines[0].Split(CSV_SEPARATOR);
        if (headers.Length <= 1)
        {
            Debug.LogError("Translation file must have at least one key and one language column.");
            return;
        }

        // Parse the csv content and populate the translations dictionary   
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string[] values = line.Split(CSV_SEPARATOR);
            if (values.Length != headers.Length)
            {
                Debug.LogWarning($"Line {i + 1} in translation file has a different number of columns than the header: {line}");
                continue;
            }

            string key = values[0].Trim();

            translations[key] = new Dictionary<Language, string>()
                {
                    // Should match the order of languages in the header
                    { Language.EN, values[1].Trim() },
                    { Language.FR, values[2].Trim() }
                };
        }
    }

    /// <summary>
    /// Gets the translation for a given key in the current language.
    /// </summary>
    /// <param name="key">The key for the translation.</param>
    /// <param name="varValues">Optional variable values to replace in the translation string.</param>
    /// <returns>The translated string if found, otherwise returns the key.</returns>
    public static string Get(string key, params string[] varValues)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("Key is null or empty.");
            return string.Empty;
        }

        // Ensure translations are loaded before attempting to get a translation
        if (!AreTranslationsLoaded())
        {
            PopulateDictionary();

            if (!AreTranslationsLoaded())
            {
                Debug.LogError("Translations dictionary is still empty after initialization.");
                return key;
            }
        }

        if (translations.TryGetValue(key, out var langDict))
        {
            if (langDict.TryGetValue(currentLanguage, out var translation))
            {
                // Replace variable identifiers with provided values
                for (int i = 0; i < varValues.Length; i++)
                {
                    string varIdentifier = VAR_IDENTIFIER.Replace("x", i.ToString());
                    translation = translation.Replace(varIdentifier, varValues[i], StringComparison.Ordinal);
                }
                return translation;
            }
            else
            {
                Debug.LogWarning($"Translation for key '{key}' not found in language '{currentLanguage}'.");
                return key;
            }
        }
        else
        {
            Debug.LogWarning($"Key '{key}' not found in translations.");
            return key;
        }
    }

    /// <summary>
    /// Checks if translations have been loaded.
    /// </summary>
    /// <returns>Returns true if translations are loaded, otherwise false.</returns>
    public static bool AreTranslationsLoaded()
    {
        return translations.Count > 0;
    }

    /// <summary>
    /// Sets the current language for translations.
    /// </summary>
    /// <param name="language">The language to set as current.</param>
    public static void SetLanguage(Language language)
    {
        currentLanguage = language;
        OnLanguageChanged?.Invoke();
    }

    /// <summary>
    /// Gets the file path for the translation CSV file.
    /// /// </summary>
    /// <returns>The file path as a string.</returns>
    public static string GetTranslationFilePath()
    {
        return $"{LANG_FOLDER_PATH}/{LANG_FILE_NAME}";
    }
}
