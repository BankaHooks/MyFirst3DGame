using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    private Vector3 offset = new Vector3(0,2,-5);

    private Vector3 smoothVelocity = Vector3.zero;

    [Header("Mouse Settings")]
    [SerializeField]
    private float mouseSensitivity = 3f;
    [SerializeField]
    private float minVerticalAngle = -30f;
    [SerializeField]
    private float maxVerticalAngle = 60f;

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        if(target == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        currentRotationX += mouseX;
        currentRotationY -= mouseY;

        currentRotationY = Mathf.Clamp(currentRotationY,minVerticalAngle,maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(currentRotationY,currentRotationX,0);


        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;

        transform.LookAt(target);
    }
}
