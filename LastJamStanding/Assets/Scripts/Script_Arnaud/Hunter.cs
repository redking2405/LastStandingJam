using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityStandardAssets._2D;
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


    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
    }
    // Start is called before the first frame update
    void Start()
    {
        originalTimeMoving = timeMoving;
        canMove = true;
        canShoot = true;
        numTimeMissed = 0;
        alpha = GetComponent<SpriteRenderer>().color;
        original = alpha;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeMoving <= 0 && canMove)
        {
            canMove = false;
            canShoot = false;
            StartCoroutine(WaitForBreath());
        }
        timeMoving -= Time.deltaTime;

        if(canShoot && player.GetButtonDown("Fire"))
        {
            Shoot();
        }

        if (!canShoot)
        {
            Color tmp;
            tmp = alpha;
            tmp.a = 0.5f;
            alpha = tmp;
        }

        else
        {
            alpha = original;
        }

        if (player.GetButton("Modifier"))
        {
            isAiming = true;
        }
        else isAiming = false;

    }

    public void Shoot()
    {
        Debug.Log("Pan!");
       
        Ray ray = new Ray(transform.position, Vector3.back*1000);
        
        Physics.Raycast(ray,out RaycastHit hit, 1000);

        Debug.Log(ray.origin);
        Debug.Log(ray.direction);
        Debug.DrawLine(transform.position, Vector3.forward, Color.red, 9999999);

        if (hit.collider.gameObject.transform.parent.gameObject.tag == "Prey")
        {
            Debug.Log("Bam t'es mort");
            GameManager.Instance.Switch(this, hit.collider.GetComponentInParent<UserControl>());
        }

        if (hit.collider.gameObject.transform.parent.gameObject.tag != "Prey")
        {
            numTimeMissed++;
            canShoot = false;
        }

        

    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(numTimeMissed * baseTimeForReload);
        canShoot = true;
    }

    IEnumerator WaitForBreath()
    {

        yield return new WaitForSeconds(timeStopping);
        canShoot = true;
        canMove = true;
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
