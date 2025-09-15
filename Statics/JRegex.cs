using System.Text.RegularExpressions;

/// <summary>
/// Provides utility methods for validating strings using common regular expressions
/// </summary>
public class JRegex
{
    private const string EMAIL_REGEX = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"; // Email address format
    private const string URL_REGEX = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$"; // URL with http, https, or ftp protocol
    private const string PHONE_REGEX = @"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$"; // Phone number with optional country code
    private const string IP_ADDRESS_REGEX = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$"; // IPv4 address
    private const string ALPHANUMERIC_REGEX = @"^[a-zA-Z0-9]+$"; // Only letters and numbers    
    private const string HEX_COLOR_REGEX = @"^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$"; // Hex color code
    private const string UUID_REGEX = @"^[{(]?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}[)}]?$"; // UUID format

    /// <summary>
    /// Checks if the input matches the specified regular expression.
    /// </summary>
    public static bool IsValid(string input, string regex)
    {
        return Regex.IsMatch(input, regex);
    }

    /// <summary>
    /// Checks if the input is a valid email address.
    /// </summary>
    public static bool IsValidEmail(string input)
    {
        return IsValid(input, EMAIL_REGEX);
    }

    /// <summary>
    /// Checks if the input is a valid URL.
    /// </summary>
    public static bool IsValidURL(string input)
    {
        return IsValid(input, URL_REGEX);
    }

    /// <summary>
    /// Checks if the input is a valid phone number.
    /// </summary>
    public static bool IsValidPhoneNumber(string input)
    {
        return IsValid(input, PHONE_REGEX);
    }

    /// <summary>
    /// Checks if the input is a valid IP address (IPv4).
    /// </summary>
    public static bool IsValidIPAddress(string input)
    {
        return IsValid(input, IP_ADDRESS_REGEX);
    }

    /// <summary>
    /// Checks if the input is alphanumeric.
    /// </summary>
    public static bool IsAlphanumeric(string input)
    {
        return IsValid(input, ALPHANUMERIC_REGEX);
    }

    /// <summary>
    /// Checks if the input is a valid hex color.
    /// </summary>
    public static bool IsValidHexColor(string input)
    {
        return IsValid(input, HEX_COLOR_REGEX);
    }

    /// <summary>
    /// Checks if the input is a valid UUID.
    /// </summary>
    public static bool IsValidUUID(string input)
    {
        return IsValid(input, UUID_REGEX);
    }
}
