using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform target;
    public float speed = 1;
    public bool moveObj = true;
    private float step;

    private Vector3 startPosition;
    private bool direction = true;
    private bool goingBack = false;
    private bool locked = true;

    //this is to make the platform carry a single object, for more needs rework
    private GameObject carryTarget = null;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("direction: " + direction.ToString() + " goingBack: " + goingBack.ToString() + " locked: " + locked.ToString());
        step = 0;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (moveObj && direction)
        {
            step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
            if(carryTarget != null)
            {
                carryTarget.transform.position = Vector2.MoveTowards(carryTarget.transform.position, target.position + offset, step);
            }
        } else
        {
            step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, startPosition, step);
            if (carryTarget != null)
            {
                carryTarget.transform.position = Vector2.MoveTowards(carryTarget.transform.position, startPosition + offset, step);
            }
        }

        if (Vector2.Distance(target.position, transform.position) < Vector2.kEpsilon && !goingBack && locked)
        {
            locked = !locked;
            direction = !direction;
            goingBack = !goingBack;
        }
        else if(Vector2.Distance(target.position, transform.position) < Vector2.kEpsilon && !goingBack && !locked)
        {
            direction = !direction;
            goingBack = !goingBack;
        } else if(Vector2.Distance(startPosition, transform.position) < Vector2.kEpsilon && goingBack && !locked)
        {
            direction = !direction;
            goingBack = !goingBack;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            carryTarget = collision.gameObject;
            offset = carryTarget.transform.position - transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            carryTarget = null;
        }
    }
}
