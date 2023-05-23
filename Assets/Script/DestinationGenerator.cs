using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationGenerator : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject destinationPrefab;
    public int numberOfDestinations;

    private List<Vector3> destinationPositions = new List<Vector3>();
    private int destinationsGenerated = 0;

    void Start()
    {
        // Generate the initial destination
            GenerateDestination();

        // Generate additional destinations until we have enough
        while (destinationsGenerated < numberOfDestinations)
        {
            // Pick a random position from our list of destination positions
            int randomIndex = Random.Range(0, destinationPositions.Count);
            Vector3 position = destinationPositions[randomIndex];

            // Generate a new destination at that position
            GenerateDestination(position);

            // Remove the position from our list so we don't generate a destination there again
            destinationPositions.RemoveAt(randomIndex);
        }
    }

    void GenerateDestination(Vector3 position = default)
    {
        // If no position is specified, generate a destination randomly within the maze
        if (position == default)
        {
            do
            {
                position = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
            }
            while (!IsValidPosition(position));
        }

        // Instantiate the destination prefab at the chosen position
        GameObject destination = Instantiate(destinationPrefab, position, Quaternion.identity);
        destination.transform.parent = transform;

        // Update our tracking variables
        destinationPositions.Add(position);
        destinationsGenerated++;
    }

    bool IsValidPosition(Vector3 position)
    {
        // Check if the position is too close to the player or another destination
        if (Vector3.Distance(position, playerTransform.position) < 5) return false;
        foreach (Vector3 destinationPosition in destinationPositions)
        {
            if (Vector3.Distance(position, destinationPosition) < 5) return false;
        }

        // Check if the position is within the maze bounds (assuming maze walls are at x = +/- 50 and z = +/- 50)
        if (position.x < -4f || position.x > 4f || position.z < -4f || position.z > 4f) return false;

        // If all checks pass, the position is valid
        return true;
    }
}
