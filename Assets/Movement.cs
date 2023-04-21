using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{
    [SerializeField, Min(0)] private float speed = 10;
    [SerializeField] private bool auto;

    private int direction;
    private int prvDirection = 1;
    // Update is called once per frame
    private void Start()
    {

        if (auto)
        {
            direction = Random.Range(0, 2) == 1 ? -1 : 1;
            prvDirection = direction;
        }

    }

    void Update()
    {
        //Lazy as hell, but time crunch
        if (!auto)
        {
            if (Input.GetKey(KeyCode.A))
            {
                direction = -1;
                prvDirection = direction;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                direction = 1;
                prvDirection = direction;
            }
            else direction = 0;
        }
        
        

        if (Physics.Raycast(transform.position, transform.right * direction, 2))
        {
            if (auto) direction = -direction;
            else direction = 0;
        }

        transform.localScale = new Vector3(-prvDirection * 2, 3, 1);
        transform.position += transform.right * (Time.deltaTime * speed * direction);
    }
}
