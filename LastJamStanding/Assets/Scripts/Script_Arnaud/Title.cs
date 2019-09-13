using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class Title : MonoBehaviour
{
    public enum GameMode
    {
        Prey,
        Hunter,
        Title
    }
    public AudioSource[] moops;
    public MenuController Prey, Hunter1, Hunter2;
    [HideInInspector]public GameMode currentGameMode;
    public IEnumerable<ControllerMap> maps;
    // A dictionary to look up the Map Category from the GameMode
    static Dictionary<GameMode, string> gameModeToMapCategory = new Dictionary<GameMode, string>() {
         { GameMode.Prey, "Prey" },
         { GameMode.Hunter, "Hunter" },
         { GameMode.Title,"Title" },
    };

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Prey.isReady && Hunter1.isReady && Hunter2.isReady)
        {
            ChangeGameMode(GameMode.Prey, Prey.player);
            ChangeGameMode(GameMode.Hunter, Hunter1.player);
            ChangeGameMode(GameMode.Hunter, Hunter2.player);
            int ranNun = Random.Range(0, moops.Length);
            moops[ranNun].Play();
            SceneManager.LoadScene("FinalScene");
        }
    }

    public void ChangeGameMode(GameMode mode, Player p)
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
