using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    NavMeshAgent nav;
    GameObject Player;
    Vector3 positionToGo;
    Animator animator;
    bool shooting;
    RaycastHit hit;

    GameObject effect;

    void Start()
    {
        nav = transform.gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        animator = transform.gameObject.GetComponent<Animator>();
        effect = gameObject.transform.GetChild(2).transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < 25)
        {
            transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
            positionToGo = new Vector3(Player.transform.position.x - 5, Player.transform.position.y, Player.transform.position.z - 5f);
            if (Vector3.Distance(Player.transform.position, transform.position) > 10)
            {
                nav.SetDestination(positionToGo);
            }

        }
        if ((nav.velocity.magnitude / nav.speed) > 0) // zjiöùuje rychlost pohybu nep¯Ìtele
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
        if (Vector3.Distance(transform.position, Player.transform.position) < 10 && !shooting)
        {
            InvokeRepeating("Shoot", 0, 2);
            shooting = true;
        }
        if (Vector3.Distance(transform.position, Player.transform.position) > 10)
        {
            shooting = false;
            CancelInvoke();
        }
    }

    public void Shoot()
    {
        Ray ray = new Ray();
        ray.origin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        ray.direction = (Player.transform.position - new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
        Debug.Log("Shoot");
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.2f), (Player.transform.position - new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.2f)) * 10, Color.yellow, 2f);
        if (Physics.Raycast(ray, out hit))
        {
            transform.GetComponent<AudioSource>().Play();
            effect.GetComponent<ParticleSystem>().Play();
            Invoke("StopEffect", .15f);
            if (Random.Range(0, 2) == 1 && hit.transform.tag == "Player")
            {
                hit.transform.gameObject.GetComponent<PlayerHealth>().health -= 10;
            }
        }

    }

    public void StopEffect()
    {
        effect.GetComponent<ParticleSystem>().Stop();
    }

}