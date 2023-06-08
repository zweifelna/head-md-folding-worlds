using UnityEngine;
using System.Collections;

public class Rowing : MonoBehaviour
{
    [SerializeField] Transform RightHand;
    [SerializeField] Transform Paddle;
    [SerializeField] Transform Boat;

    private Vector3 previousHandPosition;
    private bool isInWater = true;

    void Update()
    {
        Debug.Log(RightHand.position);

        Paddle.position = RightHand.position;
        Paddle.rotation = RightHand.rotation;

        Vector3 pullDirection = Paddle.position - previousHandPosition;
        float pullForce = pullDirection.magnitude / Time.deltaTime;

        if (isInWater && pullDirection.z > 0)
        {
            Boat.position += Boat.forward * pullForce;
        }

        previousHandPosition = Paddle.position;

    }
}