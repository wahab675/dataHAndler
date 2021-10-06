using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PLayerMovement : MonoBehaviour
{

    public static PLayerMovement instance;
    public GameObject[] waypoints;
    public int num = 0;

    public float minDist;
    public float speed;
    public Animator anim;

    public bool rand = false;
    public bool go = true;
    public Animator Anim;
    public Transform InstentsiatePoint;
    public GameObject FireBall;
    public Material nisghtSky;
    public Renderer minion;
    public bool KickBool;
    // audio
    public AudioSource AudioS;
    public AudioClip fireballSound;
    public AudioClip die;
    public AudioClip PlayerYelp;
    public AudioClip kick;

    // ui
    public GameObject FailedPanel;
    public GameObject AttackButton;
    public GameObject Fireworks;
    public GameObject LevelComplete;
    public GameObject tutorialPnael;
    public GameObject GameLevel;
    public GameObject KickButton;

    // Cams

    public cameraFollow camScr;
    public GameObject cam;
    public bool CamLerp;
    public Transform CamerLerpPoint;

    //
    public GameObject cv;


    public void Awake()
    {
        instance = this;

    }
    public void Start()
    {
        DataHandler.Instance.inGameData.i = 0;
        DataHandler.Instance.inGameData.coins = 0;
    }


    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, waypoints[num].transform.position);
      if(CamLerp)
        {
            cam.transform.position =  Vector3.Lerp(cam.transform.position,CamerLerpPoint.position,3 * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, CamerLerpPoint.rotation, 3 * Time.deltaTime);
            Debug.Log("cam");
        }

        if (go)
        {
            if (dist > minDist)
            {
                Move();
            }
            else
            {
                if (!rand)
                {
                    if (num + 1 == waypoints.Length)
                    {
                        //  num = 0;
                        Anim.SetBool("Run", false);

                    }
                    else
                    {
                        num++;
                    }
                }
                else
                {
                   // num = Random.Range(0, waypoints.Length);
                }
            }
        }
    }

    public void Move()
    {
        Quaternion game = Quaternion.LookRotation(waypoints[num].transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, game, 4f * Time.deltaTime);
      
        Anim.SetBool("Run",true);
        gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;
    }

    public void Attack()
    {
        Anim.SetTrigger("Attack");
        Anim.SetBool("Run",true);
    }
    public void StartGAme()
    {
       
        if (!DataHandler.Instance.inGameData.tutoril)
        {
            tutorialPnael.SetActive(true);
            DataHandler.Instance.inGameData.tutoril = true;
            GameLevel.gameObject.SetActive(false);


        }
        else
        {
            //AttackButton.transform.parent = null;
            tutorialPnael.SetActive(false);
            GameLevel.gameObject.SetActive(true);

            go = true;
        }
    }
   

    public void Fireball()
    {
        AudioS.PlayOneShot(PlayerYelp);
        GameObject ball =Instantiate(FireBall,InstentsiatePoint.position,Quaternion.identity);
        ball.GetComponent<Rigidbody>().AddForce(transform.forward *30f, ForceMode.Impulse);
        AudioS.PlayOneShot(fireballSound);
    }

    public void Kick()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Kick");
        AudioS.PlayOneShot(kick);
     
    }
    public void KickOn()
    {
        KickBool = true;
    }
    public void KickOff()
    {
        KickBool = false;
    }
    public void OnCollisionEnter(Collision other)
    {
        

        if (other.gameObject.CompareTag("Bee"))
        {
            gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse(2f);
            Destroy(other.gameObject);
            DataHandler.Instance.inGameData.i = 0;
            DataHandler.Instance.inGameData.coins = 0;

            Handheld.Vibrate();
            go = false;
            anim.SetTrigger("Death");
            Handheld.Vibrate();
            AttackButton.gameObject.SetActive(false);
            KickButton.SetActive(false);
            AudioS.PlayOneShot(die);
            StartCoroutine(LoadingLevelFailPanel());
           
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            DataHandler.Instance.inGameData.coins += 1;
            GameManager.instance.BeesCounterText.text = DataHandler.Instance.inGameData.coins.ToString();
        }
        if (other.gameObject.CompareTag("Last"))
        {
            CamLerp = true;
            camScr.player = null;
            DataHandler.Instance.inGameData.i = 0;
            DataHandler.Instance.inGameData.coins = 0;
            AttackButton.SetActive(false);
          
         //   GameManager.instance.ComPanyNameCanv.SetActive(true);
            minion.enabled = false;
            StartCoroutine(Fireworks1());

        }
    }
    IEnumerator Fireworks1()
    {
        yield return new WaitForSeconds(1f);
        Fireworks.SetActive(true);
        // StartCoroutine(LevelComplete1());
        cv.SetActive(true);
    }
    IEnumerator LevelComplete1()
    {
        yield return new WaitForSeconds(7f);
        LevelComplete.SetActive(true);

    }
    IEnumerator LoadingLevelFailPanel()
    {
        yield return new WaitForSeconds(3f);
        FailedPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
    }
}


