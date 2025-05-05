using UnityEngine;

public class ExpandingPin : MonoBehaviour
{
    [Header("Settings")]
    public float expandedRadius = 1.5f;  
    private new SphereCollider collider;
    private float originalRadius;
    private new Renderer renderer;
    private Pin pin;

    void Awake()
    {
        collider = GetComponent<SphereCollider>();
        renderer = GetComponent<Renderer>();
        pin = GetComponent<Pin>();
        
        if (collider != null)
        {
            originalRadius = collider.radius;
        }
    }

    void Update()
    {
        if (pin == null) return;

        // Immediate disappearance when knocked over
        if (pin.IsKnockedOver)
        {
            if (collider != null)
            {
                collider.radius = expandedRadius;
            }
            
            if (renderer != null && renderer.enabled)
            {
                renderer.enabled = false;
            }
        }
        else
        {
            if (collider != null)
            {
                collider.radius = originalRadius;
            }
            
            if (renderer != null && !renderer.enabled)
            {
                renderer.enabled = true;
            }
        }
    }

    void OnDestroy()
    {
        CancelInvoke();
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        // Editor-only safety checks
        if (!Application.isPlaying)
        {
            CancelInvoke();
        }
    }
    #endif
}