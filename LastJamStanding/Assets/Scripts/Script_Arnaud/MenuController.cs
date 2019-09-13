using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class MenuController : MonoBehaviour
{

    public GameObject impact;
    List<GameObject> impacts = new List<GameObject>();
    public AudioSource shootSource;
    public AudioSource missSource;
    public bool isReady;
    public Player player;
    [SerializeField] private int playerID;
    Vector3 center;
    Vector2 size;
    public int GetPlayerID()
    {
        return playerID;
    }

    public void SetPlayerID(int newID)
    {
        playerID = newID;
        player = ReInput.players.GetPlayer(playerID);
    }
    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
    }
    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        SpriteRenderer renderer= GetComponent<SpriteRenderer>();
        Vector3 itemSize = renderer.bounds.size;
        float pixelPerUnit = renderer.sprite.pixelsPerUnit;
        size = new Vector2(itemSize.x * pixelPerUnit, itemSize.y * pixelPerUnit);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Ready"))
        {
            Vector3 randonPos = center+new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), 0);
            impacts.Add(Instantiate(impact, center, Quaternion.Euler(0, 0, Random.Range(0, 360)), null));
            shootSource.Play();
            isReady = true;


        }

        if (player.GetButtonDown("Unready"))
        {
            for (int i = 0; i < impacts.Count; i++)
            {
                Destroy(impacts[i]);
            }
            missSource.Play();
            impacts.Clear();
            isReady = false;
        }
    }
}
