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

    private NavMeshAgent m_agent;

    private bool m_haveDestination = false;
    private GameObject m_player;
    private Vector3 m_destination;

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
       // Vector3 point;
       // if (RandomPoint(transform.position, SearchRadius, out point) && !m_haveDestination)
       // {
       //     m_destination = point;
       //     m_agent.SetDestination(point);
       //     m_haveDestination = true;
       //     Debug.DrawRay(point, Vector3.up, UnityEngine.Color.blue, 1.0f);
       // }
       //
       // if (m_haveDestination)
       // {
       //     float distance = Vector3.Magnitude(m_destination - transform.position);
       //     if (distance < 2)
       //     {
       //         m_haveDestination = false;
       //         Debug.Log("I arrived at my destination");
       //     }
       // }


        //Raycast to the player
        Vector3 directionToPlayer = m_player.transform.position - transform.position;
        Debug.DrawRay(transform.position, directionToPlayer, UnityEngine.Color.red);
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, SightDistance, RaycastMask))
        {
            if(hit.collider.gameObject.tag == "Player")
                Debug.Log("I can see the player");
        }
        //If we see the player, change state to chase
    }


    void Update()
    {
        switch(m_enemyState)
        {
            case EnemyStates.PATROLLING:
                PatrolStateBehavior();
                break;
            case EnemyStates.ATTACKING:
                break;

            case EnemyStates.CHASING:
                break;
        }      

    }
}
