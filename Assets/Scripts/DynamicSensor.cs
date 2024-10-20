using UnityEngine;

public class DynamicSensor : MonoBehaviour
{
    public enum DetectionMode { ByTag, ByLayer }

    public DetectionMode detectionMode = DetectionMode.ByTag;  // Select detection by Tag or Layer
    public string targetTag = "Enemy";                        // Tag to detect (if using ByTag)
    public LayerMask targetLayer;                             // Layer to detect (if using ByLayer)
    public float detectionRadius = 10f;                       // Radius of detection

    [SerializeField] private Collider[] detectedObjects;      // Array to show detected objects in Inspector

    private void Update()
    {
        // Update the detected objects array every frame
        detectedObjects = Detect();
    }

    // Function to return detected objects
    public Collider[] Detect()
    {
        Collider[] objects = null;

        switch (detectionMode)
        {
            case DetectionMode.ByTag:
                objects = DetectByTag(targetTag);
                break;
            case DetectionMode.ByLayer:
                objects = DetectByLayer(targetLayer);
                break;
        }

        return objects;
    }

    // Detect objects by tag
    private Collider[] DetectByTag(string tag)
    {
        Collider[] allColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        return System.Array.FindAll(allColliders, col => col.CompareTag(tag));
    }

    // Detect objects by layer
    private Collider[] DetectByLayer(LayerMask layer)
    {
        return Physics.OverlapSphere(transform.position, detectionRadius, layer);
    }

    // Visualize detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
