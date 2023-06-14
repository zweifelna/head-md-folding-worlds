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

            //Find audioManager
            GameObject audioManager = GameObject.Find("AudioManager");
            audioManager.GetComponent<AudioManager>().play("Obole");
        }
    }

    public void stopAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}
