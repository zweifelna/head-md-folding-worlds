using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class BoatController : MonoBehaviour
{
    public static BoatController Instance { get; private set; }

    [SerializeField] Collider waterCollider;
    [SerializeField] GameObject oarCollider;
    [SerializeField] List<GameObject> targets = new List<GameObject>();
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] bool debug = false;
    [SerializeField] bool isWaiting = false;

    Vector3 previousHandPosition;
    public bool isInWater = false;

    public float rowSpeed = 0.0f;
    Vector3 rowDirection = Vector3.forward;
    Vector3 directionToTarget;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isWaiting)
        {
            // lerp rowSpeed to 0 in 1 second
            rowSpeed = Mathf.Lerp(rowSpeed, 0, Time.deltaTime);
        }
        // Debug.Log(audioSource.volume);

        if (rowSpeed < 0.01f)
        {
            rowSpeed = 0.0f;
            audioSource.volume = 0.0f;
        }
        audioSource.volume = Mathf.Clamp01(rowSpeed / 3f);
    }

    public void rowingUpdate()
    {
        GameObject target = targets.Find(x => x != null);
        if (target == null) return;
        handleMovement(target);
    }

    public void faceFront()
    {
        StartCoroutine(ResetRotation());
    }

    IEnumerator ResetRotation()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
    }

    void handleMovement(GameObject target)
    {
        // Calculate the pulling force
        Vector3 pullDirection = oarCollider.transform.position - previousHandPosition;
        float pullForce = pullDirection.magnitude / Time.deltaTime * .01f;
        int forceMultiplier = 1;

        if (isInWater && pullDirection.z < 0)
        {
            // Calculate the direction to the target
            directionToTarget = (target.transform.position - transform.position);
            directionToTarget.y = 0;
            directionToTarget = directionToTarget.normalized;

            // Move the boat towards the target
            //transform.position += directionToTarget * pullForce; // douglas == bull in a china shop

            rowSpeed += pullForce * forceMultiplier;

            // Rotate the boat to face the target
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, Time.deltaTime * 2.0f); // The number 2.0f represents the speed of rotation
        }

        transform.position += directionToTarget * rowSpeed * Time.deltaTime;

        rowSpeed *= 0.99f;

        if (Input.GetKeyDown(KeyCode.Space) && debug)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position);
            directionToTarget.y = 0;
            directionToTarget = directionToTarget.normalized;

            // Move the boat towards the target
            transform.position += directionToTarget * 1.5f;

            // Rotate the boat to face the target
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, Time.deltaTime * 2.0f); // The number 2.0f represents the speed of rotation
        }

        previousHandPosition = oarCollider.transform.position;
    }

    // IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    // {
    //     float startVolume = audioSource.volume;

    //     while (audioSource.volume > 0)
    //     {
    //         audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
    //         yield return null;
    //     }

    //     audioSource.Stop();
    //     audioSource.volume = startVolume;
    // }

    public void addTarget(GameObject target)
    {
        targets.Add(target);
    }

    public bool getIsWaiting()
    {
        return isWaiting;
    }

    public void setIsWaiting(bool value)
    {
        isWaiting = value;
    }
}