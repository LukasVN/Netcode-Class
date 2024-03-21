using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(); //Network position for each NetworkPlayer
    private NetworkVariable<int> MaterialIndex = new NetworkVariable<int>(); //Network Material Index for each NetworkPlayer
    public Material[] colorMaterials;
    private MeshRenderer meshRenderer;

        public override void OnNetworkSpawn()
        {
            if (IsOwner) //Checks for client's gameObject
            {
                meshRenderer = GetComponent<MeshRenderer>();
                SetInitialPosition();
            }
        }

        public void OnPlayerDisconnected(NetworkPlayer player) {
            ColorNetworkManager.PlayerCount.Value--;
        }

        public void SetInitialPosition()
        {
            SubmitInitialPositionRequestServerRpc();
        }

        public void SetRandomColor(){
            SubmitColorRequestServerRpc();
        }

        [Rpc(SendTo.Server)]
        void SubmitInitialPositionRequestServerRpc(RpcParams rpcParams = default)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition; //Asigns network position for client's player gameObject
        }

        [Rpc(SendTo.Server)]
        void SubmitColorRequestServerRpc(RpcParams rpcParams = default)
        {
            int randomIndex = Random.Range(0,colorMaterials.Length);
            while(randomIndex == MaterialIndex.Value){
                randomIndex = Random.Range(0,colorMaterials.Length);
            }
            MaterialIndex.Value = randomIndex; //Asigns network color index for client's player meshRenderer
            meshRenderer.material = colorMaterials[MaterialIndex.Value];
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;
            GetComponent<MeshRenderer>().material =  colorMaterials[MaterialIndex.Value];
        }
    }

