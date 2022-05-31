using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    public GameObject Player;
    public AudioClip footsounds;
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

    public void footsteps()
    {
        sound.clip = footsounds;
        sound.Play();
        Debug.Log("AudioPlayed");
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
                UnityEngine.AI.NavMeshHit navHit;
                UnityEngine.AI.NavMesh.SamplePosition(transform.position + randomPos, out navHit, 5f,UnityEngine.AI.NavMesh.AllAreas);

                //Go near the Player//
                if(highAlert)
                {
                    UnityEngine.AI.NavMesh.SamplePosition(Player.transform.position + randomPos, out navHit, 20f, UnityEngine.AI.NavMesh.AllAreas);

                    alertness += 5f;

                    if (alertness > 5f)
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

            else if (nav.remainingDistance <= nav.stoppingDistance + 1f && !nav.pathPending)
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
                    SceneManager.LoadScene(2);
                    Cursor.lockState = CursorLockMode.None;
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

        if(state == "kill")
        {
            deathCam.transform.position = Vector3.Slerp(deathCam.transform.position, camPos.position, 10f * Time.deltaTime);
            deathCam.transform.rotation = Quaternion.Slerp(deathCam.transform.rotation, camPos.rotation, 10f * Time.deltaTime);
            anim.speed = 1f;
            nav.SetDestination(deathCam.transform.position);
        }
    }
    private void Reset()
    {
        SceneManager.LoadScene(2);
    }
}
