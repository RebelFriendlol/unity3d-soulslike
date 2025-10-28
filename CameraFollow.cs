using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 15f, -15f);
    public float smoothTime = 0.6f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.LookAt(target);
    }
}

//oryginalny skrypt do kamery mniej p³ynny:
//public class CameraFollow : MonoBehaviour
//{
//    public Transform target;
//    public Vector3 offset = new Vector3(0, 15f, -15f);
//    public float smoothSpeed = 0.01f;

//    void LateUpdate()
//    {
//        Vector3 desiredPosition = target.position + offset;
//        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//        transform.position = smoothedPosition;
//        transform.LookAt(target);
//    }
//}