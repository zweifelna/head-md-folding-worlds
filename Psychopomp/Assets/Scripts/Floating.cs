using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float height = 1f;
    Vector3 pos;


    public void getPos()
    {
        pos = transform.position;
    }

    public void floating()
    {
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
