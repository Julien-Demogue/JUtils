using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a team in the game.
/// </summary>
[System.Serializable]
public class JTeam
{
    public string Id { get; }
    public string Name;
    public Color Color;
    public List<JTeamMember> Members = new();

    /// <summary>
    /// Constructor for creating a team with a name and an optional color.
    /// </summary>
    /// <param name="name">The name of the team.</param>
    /// <param name="color">The color of the team, default is white.</param>
    public JTeam(string name, Color color = default)
    {
        Id = JIDGenerator.GenerateIdString();
        Name = name;
        Color = color;
    }

    /// <summary>
    /// Gets a team member by their unique ID.
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns>The JTeamMember with the specified ID, or null if not found.</returns>
    public JTeamMember GetMemberById(string memberId)
    {
        return Members.Find(member => member.Id == memberId);
    }

    /// <summary>
    /// Gets a team member by their name.
    /// </summary>
    /// <param name="memberName">The name of the team member.</param>
    /// <returns>The JTeamMember with the specified name, or null if not found.</returns>
    public JTeamMember GetMemberByName(string memberName)
    {
        return Members.Find(member => member.Name == memberName);
    }

    /// <summary>
    /// Gets the member count for the team.
    /// </summary>
    /// <returns>The number of members in the team.</returns>
    public int GetMemberCount()
    {
        return Members.Count;
    }

    /// <summary>
    /// Adds a member to the team.
    /// </summary>
    /// <param name="member">The JTeamMember to add.</param>
    public void AddMember(JTeamMember member)
    {
        if (member != null && !Members.Contains(member))
        {
            Members.Add(member);
        }
    }

    /// <summary>
    /// Removes a member from the team by their unique ID.
    /// </summary>
    /// <param name="memberId">The unique ID of the member to remove.</param
    public void RemoveMember(string memberId)
    {
        JTeamMember memberToRemove = GetMemberById(memberId);
        if (memberToRemove != null)
        {
            Members.Remove(memberToRemove);
        }
    }

    /// <summary>
    /// Clears all members from the team.
    /// </summary>
    public void ClearMembers()
    {
        Members.Clear();
    }
}