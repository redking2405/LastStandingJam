using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{


    public float speed;
    public float timeMoving;
    private float originalTimeMoving;
    public float timeStopping;
    public float aimingFactor;
    private bool canMove;
    private bool canShoot;
    private bool isAiming;
    Vector2 position;

    // Start is called before the first frame update
    void Start()
    {
        originalTimeMoving = timeMoving;
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
        if (isAiming)
        {
            position.x = (Input.GetAxis("Horizontal") * speed * Time.deltaTime)/aimingFactor;
            position.y = (Input.GetAxis("Vertical") * speed * Time.deltaTime)/aimingFactor;
        }
        

        else
        {
            position.x = (Input.GetAxis("Horizontal") * speed * Time.deltaTime);
            position.y = (Input.GetAxis("Vertical") * speed * Time.deltaTime);
        }

        transform.position = new Vector3(position.x, position.y, transform.position.z);

    }
}
