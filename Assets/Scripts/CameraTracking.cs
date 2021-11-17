using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform trackingTarget;
    public float yOffset = 2f;
    public bool isXLocked = true;
    public bool isYLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isXLocked && !isYLocked)
        {
            transform.position = new Vector3(trackingTarget.position.x, trackingTarget.position.y + yOffset, transform.position.z);
        } else if(!isXLocked && isYLocked)
        {
            transform.position = new Vector3(trackingTarget.position.x, transform.position.y, transform.position.z);
        } else if(isXLocked && !isYLocked)
        {
            transform.position = new Vector3(transform.position.x, trackingTarget.position.y + yOffset, transform.position.z);
        }
    }
}
