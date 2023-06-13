using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    private void OnDestroy()
    {
        BoatController.Instance.setIsWaiting(false);
    }

    public bool isActive = false;
    Transform lookAtTarget;
    public Transform endPosition;
    Transform soulAnchor;
    bool hasAppeared = false;
    bool isOnBoat = false;
    bool mustLeave = false;
    public bool mustEmbark = false;

    [SerializeField] GameObject obolePrefab;

    void Update()
    {
        soulAnchor = getSoulAnchor();
    }

    IEnumerator toggleIsOnBoatCoroutine(bool value)
    {
        yield return new WaitForSeconds(2f);
        isOnBoat = value;
    }

    IEnumerator dropOboleCoroutine()
    {
        yield return new WaitForSeconds(2f);

        // Drop obole with a random rotation;
        GameObject obole = Instantiate(obolePrefab, transform.position, Quaternion.identity);
    }

    IEnumerator appearCoroutine()
    {
        yield return new WaitForSeconds(2f);
        hasAppeared = true;
        Debug.Log("Appearing");
    }

    IEnumerator disapearCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    IEnumerator startToLeaveCoroutine()
    {
        yield return new WaitForSeconds(2f);
        mustLeave = true;
    }

    public void lookAt(Transform target)
    {
        //Find the Pivot child gameobject
        Transform pivot = transform.Find("Pivot");

        // Calculate the direction vector from the current object's position to the target's position
        Vector3 direction = target.position - pivot.position;
        direction.Normalize();

        // Calculate the rotation needed to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Use slerp to smoothly interpolate between the current rotation and the target rotation
        float rotationSpeed = 3f;
        pivot.rotation = Quaternion.Slerp(pivot.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void keepPosition(Transform position)
    {
        // Keep x and z position
        Vector3 newPosition = transform.position;
        newPosition.x = position.position.x;
        newPosition.z = position.position.z;
        transform.position = newPosition;
    }

    public void fromStartToBoat()
    {
        float duration = 2;
        StartCoroutine(fromStartToBoatCoroutine(duration));
    }

    IEnumerator fromStartToBoatCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Vector3 start = transform.position;
        Vector3 end = soulAnchor.position;
        end.y = transform.position.y;

        while (elapsedTime < duration)
        {
            Vector3 newPosition = Vector3.Lerp(start, end, (elapsedTime / duration));
            newPosition.y = transform.position.y;

            // Calculate the direction vector and set the rotation
            // Vector3 direction = (newPosition - transform.position).normalized;
            // if (direction != Vector3.zero) // Check for zero direction vector
            // {
            //     Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            //     transform.rotation = toRotation;
            // }

            transform.position = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the movement is completed by setting the final position to the target
        Vector3 finalPosition = end;
        finalPosition.y = transform.position.y;
        transform.position = finalPosition;

        // Drop Obole
        StartCoroutine(dropOboleCoroutine());

        // Toggle is on boat
        StartCoroutine(toggleIsOnBoatCoroutine(true));
    }

    IEnumerator fromBoatToEndCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Vector3 start = transform.position;
        Vector3 end = endPosition.position;
        end.y = transform.position.y;

        while (elapsedTime < duration)
        {
            Vector3 newPosition = Vector3.Lerp(start, end, (elapsedTime / duration));
            newPosition.y = transform.position.y;

            // Calculate the direction vector and set the rotation
            Vector3 direction = (newPosition - transform.position).normalized;
            if (direction != Vector3.zero) // Check for zero direction vector
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = toRotation;
            }

            transform.position = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the movement is completed by setting the final position to the target
        Vector3 finalPosition = end;
        finalPosition.y = transform.position.y;
        transform.position = finalPosition;

        // Toggle is on boat
        StartCoroutine(toggleIsOnBoatCoroutine(false));
    }

    public void FromBoatToEnd()
    {
        float duration = 2;
        StartCoroutine(fromBoatToEndCoroutine(duration));
    }

    public void appear()
    {
        StartCoroutine(appearCoroutine());
    }

    public void disapear()
    {
        StartCoroutine(disapearCoroutine());
    }

    public void prepareToLeave()
    {
        StartCoroutine(startToLeaveCoroutine());
    }

    public Transform getSoulAnchor()
    {
        Transform targetPosition = GameObject.FindGameObjectWithTag("SoulAnchor").transform;
        return targetPosition;
    }

    // public Transform getEndPosition()
    // {
    //     Transform targetPosition = GameObject.FindGameObjectWithTag("EndPosition").transform;
    //     return targetPosition;
    // }

    public Transform getLookAtTarget()
    {
        lookAtTarget = GameObject.FindGameObjectWithTag("MainCamera").transform;
        return lookAtTarget;
    }

    public bool getIsOnBoat()
    {
        return isOnBoat;
    }

    public bool getHasAppeared()
    {
        return hasAppeared;
    }

    public bool getMustLeave()
    {
        return mustLeave;
    }

    public void setEndPosition(Transform position)
    {
        endPosition = position;
    }

    public void turnOffLights()
    {
        LightController.Instance.turnOff();
    }

    public void endStartDockAnimation()
    {
        GetComponent<Animator>().enabled = false;
        mustEmbark = true;
    }
}
