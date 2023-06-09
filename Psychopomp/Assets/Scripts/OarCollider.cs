using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OarCollider : MonoBehaviour
{
    [SerializeField] Collider waterCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other == waterCollider)
        {
            BoatController.Instance.isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == waterCollider)
        {
            BoatController.Instance.isInWater = false;
        }
    }
}
