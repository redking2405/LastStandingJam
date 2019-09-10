using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityStandardAssets._2D;

public class GameManager : Singleton<GameManager>
{

    public Hunter player1;
    public Hunter player2;
    private Player player;
    private GameObject nPcClone; 
    public int count;
    public int xMax;
    public int yMax;

    private void Awake()
    {
        ReInput.ControllerConnectedEvent += OnControllerConnected;

        foreach(Joystick j in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(j)) continue;

            AssignJoystickToNextOpenPlayer(j);
        }

        nPcClone = new GameObject("StartClone", typeof(Rigidbody2D), typeof(Character2D), typeof(AIBehaviour));
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

    void InstantiateCrowd()
    {
        
        
        for (int i = 0; i < count; i++)
        {
            Vector2 r_Postition = new Vector2(Random.Range(0, xMax), Random.Range(0, yMax));
            Instantiate(nPcClone, r_Postition, Quaternion.identity, transform);
        }
    }

    void Update()
    {
        
    }
}
