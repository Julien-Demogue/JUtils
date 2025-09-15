using System.Collections.Generic;
using UnityEngine;

// <summary>
// JRandomSpawn is a utility class for spawning objects at random positions within a defined area or at specified points.
// </summary>
public class JRandomSpawn : MonoBehaviour
{
    public enum SpawnType
    {
        AREA,
        POINTS
    }

    public SpawnType Type = SpawnType.AREA;
    public Vector3 SpawnAreaSize = new Vector3(10, 0, 10);
    public Transform[] SpawnPoints;

    /// <summary>
    /// Spawns a single object at a random position within the defined spawn area or at one of the defined spawn points.
    /// </summary>
    /// <typeparam name="T">The type of the object to spawn, must be a MonoBehaviour.</typeparam>
    /// <param name="objectToSpawn">The object to spawn.</param>
    /// <param name="offset">An optional offset to apply to the spawn position.</param>
    /// <returns>The spawned object.</returns>
    public T SpawnSingle<T>(T objectToSpawn, Vector3 offset = default) where T : MonoBehaviour
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning("Object to spawn is null.");
            return null;
        }

        Vector3 spawnPosition = GetSpawnPosition() + offset;
        return Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// Spawns a single object from a list of spawnable objects at a random position within
    /// the defined spawn area or at one of the defined spawn points.
    /// </summary>
    /// <typeparam name="T">The type of the object to spawn, must be a MonoBehaviour.</typeparam>
    /// <param name="spawnableObjects">An array of objects to choose from for spawning.</param>
    /// <param name="offset">An optional offset to apply to the spawn position.</param>
    /// <returns>The spawned object.</returns>
    public T SpawnSingleFromList<T>(T[] spawnableObjects, Vector3 offset = default) where T : MonoBehaviour
    {
        if (spawnableObjects == null || spawnableObjects.Length == 0)
        {
            Debug.LogWarning("No objects to spawn.");
            return null;
        }

        int randomIndex = Random.Range(0, spawnableObjects.Length);
        return SpawnSingle(spawnableObjects[randomIndex], offset);
    }

    /// <summary>
    /// Spawns multiple instances of the specified object at random positions within the defined spawn area or  
    /// at one of the defined spawn points.
    /// </summary>
    /// <typeparam name="T">The type of the object to spawn, must be a MonoBehaviour.</typeparam>
    /// <param name="objectToSpawn">The object to spawn.</param>
    /// <param name="amount">The number of objects to spawn.</param>
    /// <param name="offset">An optional offset to apply to the spawn position.</param>
    /// <returns>A list of spawned objects.</returns>
    public List<T> SpawnMultiple<T>(T objectToSpawn, int amount, Vector3 offset = default) where T : MonoBehaviour
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Amount must be greater than zero.");
            return null;
        }

        if (objectToSpawn == null)
        {
            Debug.LogWarning("No object to spawn.");
            return null;
        }

        List<T> spawnedObjects = new List<T>();
        for (int i = 0; i < amount; i++)
        {
            T spawnedObject = SpawnSingle(objectToSpawn, offset);
            if (spawnedObject != null)
            {
                spawnedObjects.Add(spawnedObject);
            }
            else
            {
                Debug.LogWarning($"Failed to spawn object {i + 1}.");
            }
        }

        if (spawnedObjects.Count == 0)
        {
            Debug.LogWarning("No objects were spawned.");
            return null;
        }
        return spawnedObjects;
    }

    /// <summary>
    /// Spawns multiple instances of objects from a list of spawnable objects at random positions within
    /// the defined spawn area or at one of the defined spawn points.
    /// </summary>
    /// <typeparam name="T">The type of the object to spawn, must be a MonoBehaviour.</typeparam>
    /// <param name="spawnableObjects">An array of objects to choose from for spawning.</param>
    /// <param name="amount">The number of objects to spawn.</param>
    /// <param name="offset">An optional offset to apply to the spawn position.</param>
    /// <returns>A list of spawned objects.</returns>
    public List<T> SpawnMultipleFromList<T>(T[] spawnableObjects, int amount, Vector3 offset = default) where T : MonoBehaviour
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Amount must be greater than zero.");
            return null;
        }

        if (spawnableObjects == null || spawnableObjects.Length == 0)
        {
            Debug.LogWarning("No objects to spawn.");
            return null;
        }

        List<T> spawnedObjects = new List<T>();
        for (int i = 0; i < amount; i++)
        {
            T spawnedObject = SpawnSingleFromList(spawnableObjects, offset);
            if (spawnedObject != null)
            {
                spawnedObjects.Add(spawnedObject);
            }
            else
            {
                Debug.LogWarning($"Failed to spawn object {i + 1} from list.");
            }
        }

        if (spawnedObjects.Count == 0)
        {
            Debug.LogWarning("No objects were spawned.");
            return null;
        }
        return spawnedObjects;
    }

    /// <summary>
    /// Gets a random spawn position based on the defined spawn type.
    /// If the spawn type is AREA, it returns a random position within the defined area.
    /// If the spawn type is POINTS, it returns a random position from the defined spawn points.
    /// </summary>
    /// <returns>A random position for spawning.</returns>
    private Vector3 GetSpawnPosition()
    {
        if (Type == SpawnType.AREA)
        {
            return new Vector3(
                Random.Range(-SpawnAreaSize.x / 2, SpawnAreaSize.x / 2),
                this.transform.position.y,
                Random.Range(-SpawnAreaSize.z / 2, SpawnAreaSize.z / 2)
            );
        }
        else if (Type == SpawnType.POINTS)
        {
            if (SpawnPoints != null && SpawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, SpawnPoints.Length);
                return SpawnPoints[randomIndex].position;
            }
            else
            {
                Debug.LogWarning("No spawn points defined.");
            }
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Draws gizmos in the editor to visualize the spawn area or points.
    /// This method is called when the object is selected in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (Type == SpawnType.AREA)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, SpawnAreaSize);
        }
        else if (Type == SpawnType.POINTS)
        {
            Gizmos.color = Color.blue;
            foreach (Transform point in SpawnPoints)
            {
                if (point == null) continue;
                Gizmos.DrawSphere(point.position, .5f);
            }
        }
    }
}