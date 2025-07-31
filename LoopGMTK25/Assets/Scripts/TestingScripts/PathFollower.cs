using UnityEngine;
using System.Collections.Generic;

public class PathFollower : MonoBehaviour
{
    public static PathFollower Instance;
    
    [System.Serializable]
    public class Follower
    {
        public Transform target;
        [HideInInspector] public float distanceTraveled = 0f;
    }

    public List<Follower> followers = new List<Follower>();              // Multiple targets to animate
    public Transform[] waypoints;            // Spline control points
    public float speed = 2f;                 // Movement speed (units per second)
    public int samplesPerSegment = 20;       // Accuracy of arc length approximation

    private List<Vector3> samples = new();
    private List<float> cumulativeDistances = new();
    private float totalLength;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        if (waypoints.Length < 4)
        {
            Debug.LogError("Need at least 4 waypoints for Catmull-Rom spline.");
            enabled = false;
            return;
        }

        PrecomputeSplineSamples();
    }

    void Update()
    {
        foreach (var follower in followers)
        {
            if (follower.target == null) continue;

            follower.distanceTraveled += speed * Time.deltaTime;

            // Loop back to start
            if (follower.distanceTraveled > totalLength)
                follower.distanceTraveled -= totalLength;

            Vector3 pos = GetPositionAtDistance(follower.distanceTraveled);
            Vector3 next = GetPositionAtDistance(follower.distanceTraveled + 0.01f);

            follower.target.position = pos;

            Vector3 direction = (next - pos).normalized;
            if (direction != Vector3.zero)
                follower.target.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    public void AddFollower(Transform newTarget)
    {
        if (newTarget == null) return;

        Follower newFollower = new Follower
        {
            target = newTarget,
            distanceTraveled = 0f
        };

        followers.Add(newFollower);
    }

    void PrecomputeSplineSamples()
    {
        samples.Clear();
        cumulativeDistances.Clear();
        totalLength = 0f;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 p0 = waypoints[WrapIndex(i - 1)].position;
            Vector3 p1 = waypoints[WrapIndex(i)].position;
            Vector3 p2 = waypoints[WrapIndex(i + 1)].position;
            Vector3 p3 = waypoints[WrapIndex(i + 2)].position;

            Vector3 prev = CatmullRom(p0, p1, p2, p3, 0f);
            if (i == 0)
            {
                samples.Add(prev);
                cumulativeDistances.Add(0f);
            }

            for (int j = 1; j <= samplesPerSegment; j++)
            {
                float t = j / (float)samplesPerSegment;
                Vector3 pt = CatmullRom(p0, p1, p2, p3, t);
                totalLength += Vector3.Distance(prev, pt);
                samples.Add(pt);
                cumulativeDistances.Add(totalLength);
                prev = pt;
            }
        }
    }

    Vector3 GetPositionAtDistance(float distance)
    {
        for (int i = 0; i < cumulativeDistances.Count - 1; i++)
        {
            float d0 = cumulativeDistances[i];
            float d1 = cumulativeDistances[i + 1];

            if (distance >= d0 && distance <= d1)
            {
                float t = Mathf.InverseLerp(d0, d1, distance);
                return Vector3.Lerp(samples[i], samples[i + 1], t);
            }
        }

        return samples[^1]; // fallback
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

    int WrapIndex(int index)
    {
        return (index + waypoints.Length) % waypoints.Length;
    }
}
