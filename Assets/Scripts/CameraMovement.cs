using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject Target;
    public float FollowSpeed = 5.0f;
    public Vector3 Offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null) return;

        Vector3 desiredPosition = Target.transform.position + Offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, FollowSpeed);
        transform.LookAt(Target.transform.position);    
    }
}
