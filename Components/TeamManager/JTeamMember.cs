using UnityEngine;

/// <summary>
/// Represents a member of a team in the game.
/// </summary>
[System.Serializable]
public class JTeamMember
{
    public string Id { get; }
    public string Name;
    public MonoBehaviour Member;

    /// <summary>
    /// Default constructor for creating an empty team member.
    /// </summary>
    public JTeamMember()
    {
        Id = JIDGenerator.GenerateIdString();
        Name = "";
        Member = null;
    }

    /// <summary>
    /// Constructor for creating a team member with a MonoBehaviour.
    /// </summary>
    /// <param name="member">The MonoBehaviour representing the team member.</param>
    public JTeamMember(MonoBehaviour member)
    {
        Id = JIDGenerator.GenerateIdString();
        Member = member;
        Name = member.name;
    }

    /// <summary>
    /// Constructor for creating a team member with a name and a MonoBehaviour.
    /// </summary>
    /// <param name="name">The name of the team member.</param>
    /// <param name="member">The MonoBehaviour representing the team member.</param>
    public JTeamMember(string name, MonoBehaviour member)
    {
        Id = JIDGenerator.GenerateIdString();
        Name = name;
        Member = member;
    }
}