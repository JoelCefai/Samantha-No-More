using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    public GameObject Player;
    public AudioClip[] footsounds;
    public Transform Eyes;
    public AudioSource growl;
    public GameObject deathCam;
    public Transform camPos;

    private UnityEngine.AI.NavMeshAgent nav;
    private AudioSource sound;
    private Animator anim;
    private string state = "Idle";
    private bool alive = true;
    private float wait = 0f;
    private bool highAlert = false;
    private float alertness = 10f;


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
              print("hit" + rayHit.collider.gameObject.name);
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
                Vector3 randomPos = Random.insideUnitSphere * alertness;
                NavMeshHit navHit;
                NavMesh.SamplePosition(transform.position + randomPos, out navHit, 10f,NavMesh.AllAreas);

                //Go near the Player//
                if(highAlert)
                {
                    NavMesh.SamplePosition(Player.transform.position + randomPos, out navHit, 0f, NavMesh.AllAreas);

                    alertness += 5f;

                    if (alertness > 10f)
                    {
                        highAlert = false;
                        nav.speed = 1.2f;
                        anim.speed = 1.2f;
                    }
                }

                nav.SetDestination(navHit.position);
                state = "Walking";
            }
            if(state == "Walking")
            {
                if(nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                {
                    state = "Search";
                    wait = 5f;
                }
            }
            if(state == "Search")
            {
                if(wait > 0f)
                {
                    wait -= Time.deltaTime;
                    transform.Rotate(0f, 120f * Time.deltaTime, 0f);
                }
                else
                {
                    state = "Idle";
                }
            }

            if(state == "Chase")
            {
                nav.destination = Player.transform.position;
            }
           nav.SetDestination(Player.transform.position);
            float distance = Vector3.Distance(transform.position, Player.transform.position);
            if(distance > 10f)
            {
                state = "Hunt";
            }

            else if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
            {
                if (Player.GetComponent<PlayerSight>().alive)
                {
                    state = "kill";
                    Player.GetComponent<PlayerSight>().alive = false;
                    Player.GetComponent<FirstPersonController>().enabled = false;
                    deathCam.SetActive(true);
                    deathCam.transform.position = Camera.main.transform.position;
                    deathCam.transform.rotation = Camera.main.transform.rotation;
                    Camera.main.gameObject.SetActive(false);
                    growl.pitch = 0.7f;
                    growl.Play();
                    Invoke("reset", 1f);
                }
            }

        }
        if (state == "Hunt")
        {
            if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
            {
                state = "Search";
                wait = 5f;
                highAlert = true;
                alertness = 5f;
                CheckSight();
            }
        }
    }
    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
