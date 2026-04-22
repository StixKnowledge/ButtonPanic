using UnityEngine;

public class TargetAvoidPlayer : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public float moveSpeed = 3f;

    void Update()
    {
        Vector3 directionToPlayer = transform.position - player.position;
        float distance = directionToPlayer.magnitude;

        if (distance < detectionRadius)
        {
            Vector3 moveDirection = directionToPlayer.normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}