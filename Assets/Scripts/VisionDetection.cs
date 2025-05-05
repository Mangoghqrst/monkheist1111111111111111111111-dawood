using UnityEngine;

public class VisionDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🚨 Player detected by VisionCone!");

            // Get the PlayerHealth component and trigger death
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.DieFromVision(); // Custom function we'll add!
            }
        }
    }
}
