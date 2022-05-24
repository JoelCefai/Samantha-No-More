using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject Player;
    public AudioClip[] footsounds;
    public Transform Eyes;
    public AudioSource growl;

    private UnityEngine.AI.NavMeshAgent nav;
    private AudioSource sound;
    private Animator anim;
    private string state = "Idle";
    private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        sound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        nav.speed = 1.2f;
        anim.speed = 1.2f;
    }

    public void footstep(int _num)
        {
        sound.clip = footsounds[_num];
        sound.Play();
        }

    //check if we can see the player//
    public void CheckSight()
    {
        if (alive)
        {
            RaycastHit rayHit;
            if(Physics.Linecast(Eyes.position,Player.transform.position, out rayHit))
                {
              //print("hit" + rayHit.collider.gameObject.name);
              if(rayHit.collider.gameObject.name == "FirstPersonController")
                {
                    if(state != "kill")
                    {
                        state = "chase";
                        nav.speed = 3.5f;
                        anim.speed = 3.5f;
                        growl.pitch = 1.2f;
                        growl.Play();

                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
      //Debug.DrawLine(Eyes.position, Player.transform.position, Color.green);
        if (alive)
        {
            anim.SetFloat("velocity", nav.velocity.magnitude);
            //Idle//
            if(state == "Idle")
            {
                Vector3 randomPos = Random.insideUnitSphere * 20f;
                UnityEngine.AI.NavMeshHit navHit;
                UnityEngine.AI.NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, UnityEngine.AI.NavMesh.AllAreas);
                nav.SetDestination(navHit.position);
                state = "Walking";
            }
            if(state == "Walking")
            {
                if(nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                {
                    state = "Idle";
                }
            }

            if(state == "Chase")
            {
                nav.destination = Player.transform.position;
            }
           nav.SetDestination(Player.transform.position);
        }
    }
}
