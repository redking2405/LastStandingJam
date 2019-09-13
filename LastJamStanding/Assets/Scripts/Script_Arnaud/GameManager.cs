using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityStandardAssets._2D;

public class GameManager : Singleton<GameManager>
{
    private Player player;

    public GameObject[] players;
    public GameObject[] nPCs;
   
    public int maxCloneCount = 30;
    public int currentCloneCount = 0;
    public int startSpawnCloneCount;
    public GameObject[] clones;
    public Sprite[] wp_Sprites;
    public Vector2[] respawnPoints;

    // scene collider
    private EdgeCollider2D edges;
    private Vector2[] newVerticies = new Vector2[5];
    public float screenXmax, screenYmax;

    private void Awake()
    {
        ReInput.ControllerConnectedEvent += OnControllerConnected;

        foreach(Joystick j in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(j)) continue;

            AssignJoystickToNextOpenPlayer(j);
        }

        // Setting up edges points
        edges = gameObject.AddComponent<EdgeCollider2D>();
        screenYmax = Camera.main.orthographicSize;
        screenXmax = Camera.main.aspect * screenYmax;
        newVerticies[0] = new Vector2(screenXmax, screenYmax);
        newVerticies[1] = new Vector2(-screenXmax, screenYmax);
        newVerticies[2] = new Vector2(-screenXmax, -screenYmax);
        newVerticies[3] = new Vector2(screenXmax, -screenYmax);
        newVerticies[4] = new Vector2(screenXmax, screenYmax);
        edges.points = newVerticies;
    }
   
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (args.controllerType != ControllerType.Joystick) return; // skip if this isn't a Joystick

        // Assign Joystick to first Player that doesn't have any assigned
        AssignJoystickToNextOpenPlayer(ReInput.controllers.GetJoystick(args.controllerId));
    }

    void AssignJoystickToNextOpenPlayer(Joystick j)
    {
        foreach (Player p in ReInput.players.Players)
        {
            if (p.controllers.joystickCount > 0) continue; // player already has a joystick
            p.controllers.AddController(j, true); // assign joystick to player
            return;
        }
    }

    

    void Start()
    {
        InstantiateCrowd();
    }


    private void Update()
    {
        if (currentCloneCount != startSpawnCloneCount)
        {
            int ranNum = Mathf.RoundToInt(Random.Range(0, nPCs.Length));
            Vector2 r_Postition = new Vector2(Random.Range((int)-screenXmax, (int)screenXmax), Random.Range((int)-screenYmax, (int)screenYmax));
            InstantiateClone(nPCs[ranNum], r_Postition, Random.Range(0, 2) == 0);
        }
    }


    void InstantiateCrowd()
    {
        int count = startSpawnCloneCount;
        //nPcClone.transform.position = new Vector2(screenXmax, screenYmax);
        for (int i = 0; i < count; i++)
        {
            int ranNum = Mathf.RoundToInt(Random.Range(0, nPCs.Length));
            Vector2 r_Postition = transform.position = respawnPoints[Random.Range(0, respawnPoints.Length)];
            InstantiateClone(nPCs[ranNum], r_Postition, Random.Range(0, 2) == 0);
        }
    }

    public void InstantiateClone(GameObject kid, Vector2 position, bool isRight)
    {
        var newClone = Instantiate(kid, position, Quaternion.identity, transform);
        
        currentCloneCount++;
        if (maxCloneCount < currentCloneCount)
        {
            int randomArrayId = Random.Range(0, currentCloneCount);
            clones[randomArrayId].GetComponent<AIBehaviour>().Death(); 
            currentCloneCount--;
            clones[randomArrayId] = newClone;
        }
        else
        {
            GameObject[] tempArray = clones;
            clones = new GameObject[currentCloneCount];
            tempArray.CopyTo(clones, 0);
            clones[currentCloneCount - 1] = newClone;
        }
        if (!isRight)
            newClone.GetComponent<Character2D>().Flip();
    }

    public void InstantiateWeapon(Vector2 position)
    {
        GameObject weaponI = new GameObject("weapon", typeof(SpriteRenderer));
        weaponI.tag = "Weapon";
        weaponI.transform.position = position;
        weaponI.GetComponent<SpriteRenderer>().sprite = wp_Sprites[Random.Range(0, wp_Sprites.Length)];
        weaponI.AddComponent(typeof(PolygonCollider2D));
        weaponI.GetComponent<PolygonCollider2D>().isTrigger = true;
        weaponI.AddComponent<WeaponAttack>();
    }

    public void RemoveJoystickFromPlayer(Player player1, Player player2)
    {
        //player1.controllers;
    }
}
