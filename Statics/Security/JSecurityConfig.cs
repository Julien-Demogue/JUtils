using UnityEngine;

/// <summary>
/// Holds the AES key JSecurity encrypts and decrypts with.
/// Create one asset per game project (Assets > Create > JUtils > Security Config), place it
/// in a Resources folder named "JSecurityConfig", and keep that asset out of the shared JUtils
/// repository so each project has its own key instead of a key hardcoded in JUtils itself.
/// </summary>
[CreateAssetMenu(fileName = "JSecurityConfig", menuName = "JUtils/Security Config")]
public class JSecurityConfig : ScriptableObject
{
    [SerializeField] private string encryptionKey;

    public string EncryptionKey => encryptionKey;
}
