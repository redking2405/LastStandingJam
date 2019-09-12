using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityStandardAssets._2D;
using UnityEngine.EventSystems;
public class GameManager : Singleton<GameManager>
{
    public enum GameMode
    {
        Prey,
        Hunter
    }
    public Hunter player1;
    public Hunter player2;
    private Player player;
    public GameMode currentGameMode;
    public GameObject nPcClone, playerClone;
    [SerializeField]
    private RuntimeAnimatorController[] npcAnimatorControllers;
    public int maxCloneCount = 30;
    public int currentCloneCount = 0;
    public const int startSpawnCloneCount = 15;
    public GameObject[] clones;

    public IEnumerable<ControllerMap> maps;
    // A dictionary to look up the Map Category from the GameMode
    static Dictionary<GameMode, string> gameModeToMapCategory = new Dictionary<GameMode, string>() {
         { GameMode.Prey, "Prey" },
         { GameMode.Hunter, "Hunter" },
    };
    // scene collider
    private EdgeCollider2D edges;
    private Vector2[] newVerticies = new Vector2[5];
    private float screenXmax, screenYmax;

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
        int count = startSpawnCloneCount;
        nPcClone.transform.position = new Vector2(screenXmax, screenYmax);
        for (int i = 0; i < count; i++)
        {
            Vector2 r_Postition = new Vector2(Random.Range((int)-screenXmax, (int)screenXmax), Random.Range((int)-screenYmax, (int)screenYmax));
            InstantiateClone(r_Postition, Random.Range(0, 2) == 0, Random.Range(0, 4));
        }
    }

    public void InstantiateClone(Vector2 position, bool isRight, int animationControllerID)
    {
        var newClone = Instantiate(nPcClone, position, Quaternion.identity, transform);
        RuntimeAnimatorController animatorController = npcAnimatorControllers[animationControllerID];
        newClone.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        newClone.GetComponent<Character2D>().currentColor = animationControllerID;
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

        ChangeGameMode(GameMode.Hunter, hunter.player);
        ChangeGameMode(GameMode.Prey, prey.player);

        UserControl.Instance.Respawn();
        Timer.Instance.RestartTimer();

    }
    public void ChangeGameMode(GameMode mode,Player p)
    {
        currentGameMode = mode; // store the new game mode
        SetControllerMapsForCurrentGameMode(p); // enable the correct Controller Maps for the game mode
    }
      void SetControllerMapsForCurrentGameMode(Player p)
    {
        // Disable all controller maps first for all controllers of all types
        p.controllers.maps.SetAllMapsEnabled(false);

        // Enable maps for the current game mode for all controlllers of all types
        p.controllers.maps.SetMapsEnabled(true, gameModeToMapCategory[currentGameMode]);

    }
}
