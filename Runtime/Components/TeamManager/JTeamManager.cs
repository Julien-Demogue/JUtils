using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a collection of teams in the game.
/// </summary>
public class JTeamManager : MonoBehaviour
{
    [SerializeField] private bool isPersistent = true;

    public List<JTeam> Teams = new();

    public static JTeamManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (isPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gets a team by its unique ID.
    /// </summary>
    /// <param name="id">The unique ID of the team.</param>
    /// <returns>The JTeam with the specified ID, or null if not found.</returns>
    public JTeam GetTeamById(string id)
    {
        return Teams.Find(team => team.Id == id);
    }

    /// <summary>
    /// Gets a team by its name.
    /// </summary>
    /// <param name="name">The name of the team.</param>
    /// <returns>The JTeam with the specified name, or null if not found.</returns>
    public JTeam GetTeamByName(string name)
    {
        return Teams.Find(team => team.Name == name);
    }

    /// <summary>
    /// Gets the count of teams managed by this manager.
    /// </summary>
    /// <returns>The number of teams.</returns>
    public JTeam AddTeam(string name, Color color = default)
    {
        JTeam newTeam = new JTeam(name, color);
        Teams.Add(newTeam);
        return newTeam;
    }

    /// <summary>
    /// Removes a team by its unique ID.
    /// </summary>
    /// <param name="id">The unique ID of the team to remove.</param>
    public void RemoveTeam(string id)
    {
        JTeam teamToRemove = GetTeamById(id);
        if (teamToRemove != null)
        {
            Teams.Remove(teamToRemove);
        }
    }

    /// <summary>
    /// Clears all teams managed by this manager.
    /// </summary>
    public void ClearTeams()
    {
        Teams.Clear();
    }
}
