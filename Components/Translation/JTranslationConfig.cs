using UnityEngine;

[CreateAssetMenu(fileName = "JTranslationConfig", menuName = "JUtils/Translation/Translation Config")]
public class JTranslationConfig : ScriptableObject
{
    [Header("Google Sheet Configuration")]
    [Tooltip("The URL of the Google Sheet containing translations. Should be format TSV.")]
    public string GoogleSheetUrl;

    [Tooltip("The name of the sheet within the Google Sheet.")]
    public string SheetName;

    [Header("Local Storage Configuration")]
    [Tooltip("The folder path where translation files will be stored.")]
    public string LangFolderPath = "Assets/Translations";

    [Tooltip("The name of the translation file.")]
    public string LangFileName = "translations.csv";

    [Header("CSV Configuration")]
    [Tooltip("The separator used in the CSV file.")]
    public char CsvSeparator = ',';
}