using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OarVisualController : MonoBehaviour
{
    public Transform target; // Target to follow
    public float followSpeed = 5f; // Speed of following
    public float rotationSpeed = 5f; // Speed of rotation
    public float followDelay = 0.5f; // Delay in seconds

    private Vector3 nextPosition;
    private Quaternion nextRotation;

    private void Start()
    {
        // Start the coroutine that will periodically update the target position and rotation
        StartCoroutine(UpdateTargetCoroutine());
    }

    private void Update()
    {
        // Smoothly move and rotate towards the target position and rotation
        transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * followSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * rotationSpeed);
    }

    private IEnumerator UpdateTargetCoroutine()
    {
        while (true)
        {
            // Wait for the delay time
            yield return new WaitForSeconds(followDelay);

            // Update the next position and rotation
            nextPosition = target.position;
            nextRotation = target.rotation;
        }
    }
}
