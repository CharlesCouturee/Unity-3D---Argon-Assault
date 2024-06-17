using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    [SerializeField] GameObject hitFX;
    [SerializeField] int scorePerHit = 15;
    [SerializeField] int hitPoints = 3;

    Scoreboard scoreboard;
    GameObject parentGameObject;

    private void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
        parentGameObject = GameObject.FindWithTag("SpawnAtRuntime");
        AddRigidBody();
    }

    private void AddRigidBody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();

        if (hitPoints < 1)
        {
            KillEnemy();
        }   
    }

    private void KillEnemy()
    {
        scoreboard.IncreaseScore(scorePerHit);
        GameObject VFX = Instantiate(deathFX, transform.position, Quaternion.identity);
        VFX.transform.parent = parentGameObject.transform;

        Destroy(gameObject);
    }

    private void ProcessHit()
    {
        GameObject VFX = Instantiate(hitFX, transform.position, Quaternion.identity);
        VFX.transform.parent = parentGameObject.transform;

        hitPoints--;
        
    }
}
