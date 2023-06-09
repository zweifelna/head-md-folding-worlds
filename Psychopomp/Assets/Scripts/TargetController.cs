using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{

    [SerializeField] bool isGap = false;
    [SerializeField] GameObject UnitPrefab;
    [SerializeField] float zOffset = 30.5f;
    public int id;
    List<GameObject> nextTargets = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boat")
        {
            Destroy(gameObject);

            if (isGap)
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
            else
            {
                // Set boat to is waiting
                BoatController.Instance.setIsWaiting(true);
            }
        }
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
