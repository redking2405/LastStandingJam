﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameManager : Singleton<GameManager>
{

    public Hunter player1;
    public Hunter player2;
    private Player player;


    private void Awake()
    {
        ReInput.ControllerConnectedEvent += OnControllerConnected;

        foreach(Joystick j in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(j)) continue;

            AssignJoystickToNextOpenPlayer(j);
        }
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
        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
