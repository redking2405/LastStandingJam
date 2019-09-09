using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

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
    public int playerID;
    Vector2 position;


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

        if (player.GetButton("Modifier"))
        {
            isAiming = true;
        }
        else isAiming = false;

    }

    public void Shoot()
    {
        Debug.Log("Pan!");
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
