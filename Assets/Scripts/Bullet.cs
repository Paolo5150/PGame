using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage = 1.0f;
    float m_lifeTimer = 20;
    public float m_timer = 0;

    int m_groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_groundLayer = LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer > m_lifeTimer )
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var otherLayer = other.gameObject.layer;
        if(otherLayer == m_groundLayer)
        {
            Destroy(gameObject);
        }
        if(other.tag == "Enemy")
        {
            var enemyScript = other.GetComponent<Enemy>();
            enemyScript.DecreaseHealth(BulletDamage);
            Destroy(gameObject);
        }
        //Debug.Log("Bullet colliding with " + name);
    }


}
