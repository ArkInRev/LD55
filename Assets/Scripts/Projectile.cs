using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;

    public float lifetime = 4.0f;
    private float timeAlive = 0f;
    public ParticleSystem impactParticles;

    public float damageCaused = 1f;

    void Start()
    {
        switch (this.tag)
        {
            case "playerProjectile":
                damageCaused = GameManager.Instance.GetPlayerDamage();
                break;
            case "impProjectile":
                damageCaused = GameManager.Instance.GetImpDamage();
                break;
            case "enemyProjectile":
                damageCaused = GameManager.Instance.GetEnemyDamage();
                break;
            default:
                damageCaused = GameManager.Instance.GetEnemyDamage();
                break;
        }
    }

    void FixedUpdate()
    {
        timeAlive += Time.fixedDeltaTime;
        if (timeAlive >= lifetime) Die();

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<IDamageable>() != null)
        {
            collision.collider.GetComponent<IDamageable>().Damage(damageCaused);
        }
        Instantiate(impactParticles, this.transform);
        Die();
    }

    public void Die()
    {
        Destroy(gameObject);
        return;
    }

}