using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(); //Network position for each NetworkPlayer
    private NetworkVariable<int> MaterialIndex = new NetworkVariable<int>(-1); //Network Material Index for each NetworkPlayer
    private static List<int> MaterialIndexList = new List<int>(); //Material Index List for each NetworkPlayer
    private int randomIndex;
    public Material[] colorMaterials;
    private MeshRenderer meshRenderer;

        public override void OnNetworkSpawn()
        {
            meshRenderer = GetComponent<MeshRenderer>();

            if (IsOwner) //Checks for client's gameObject
            {
                SetInitialPosition();
                SetRandomColor();
            }
            
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
            if(MaterialIndexList.Count == 6){
                return;
            }
            if(MaterialIndexList.Contains(MaterialIndex.Value)){
                MaterialIndexList.Remove(MaterialIndex.Value);
            }

            randomIndex = Random.Range(0,colorMaterials.Length);
            while(randomIndex == MaterialIndex.Value || MaterialIndexList.Contains(randomIndex)){
                randomIndex = Random.Range(0,colorMaterials.Length);

            }
            MaterialIndex.Value = randomIndex; //Asigns network color index for client's player meshRenderer
            MaterialIndexList.Add(randomIndex);
            meshRenderer.material = colorMaterials[MaterialIndex.Value];
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;
            if(MaterialIndex.Value != -1){
                GetComponent<MeshRenderer>().material =  colorMaterials[MaterialIndex.Value];
            }
            
        }
}

