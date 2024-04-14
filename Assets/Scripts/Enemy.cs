using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float health { get; set; }
    public float maxHealth = 1;

    public ParticleSystem explosionParticles;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Kill()
    {
        //Explode
        Debug.Log("Kill called and enemy has this health listed: " + health.ToString());
        Instantiate(explosionParticles, this.transform.position, transform.rotation);
        Die();

    }

    public void Damage(float damageTaken)
    {
        Debug.Log("Enemy taking damage: " + damageTaken.ToString() + " out of " + health.ToString() + " remaining.");
        if (damageTaken > 0) health = Mathf.Clamp(health - damageTaken, 0, maxHealth);

        if (health <= 0) Kill();
    }

    public void Heal(float damageHealed)
    {
        //throw new System.NotImplementedException();
    }

    public void Die()
    {
        Destroy(gameObject);
        return;
    }
}
