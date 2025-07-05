using UnityEngine;

public class Player : MonoBehaviour
{
    public float RotationSpeed = 1;
    public float WalkSpeed = 1;
    public float MaxVelocity = 20;
    public float JumpForce = 20;
    public float RaycastDistance = 1;
    public float LinearDamping = 10;
    public float BulletSpeed = 5.0f;
    public LayerMask GroundLayer;
    public GameObject BulletPrefab;

    private Rigidbody m_rigidbody;

    private int m_groundLayer;
    public bool m_isGrounded;

    private GameObject m_shootingPoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       m_rigidbody = GetComponent<Rigidbody>();
       m_groundLayer = LayerMask.NameToLayer("Default");
       m_shootingPoint = GameObject.Find("ShootingPoint");

    }

    // Update is called once per frame
    void Update()
    {
        // Rotation of player
     
        //Movement
        if (Input.GetKey(KeyCode.W))
        {
            m_rigidbody.AddForce(transform.forward * WalkSpeed, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_rigidbody.AddForce(-transform.forward * WalkSpeed, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_rigidbody.AddForce(transform.right * WalkSpeed, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.A))
        {
            m_rigidbody.AddForce(-transform.right * WalkSpeed, ForceMode.Impulse);
        }

        if (m_rigidbody.linearVelocity.magnitude > MaxVelocity)
        {
            m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.normalized * MaxVelocity;
        }

        m_isGrounded = Physics.Raycast(transform.position, Vector3.down, RaycastDistance, GroundLayer);

        if (m_isGrounded)
        {
            m_rigidbody.linearDamping = LinearDamping;
        }
        else
        {
            m_rigidbody.linearDamping = LinearDamping * 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
        {
            m_rigidbody.AddForce(Vector3.up * JumpForce);
        }

        if(Input.GetMouseButtonDown(0))
        {
            GameObject go = GameObject.Instantiate(BulletPrefab);
            go.transform.position = m_shootingPoint.transform.position;
            var rigidBody = go.GetComponent<Rigidbody>();
            rigidBody.AddForce(transform.forward * BulletSpeed, ForceMode.Impulse);
        }


    }
}
