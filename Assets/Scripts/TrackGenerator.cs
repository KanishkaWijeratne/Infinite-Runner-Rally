using UnityEngine;
using System.Collections.Generic;

public class TrackGenerator : MonoBehaviour
{
    public GameObject[] trackSegmentPrefabs;    // Array to hold multiple track segment prefabs
    public Transform player;                    // Player transform
    public int segmentsAhead = 8;              // Number of segments to keep ahead
    public int segmentsBehind = 4;             // Number of segments to keep behind
    public float segmentLength = 10f;          // Length of each segment
    public float spawnThreshold = 0.6f;        // Spawn new segment when player crosses this fraction of current segment
    public float heightOffset = -0.5f;         // Vertical offset for track segments
    
    private Queue<Transform> activeSegments;    // Queue for managing active segments
    private float nextSpawnZ;                  // Z position where next segment should spawn
    private Transform currentSegment;          // Reference to the segment the player is currently on
    private int totalDesiredSegments;          // Total segments to keep active (ahead + behind)

    void Start()
    {
        activeSegments = new Queue<Transform>();
        nextSpawnZ = player.position.z;
        totalDesiredSegments = segmentsAhead + segmentsBehind;

        // Initialize track segments
        for (int i = 0; i < totalDesiredSegments; i++)
        {
            SpawnNewSegment();
        }
    }

    void Update()
    {
        if (currentSegment == null && activeSegments.Count > 0)
        {
            currentSegment = activeSegments.Peek();
        }

        // Calculate how far along the current segment the player is
        float distanceAlongSegment = (player.position.z - currentSegment.position.z) / segmentLength;

        // If player has moved far enough along current segment
        if (distanceAlongSegment >= spawnThreshold)
        {
            SpawnNewSegment();
            
            // Only remove old segments if we have more than our desired total
            if (activeSegments.Count > totalDesiredSegments)
            {
                Transform oldestSegment = activeSegments.Peek();
                
                // Check if the oldest segment is far enough behind the player
                float distanceBehindPlayer = player.position.z - oldestSegment.position.z;
                if (distanceBehindPlayer > segmentLength * segmentsBehind)
                {
                    activeSegments.Dequeue();
                    currentSegment = activeSegments.Peek(); // Update current segment reference
                    Destroy(oldestSegment.gameObject);
                }
            }
        }
    }

    private void SpawnNewSegment()
    {
        // Randomly choose a track segment prefab
        GameObject selectedPrefab = trackSegmentPrefabs[Random.Range(0, trackSegmentPrefabs.Length)];
        
        // Create new segment
        GameObject newSegment = Instantiate(selectedPrefab);
        
        // Position the new segment with height offset
        newSegment.transform.position = new Vector3(0, heightOffset, nextSpawnZ);
        
        // Update nextSpawnZ for next segment
        nextSpawnZ += segmentLength;
        
        // Add to active segments queue
        activeSegments.Enqueue(newSegment.transform);
    }
}