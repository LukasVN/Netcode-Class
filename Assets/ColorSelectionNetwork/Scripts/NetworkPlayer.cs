using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(); //Network position for each HelloWorldPlayer
    public NetworkVariable<Material> Material = new NetworkVariable<Material>(); //Network Material for each HelloWorldPlayer

        public override void OnNetworkSpawn()
        {
            if (IsOwner) //Checks for client's gameObject
            {
                SetInitialPosition();
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
            var materialColor = GetRandomColor();
            
            GetComponent<Material>().color = materialColor;
            Material.Value = GetComponent<Material>(); //Asigns network position for client's player gameObject
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        static Color GetRandomColor()
        {
            return new Color( Random.value, Random.value, Random.value, 1.0f );
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }

