using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityStandardAssets._2D;
using UnityEngine.UI;
public class Hunter : MonoBehaviour
{
    public float speed;
    public float timeMoving;
    private float originalTimeMoving;
    public float timeStopping;
    public float aimingFactor;
    private bool canMove;
    private bool canShoot;
    public bool isAiming;
    public Player player;
    [SerializeField] private int playerID;
    [SerializeField] bool targetPrey;
    [SerializeField] bool targetClone;
    bool targetNothing;
    GameObject target;
    SpriteRenderer sprite;
    public GameObject impact;
    List<GameObject> impacts = new List<GameObject>();
    public AudioSource shootSource;
    public AudioSource missSource;
    public bool isReady;
    public int GetPlayerID()
    {
        return playerID;
    }

    public void SetPlayerID(int newID)
    {
        playerID = newID;
        player = ReInput.players.GetPlayer(playerID);
    }

    Vector2 position;
    public LayerMask mask;
    int numTimeMissed;
    public float baseTimeForReload;
    Color alpha;
    Color original;
    public Image imgReload;

    public Image imgBreath;
    public Color originalColor;
    public Color breathColor;

    private void Awake()
    {
        imgReload = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        imgBreath = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        originalColor = imgBreath.color;
        breathColor = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a / 3);
        imgBreath.color = breathColor;
        player = ReInput.players.GetPlayer(playerID);
        sprite = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        originalTimeMoving = timeMoving;
        canMove = true;
        canShoot = true;
        numTimeMissed = 0;
        alpha = sprite.color;
        original = alpha;
    }

    private void FixedUpdate()
    {
        //if (canMove)
        //{
            Move();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (timeMoving <= 0 && canMove)
        {
            StartCoroutine(WaitForBreath());
        }
        if (canShoot)
        {
            timeMoving -= Time.deltaTime;
        }

        if (timeMoving > 0 && canMove)
        {
            imgBreath.fillAmount = timeMoving / originalTimeMoving;
        }
        if (canShoot && player.GetButtonDown("Fire"))
        {
            Shoot();
        }

        if (!canShoot)
        {
            Color tmp;
            tmp = alpha;
            tmp.a = 0.5f;
            alpha = tmp;
            sprite.color = alpha;
            
        }

        else
        {
            alpha = original;
            sprite.color = alpha;
        }

        if (player.GetButton("Modifier"))
        {
            isAiming = true;
        }
        else isAiming = false;

        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {

            if (collision.gameObject.tag == "Prey")
            {
                targetPrey = true;
                target = collision.gameObject;
            }

            else
            {
                if (!targetPrey)
                {
                    target = collision.gameObject;
                }
                targetClone = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (target == null)
            {
                if (collision.gameObject.tag == "Prey")
                {
                    targetPrey = true;
                    target = collision.gameObject;
                }
                else
                {
                    if (!targetPrey)
                    {
                        target = collision.gameObject;
                    }
                    targetClone = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == 8)
        {
            if (collision.gameObject.tag == "Prey")
            {
                targetPrey = false;
                target = null;

            }

            else
            {
                if (!targetPrey && collision.gameObject == target)
                {
                    target = null;
                }
                targetClone = false;
            }
        }
    }

    public void Shoot()
    {
        Debug.Log("Pan!");
        shootSource.PlayOneShot(shootSource.clip,0.1f);
        StartCoroutine("ShootCoroutine");
        /*Ray ray = new Ray(transform.position, Vector3.back*1000);

        Physics.Raycast(ray, out RaycastHit hit, 1000);

       

        Debug.Log(ray.origin);
        Debug.Log(ray.direction);
        Debug.DrawLine(transform.position, Vector3.forward, Color.red, 9999999);*/

        Instantiate(impact, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)),null);
        if (targetClone)
        {
            target.GetComponent<AIBehaviour>().Death();
            numTimeMissed++;
            canShoot = false;
            StartCoroutine(Reload());
        }

        if (targetPrey)
        {
            canShoot = false;
            GameManager.Instance.Switch(this, target.GetComponent<UserControl>());
            StartCoroutine(Reload());
        }
        
       if(!targetPrey && !targetClone)
       {
            missSource.Play();
            numTimeMissed++;
            canShoot = false;
            StartCoroutine(Reload());
       }

    }

    IEnumerator ShootCoroutine()
    {
        float timerLength = 0.2f;
        float timer = timerLength;
        Vector2 originalScale = transform.localScale;
        Vector2 swelledScale = transform.localScale * 1.25f;
        
        while (timer > 0)
        {
            transform.localScale = Vector2.Lerp(swelledScale, originalScale, timer / timerLength);
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = swelledScale;
        timerLength = 0.5f;
        timer = timerLength;
        while (timer > 0)
        {
            transform.localScale = Vector2.Lerp(originalScale, swelledScale, timer / timerLength);
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = originalScale;
    }
    
    IEnumerator Reload()
    {
        float t = 0;
        float tM = timeMoving;
        while (t < /*numTimeMissed * */ baseTimeForReload)
        {
            timeMoving = Mathf.Lerp(tM, originalTimeMoving, t / (numTimeMissed * baseTimeForReload));
            imgReload.fillAmount = t / (/*numTimeMissed * */ baseTimeForReload);
            imgBreath.fillAmount = timeMoving / originalTimeMoving;
            t += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        imgReload.fillAmount = 0;
        timeMoving = originalTimeMoving;
        canShoot = true;
    }

    IEnumerator WaitForBreath()
    {
        //canMove = false;
        canShoot = false;
        yield return new WaitForSeconds(timeStopping);
        imgBreath.color = originalColor;
        float t = 0;
        while (t < timeStopping)
        {
            imgBreath.fillAmount = t / timeStopping;
            t += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        imgBreath.color = breathColor;
        canShoot = true;
        //canMove = true;
        timeMoving = originalTimeMoving;
    }

    public void Move()
    {
        position = new Vector2(transform.position.x, transform.position.y);

        if(player.GetAxis("Move Horizontaly") !=0 || player.GetAxis("Move Verticaly") != 0)
        {
            if (isAiming)
            {
                position.x += (player.GetAxis("Move Horizontaly") * speed *Time.deltaTime) * aimingFactor;
                position.y += (player.GetAxis("Move Verticaly") * speed *Time.deltaTime) * aimingFactor;
            }


            else
            {
                position.x += (player.GetAxis("Move Horizontaly") * speed *Time.deltaTime);
                position.y += (player.GetAxis("Move Verticaly") * speed*Time.deltaTime);
            }

            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        

    }
}
