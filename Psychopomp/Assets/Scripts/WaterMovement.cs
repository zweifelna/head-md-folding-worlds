using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] float speed = .5f;
    [SerializeField] float radius = .5f;

    float time;
    Vector3 initialPosition;


    void Start()
    {
        time = Random.Range(0.0f, 2.0f * Mathf.PI);
        initialPosition = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime * speed;
        float x = Mathf.Cos(time) * radius;
        float y = Mathf.Sin(time) * radius;

        transform.position = new Vector3(initialPosition.x + x, initialPosition.y + y, transform.position.z);
    }

}