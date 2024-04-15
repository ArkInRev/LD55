using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    private GameManager gm;
    public float health { get; set; }
    public float maxHealth = 1;

    public float firepowerDrain = 3f;
    public float firepowerRestore = 3f;


    public GameObject explosionParticles;
    // Start is called before the first frame update

    public float pitchMin = 0.8f;
    public float pitchMax = 1.2f;
    public float volumeMin = 0.25f;
    public float volumeMax = 0.75f;

    bool playerInTrigger = false;

    private NavMeshAgent agent;
    public bool isOnNavMesh;
    enum AIState { GoingToForge, GoingToImp, Deciding };

    [SerializeField]
    private AIState enemyAIState;

    public float enemyChaseSpeed = 12f;

    //EatImps and Forge
    public CapsuleCollider cc;
    public GameObject forgeGameObject;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        gm = GameManager.Instance;
        forgeGameObject = gm.Forge;
        agent = GetComponent<NavMeshAgent>();
        GameManager.Instance.onTick += onTick;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        isOnNavMesh = agent.isOnNavMesh;
        bool stateChanged = false;
        switch (enemyAIState)
        {
            case AIState.Deciding:
                // set to the target destination
                enemyAIState = AIState.GoingToForge;
                break;
            case AIState.GoingToForge:
                agent.destination = forgeGameObject.transform.position;
                break;

            case AIState.GoingToImp:
                enemyAIState = AIState.GoingToForge;
                break;

        }
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
        gm.gainFirepower(firepowerRestore);
        Destroy(gameObject);
        return;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Imp"))
        {
            // we hit an imp
            other.gameObject.GetComponent<Imp>().Damage(5);

        }

        if (other.CompareTag("Player"))
        {
            //player entered trigger
            playerInTrigger = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //player left trigger
            playerInTrigger = false;
        }
    }

    private void onTick()
    {
        if (playerInTrigger)
        {
            gm.spendFirepower(firepowerDrain);
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.onTick -= onTick;
    }
}
