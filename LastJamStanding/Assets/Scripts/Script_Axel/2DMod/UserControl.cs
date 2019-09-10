using System;
using UnityEngine;
using Rewired;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (Character2D))]
    public class UserControl : MonoBehaviour
    {
        private Character2D m_Character;
        public Player player;
        public GameObject prefab;
        public GameObject instance;
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
        private void Awake()
        {
            player = ReInput.players.GetPlayer(playerID);

            m_Character = GetComponent<Character2D>();
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            //Vector2 h = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
            Vector2 h = new Vector2(player.GetAxis("Move Horizontaly"),player.GetAxis("Move Verticaly"));
            //print(player.GetAxis2D("Move Horizontaly", "Move Verticaly"));
            // Pass all parameters to the character control script.
            // m_Character.Move(h);
            
            m_Character.Move(h.magnitude,h);
            if (player.GetButtonDown("CloneVert"))
            {
                //spawn on game manager
                //GameManager.Instance.InstanciateClone
                instance = Instantiate(prefab, Vector2.zero, Quaternion.identity, null);
                instance.GetComponent<Character2D>().SetColor(m_Character.color[0]);
            }
            if (player.GetButtonDown("CloneJaune"))
            {
                //spawn on game manager
                instance = Instantiate(prefab, Vector2.zero, Quaternion.identity, null);
                instance.GetComponent<Character2D>().SetColor(m_Character.color[1]);
            }
            if (player.GetButtonDown("CloneBleu"))
            {
                //spawn on game manager
                instance = Instantiate(prefab, Vector2.zero, Quaternion.identity, null);
                instance.GetComponent<Character2D>().SetColor(m_Character.color[2]);
            }
            if (player.GetButtonDown("CloneRouge"))
            {
                //spawn on game manager
                instance = Instantiate(prefab, Vector2.zero, Quaternion.identity, null);
                instance.GetComponent<Character2D>().SetColor(m_Character.color[3]);
            }
        }
    }
}
