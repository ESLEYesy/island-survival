// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [HelpURL("https://vis2k.github.io/Mirror/Components/NetworkManagerHUD")]
    public class NetworkManagerHUD : MonoBehaviour
    {
        NetworkManager manager;
        public bool showGUI = true;
        public int offsetX;
        public int offsetY;

        // Custom UI elements
        public InputField networkAddress;
        public Button hostButton;
        public Button clientButton;
        public Button serverButton;
        //public Button quitButton;
        public bool hostButtonClicked, clientButtonClicked, serverButtonClicked, quitButtonClicked;
        public GameObject networkMenu;
        public GameObject player;
        // Array to store all players
        public GameObject[] players;
        public bool networkMenuActive;

        // void Awake()
        // {
        //     manager = GetComponent<NetworkManager>();
        // }
        void Start()
        {
            hostButtonClicked = clientButtonClicked = serverButtonClicked = quitButtonClicked = false;
            hostButton.onClick.AddListener(HostButtonClicked);
            clientButton.onClick.AddListener(ClientButtonClicked);
            serverButton.onClick.AddListener(ServerButtonClicked);
            networkMenuActive = true;

            // Get player's quit button
            // Grab all players
            // players = GameObject.FindGameObjectsWithTag("Player");
            // foreach (GameObject player in players)
            // {
            //     if (player.GetComponent<Player>().localPlayer)
            //     {
            //         //quitButton = player.Find("QuitButton");
            //     }
            // }
            // quitButton.onClick.AddListener(QuitButtonClicked);
        }

        void Update()
        {
            //if (!showGUI)
            //    return;

            //GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
            
            manager = GetComponent<NetworkManager>();
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                if (!NetworkClient.active)
                {
                    // LAN Host
                    if (Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        // if (GUILayout.Button("LAN Host"))
                        // {
                        //     manager.StartHost();
                        // }
                        if (hostButtonClicked)
                        {
                            manager.StartHost();
                            networkMenu.SetActive(false);
                            networkMenuActive = false;
                        }
                    }

                    // LAN Client + IP
                    //GUILayout.BeginHorizontal();
                    if (clientButtonClicked)
                    {
                        manager.StartClient();
                        networkMenu.SetActive(false);
                        networkMenuActive = false;
                    }
                    //manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                    //GUILayout.EndHorizontal();
                    manager.networkAddress = "localhost";

                    // LAN Server Only
                    if (Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        // cant be a server in webgl build
                        Debug.Log("WebGL cannot be server");
                    }
                    else
                    {
                        if (serverButtonClicked)
                        {
                            manager.StartServer();
                            networkMenu.SetActive(false);
                            networkMenuActive = false;
                        }
                    }
                }
                else
                {
                    // Connecting
                    //GUILayout.Label("Connecting to " + manager.networkAddress + "..");
                    //if (GUILayout.Button("Cancel Connection Attempt"))
                    //{
                    //    manager.StopClient();
                    //}
                }
            }
            else
            {
                // server / client status message
                //if (NetworkServer.active)
                //{
                //    GUILayout.Label("Server: active. Transport: " + Transport.activeTransport);
                //}
                //if (NetworkClient.isConnected)
                //{
                //    GUILayout.Label("Client: address=" + manager.networkAddress);
                //}
            }

            // client ready
            // if (NetworkClient.isConnected && !ClientScene.ready)
            // {
            //     if (GUILayout.Button("Client Ready"))
            //     {
            //         ClientScene.Ready(NetworkClient.connection);

            //         if (ClientScene.localPlayer == null)
            //         {
            //             ClientScene.AddPlayer();
            //         }
            //     }
            // }

            // stop
            if (NetworkServer.active || NetworkClient.isConnected)
            {
                // Implement this
                if (quitButtonClicked && !networkMenuActive)
                {
                    networkMenu.SetActive(true);
                    networkMenuActive = true;
                    quitButtonClicked = hostButtonClicked = clientButtonClicked = serverButtonClicked = false;
                    manager.StopHost();
                }
            }

            //GUILayout.EndArea();
        }

        void HostButtonClicked()
        {
            hostButtonClicked = true;
        }

        void ClientButtonClicked()
        {
            clientButtonClicked = true;
        }

        void ServerButtonClicked()
        {
            serverButtonClicked = true;
        }

        void QuitButtonClicked()
        {
            quitButtonClicked = true;
        }
    }
}
