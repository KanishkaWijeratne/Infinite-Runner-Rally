using UnityEngine;
using System.Collections.Generic;

public class TrackGenerator : MonoBehaviour
{
    [System.Serializable]
    public class TrackSegmentData
    {
        public GameObject prefab;
        public Vector3 offset = Vector3.zero;
    }

    public TrackSegmentData[] trackSegments;   // Array of prefabs with their offsets
    public Transform player;                    // Player transform
    public int segmentsAhead = 8;              // Number of segments to keep ahead
    public int segmentsBehind = 4;             // Number of segments to keep behind
    public float segmentLength = 10f;          // Length of each segment
    public float spawnThreshold = 0.6f;        // Spawn new segment when player crosses this fraction of current segment
    public float heightOffset = -0.5f;         // Global vertical offset for all track segments
    
    [Header("Debug Visualization")]
    public bool showCenterDebug = true;        // Toggle for debug visualization
    
    private Queue<Transform> activeSegments;    // Queue for managing active segments
    private float nextSpawnZ;                  // Z position where next segment should spawn
    private Transform currentSegment;          // Reference to the segment the player is currently on
    private int totalDesiredSegments;          // Total segments to keep active (ahead + behind)

    void Start()
    {
        if (trackSegments == null || trackSegments.Length == 0)
        {
            Debug.LogError("No track segments assigned!");
            return;
        }

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
                    currentSegment = activeSegments.Peek();
                    Destroy(oldestSegment.gameObject);
                }
            }
        }

        // Debug visualization
        if (showCenterDebug)
        {
            DrawDebugCenterLines();
        }
    }

    private void SpawnNewSegment()
    {
        // Randomly choose a track segment from the array
        TrackSegmentData segmentData = trackSegments[Random.Range(0, trackSegments.Length)];
        
        if (segmentData.prefab == null)
        {
            Debug.LogError("Track segment prefab is null!");
            return;
        }

        // Create new segment
        GameObject newSegment = Instantiate(segmentData.prefab);
        
        // Position the new segment with both height and individual prefab offsets
        Vector3 spawnPosition = new Vector3(
            segmentData.offset.x,
            heightOffset + segmentData.offset.y,
            nextSpawnZ + segmentData.offset.z
        );
        newSegment.transform.position = spawnPosition;
        
        // Update nextSpawnZ for next segment
        nextSpawnZ += segmentLength;
        
        // Add to active segments queue
        activeSegments.Enqueue(newSegment.transform);
    }

    private void DrawDebugCenterLines()
    {
        if (!Application.isEditor) return;

        foreach (Transform segment in activeSegments)
        {
            // Draw center crosshair
            Vector3 center = segment.position;
            float size = 1f;
            
            // Vertical line
            Debug.DrawLine(
                center + Vector3.up * size,
                center + Vector3.down * size,
                Color.yellow
            );
            
            // Horizontal line
            Debug.DrawLine(
                center + Vector3.left * size,
                center + Vector3.right * size,
                Color.yellow
            );
            
            // Forward/Back line
            Debug.DrawLine(
                center + Vector3.forward * size,
                center + Vector3.back * size,
                Color.yellow
            );
        }
    }

    // Optional: Method to validate track segments in editor
    private void OnValidate()
    {
        if (trackSegments != null)
        {
            foreach (var segment in trackSegments)
            {
                if (segment.prefab == null)
                {
                    Debug.LogWarning("Track segment has null prefab reference!");
                }
            }
        }
    }
}