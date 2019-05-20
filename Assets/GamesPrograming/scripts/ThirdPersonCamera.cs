using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private const float yAngleMin = -50.0f;
    private const float yAngleMax = 50.0f;

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    public float distance = 10.0f;
    public float currentY = 21.0f;
    public float currentX = 32.0f;
    public float sensivityX = 4.0f;
    public float sensivityY = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");

        currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
