using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float Movespeed = 10;
    public bool EnableAI = false;
    public float SearchRadius = 20f;
    public float SightDistance = 20;
    public LayerMask RaycastMask;
    public GameObject BulletPrefab;
    public float ShootRate = 0.5f;

    private NavMeshAgent m_agent;

    private bool m_haveDestination = false;
    private GameObject m_player;
    private Vector3 m_destination;

    private float Health = 10;
    private GameObject m_enemyShootingPoint;
    private float m_timer = 0;

    public enum EnemyTypes
    {
        RED,
        YELLOW
    }

    public EnemyTypes EnemyType;

    public enum EnemyStates
    {
        PATROLLING,
        CHASING,
        ATTACKING
    }

    EnemyStates m_enemyState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_player = GameObject.FindWithTag("Player");
        m_enemyState = EnemyStates.PATROLLING;
        m_enemyShootingPoint = transform.Find("EnemyShootingPoint").gameObject;
    }

    public void DecreaseHealth(float decreaseAmount)
    {
        Health -= decreaseAmount;
        if (Health < 0)
        {
            Destroy(gameObject);
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i+=1)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }


    void PatrolStateBehavior()
    {
        Vector3 point;
        if (RandomPoint(transform.position, SearchRadius, out point) && !m_haveDestination)
        {
            m_destination = point;
            m_agent.SetDestination(point);
            m_haveDestination = true;
            Debug.DrawRay(point, Vector3.up, UnityEngine.Color.blue, 1.0f);
        }
       
        if (m_haveDestination)
        {
            float distance = Vector3.Magnitude(m_destination - transform.position);
            if (distance < 2)
            {
                m_haveDestination = false;
                Debug.Log("I arrived at my destination");
            }
        }

        //Raycast to the player
        Vector3 directionToPlayer = m_player.transform.position - transform.position;
        Debug.DrawRay(transform.position, directionToPlayer.normalized * SightDistance, UnityEngine.Color.red);
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, SightDistance, RaycastMask))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                if(EnemyType == EnemyTypes.RED)
                 m_enemyState = EnemyStates.ATTACKING;
            }
        }
        //If we see the player, change state to chase
    }

    void AttackState()
    {
        m_timer += Time.deltaTime;

        m_agent.isStopped = true;
        transform.LookAt(m_player.transform.position);

        if(m_timer > ShootRate)
        {
            GameObject go = GameObject.Instantiate(BulletPrefab);
            go.transform.position = m_enemyShootingPoint.transform.position;
            var rigidBody = go.GetComponent<Rigidbody>();
            rigidBody.AddForce(transform.forward * 2.0f, ForceMode.Impulse);
            m_timer = 0;
        }      

    }


    void Update()
    {
        switch(m_enemyState)
        {
            case EnemyStates.PATROLLING:
                PatrolStateBehavior();
                break;
            case EnemyStates.ATTACKING:
                AttackState();
                break;

            case EnemyStates.CHASING:
                break;
        }      

    }



}
