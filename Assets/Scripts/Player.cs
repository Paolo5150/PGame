using UnityEngine;

public class Player : MonoBehaviour
{
    public float RotationSpeed = 1;
    public float WalkSpeed = 1;
    public float MaxVelocity = 20;
    public float JumpForce = 20;

    private Rigidbody m_rigidbody;

    private int m_groundLayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       m_rigidbody = GetComponent<Rigidbody>();
        m_groundLayer = LayerMask.NameToLayer("Default");

    }

    // Update is called once per frame
    void Update()
    {
        // Rotation of player
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        transform.RotateAround(Vector3.up, mouseX * RotationSpeed);

        //Movement
        if(Input.GetKey(KeyCode.W))
        {
            m_rigidbody.AddForce(transform.forward * WalkSpeed, ForceMode.Impulse);
        }

        if (m_rigidbody.linearVelocity.magnitude > MaxVelocity)
        {
            m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.normalized * MaxVelocity;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_rigidbody.AddForce(Vector3.up * JumpForce);
        }



    }
}
