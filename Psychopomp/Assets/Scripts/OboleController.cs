using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OboleController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "OboleContainer")
        {
            this.transform.parent = collision.gameObject.transform.parent;
            FindObjectOfType<AudioManager>().play("Obole");
        }
    }
}
