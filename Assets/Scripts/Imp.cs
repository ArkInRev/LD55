using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : MonoBehaviour, IDamageable
{
    public float health { get; set; }
    public float maxHealth = 1;

    public GameObject projectile;
    public Transform muzzleTip;
    public float bulletForce = 15f;
    public float shootFreq = 3f;
    private float timeSinceLastShot = 0;
    
    public float rotSpeed = 5f;

    public ParticleSystem explosionParticles;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        Shoot();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        timeSinceLastShot += Time.fixedDeltaTime;
        if (timeSinceLastShot > shootFreq)
        {
            Shoot();
        }
    }

    public void Kill()
    {
        //Explode
        Debug.Log("Kill called and imp has this health listed: " + health.ToString());
        Instantiate(explosionParticles, this.transform.position, transform.rotation);
        Die();

    }

    public void Damage(float damageTaken)
    {
        Debug.Log("Imp taking damage: " + damageTaken.ToString() + " out of " + health.ToString() + " remaining.");
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

    public void Shoot()
    {

        GameObject bullet = Instantiate(projectile, muzzleTip.position, muzzleTip.rotation);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.AddForce(muzzleTip.forward * bulletForce, ForceMode.Impulse);
        timeSinceLastShot = 0;
    }
}
