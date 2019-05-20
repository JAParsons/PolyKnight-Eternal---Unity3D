using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform PlayerTransform;
    private Vector3 camOffset;

    [Range(0.01f, 10.0f)]
    public float smoothFactor = 0.5f;

    public bool lookAtPlayer = false;
    public bool rotate = true;
    public float rotSpeed = 5.0f;

    void Start()
    {
        camOffset = transform.position - PlayerTransform.position;
    }

    void LateUpdate()
    {
        if(rotate)
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X")*rotSpeed, Vector3.up);
            camOffset = camTurnAngle * camOffset;
        }

        Vector3 newPos = PlayerTransform.position + camOffset;
        transform.position = Vector3.Slerp(PlayerTransform.position, newPos, smoothFactor);

        if(lookAtPlayer || rotate)
        {
            PlayerTransform.LookAt(PlayerTransform);
        }
    }

}