using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    [SerializeField] float duration;

    public void moveFromTo(Transform pointA, Transform pointB, float duration)
    {
        StartCoroutine(moveFromToCoroutine(pointA.position, pointB.position, duration));
    }

    IEnumerator moveFromToCoroutine(Vector3 start, Vector3 end, float time)
    {
        float elapsedTime = 0f;
        start.y = transform.position.y;
        end.y = transform.position.y;

        while (elapsedTime < time)
        {
            Vector3 newPosition = Vector3.Lerp(start, end, (elapsedTime / time));
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the movement is completed by setting the final position to the target
        Vector3 finalPosition = end;
        finalPosition.y = transform.position.y;
        transform.position = finalPosition;
    }
}
