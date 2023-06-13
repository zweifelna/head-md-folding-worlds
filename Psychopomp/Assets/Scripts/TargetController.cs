using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{

    enum TargetType
    {
        Gap,
        Start,
        End
    }

    [SerializeField] Light spotLight;
    [SerializeField] TargetType targetType;
    [SerializeField] GameObject UnitPrefab;
    [SerializeField] float zOffset = 30.5f;
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject soulPrefab;
    public int id;
    List<GameObject> nextTargets = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boat")
        {

            switch (targetType)
            {
                case TargetType.Gap:
                    handleGapEnter();
                    break;
                case TargetType.Start:
                    handleStartEnter();
                    break;
                case TargetType.End:
                    handleEndEnter();
                    break;
            }

            Destroy(gameObject);
        }
    }
    void handleGapEnter()
    {
        // Get parent component that have the tag Unit
        GameObject Unit = transform.parent.gameObject.transform.parent.gameObject;

        // Instantiate new unit with an offset relative to the unit parent
        GameObject newUnit = Instantiate(UnitPrefab, Unit.transform.position + new Vector3(0, 0, zOffset), Unit.transform.rotation);

        // Add the new unit to the UnitController
        UnitController.Instance.addUnit(newUnit);

        // Get all newUnit children that have the tag target
        findNextTargets(newUnit.transform);

        // Order the targets by their id
        nextTargets.Sort((x, y) => x.GetComponent<TargetController>().id.CompareTo(y.GetComponent<TargetController>().id));

        // Add each target to the boat target list
        foreach (GameObject target in nextTargets)
        {
            BoatController.Instance.addTarget(target);
        }
    }

    void handleStartEnter()
    {
        LightController.Instance.setLightToHandle(spotLight);
        LightController.Instance.turnOn();
        BoatController.Instance.setIsWaiting(true);
        Instantiate(soulPrefab, spawnPosition.position, Quaternion.identity);
    }

    void handleEndEnter()
    {
        LightController.Instance.setLightToHandle(spotLight);
        LightController.Instance.turnOn();

        // Make the Boat wait
        BoatController.Instance.setIsWaiting(true);

        // Find the active Soul
        SoulController soul = GameObject.FindGameObjectWithTag("Soul").GetComponent<SoulController>();

        // Find EndPosition sibling
        Transform endPosition = transform.parent.Find("EndPosition");
        soul.setEndPosition(endPosition);


        // Make the soul in its leaveBoat state
        soul.prepareToLeave();
    }

    void findNextTargets(Transform parent)
    {
        // Check each of the parent's children
        foreach (Transform child in parent)
        {
            // If this child has the desired tag, add it to the list
            if (child.tag == "Target")
            {
                nextTargets.Add(child.gameObject);
            }

            // If this child has children, recursively check them too
            if (child.childCount > 0)
            {
                findNextTargets(child);
            }
        }
    }
}
