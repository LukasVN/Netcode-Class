using Unity.Netcode;
using UnityEngine;

public class ColorNetworkManager : MonoBehaviour
{
    public static NetworkVariable<int> PlayerCount = new NetworkVariable<int>(0);
    void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) {
                NetworkManager.Singleton.StartHost(); 
                PlayerCount.Value++;
                Debug.Log(PlayerCount.Value);
                }
            if (GUILayout.Button("Client")) {
                NetworkManager.Singleton.StartClient();
                PlayerCount.Value++;
                Debug.Log(PlayerCount.Value);
            }
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }


        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Change Color" : "Request Color Change"))
            {
                Debug.Log("Change Color pressed");
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient )
                {
                    foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds){
                        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<NetworkPlayer>().SetRandomColor();
                    }
                }
                else
                {
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<NetworkPlayer>();
                    player.SetRandomColor();

                }
            }

        }

}

