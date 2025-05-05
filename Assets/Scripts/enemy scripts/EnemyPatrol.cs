using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float waitTime = 2f;

    private Transform targetPoint;
    private Animator animator;
    private bool isWaiting = false;
    private Vector3 originalScale;

    void Start()
    {
        animator = GetComponent<Animator>();
        targetPoint = pointB;
        originalScale = transform.localScale;
        animator.SetFloat("Blend", 0f); // Walk animation start
    }

    void Update()
    {
        if (isWaiting) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            StartCoroutine(WaitAndSwitchPoint());
        }

        FlipTowardsTarget();
    }

    void FlipTowardsTarget()
    {
        Vector3 scale = originalScale;

        if (targetPoint.position.x > transform.position.x)
            scale.x = Mathf.Abs(originalScale.x); // Face right
        else
            scale.x = -Mathf.Abs(originalScale.x); // Face left

        transform.localScale = scale;

        // Flip VisionCone using rotation, not scale!
        Transform visionCone = transform.Find("VisionCone");
        if (visionCone != null)
        {
            Vector3 vcRotation = visionCone.localEulerAngles;

            if (targetPoint.position.x > transform.position.x)
                vcRotation.y = 0f; // Facing right
            else
                vcRotation.y = 180f; // Flip facing left

            visionCone.localEulerAngles = vcRotation;
        }
    }


    IEnumerator WaitAndSwitchPoint()
    {
        isWaiting = true;
        animator.SetFloat("Blend", 1f); // Idle animation
        yield return new WaitForSeconds(waitTime);

        targetPoint = (targetPoint == pointA) ? pointB : pointA;

        FlipTowardsTarget();

        animator.SetFloat("Blend", 0f); // Resume walking
        isWaiting = false;
    }
}
