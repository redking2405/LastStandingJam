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

    public GameObject nPcClone, playerClone;
    [SerializeField]
    private RuntimeAnimatorController[] npcAnimatorControllers;

    private void Awake()
    {
        ReInput.ControllerConnectedEvent += OnControllerConnected;

        foreach(Joystick j in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(j)) continue;

            AssignJoystickToNextOpenPlayer(j);
        }
    }
    public RuntimeAnimatorController GetAnimatorController(int i)
    {
        return npcAnimatorControllers[i];
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
        int count = 100;
        int yMax = (int)Camera.main.orthographicSize, xMax = (int)(Camera.main.aspect * yMax);
        nPcClone.transform.position = new Vector2(xMax, yMax);
        for (int i = 0; i < count; i++)
        {
            Vector2 r_Postition = new Vector2(Random.Range(-xMax, xMax), Random.Range(-yMax, yMax));
            InstantiateClone(r_Postition, Random.Range(0, 2) == 0, Random.Range(0, 4));
        }
    }

    public void InstantiateClone(Vector2 position, bool isRight, int animationControllerID)
    {
        var newClone = Instantiate(nPcClone, position, Quaternion.identity, transform);
        RuntimeAnimatorController animatorController = npcAnimatorControllers[animationControllerID];
        newClone.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        newClone.GetComponent<Character2D>().currentColor = animationControllerID;
        if (!isRight)
            newClone.GetComponent<Character2D>().Flip();
    }

    public void RemoveJoystickFromPlayer(Player player1, Player player2)
    {
        //player1.controllers;
    }
   

    public void Switch(Hunter hunter, UserControl prey)
    {
        int hunterID = hunter.GetPlayerID();
        int preyID = prey.GetPlayerID();
        //Player hunterPlayer = hunter.player;
        //Player preyPlayer = prey.player;
        hunter.SetPlayerID(preyID);
        prey.SetPlayerID(hunterID);
        Timer.Instance.RestartTimer();

    }
}
