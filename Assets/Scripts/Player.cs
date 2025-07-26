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

    private void FixedUpdate()
    {
        Vector3 movementDirection = Vector3.zero;   

        //Movement
        if (Input.GetKey(KeyCode.W)) movementDirection += transform.forward;         
        if (Input.GetKey(KeyCode.S)) movementDirection -= transform.forward;
        if (Input.GetKey(KeyCode.D)) movementDirection += transform.right;
        if (Input.GetKey(KeyCode.A)) movementDirection -= transform.right;

        movementDirection.Normalize();

        if ((movementDirection != Vector3.zero))
        {
            m_rigidbody.AddForce(movementDirection * WalkSpeed, ForceMode.Force);
        }


        Vector3 horizontalVel = new Vector3(m_rigidbody.linearVelocity.x, 0, m_rigidbody.linearVelocity.z);
        if (horizontalVel.magnitude > MaxVelocity)
        {
            horizontalVel = horizontalVel.normalized * MaxVelocity;
            m_rigidbody.linearVelocity = new Vector3(horizontalVel.x, m_rigidbody.linearVelocity.y, horizontalVel.z);
        }




    }

    // Update is called once per frame
    void Update()
    {
        // Rotation of player

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
            m_rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = GameObject.Instantiate(BulletPrefab);
            go.transform.position = m_shootingPoint.transform.position;
            var rigidBody = go.GetComponent<Rigidbody>();
            rigidBody.AddForce(transform.forward * BulletSpeed, ForceMode.Impulse);
        }

    }
}
