using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoundArea : MonoBehaviour
{
    public Vector3 minBoundaries; // Minimum boundary position
    public Vector3 maxBoundaries; // Maximum boundary position
    public float rotationSpeed = 5.0f; // Speed of rotation when outside boundaries

    private void Update()
    {
        // Get the player's current position
        Vector3 playerPosition = transform.position;

        // Check if the player is outside the boundaries
        if (playerPosition.x < minBoundaries.x || playerPosition.x > maxBoundaries.x ||
            playerPosition.y < minBoundaries.y || playerPosition.y > maxBoundaries.y ||
            playerPosition.z < minBoundaries.z || playerPosition.z > maxBoundaries.z)
        {
            // Calculate the direction to rotate towards the boundaries
            Vector3 targetDirection = Vector3.zero;
            if (playerPosition.x < minBoundaries.x)
                targetDirection += Vector3.right;
            else if (playerPosition.x > maxBoundaries.x)
                targetDirection -= Vector3.right;

            if (playerPosition.y < minBoundaries.y)
                targetDirection += Vector3.up;
            else if (playerPosition.y > maxBoundaries.y)
                targetDirection -= Vector3.up;

            if (playerPosition.z < minBoundaries.z)
                targetDirection += Vector3.forward;
            else if (playerPosition.z > maxBoundaries.z)
                targetDirection -= Vector3.forward;

            // Rotate the player smoothly towards the boundaries
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
