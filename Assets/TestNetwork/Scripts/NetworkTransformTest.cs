using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkTransformTest : NetworkBehaviour
{
    void Update()
    {
        if (IsServer)
        {
            
        }

        void Move(){
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            Vector3 newPosition = transform.position + new Vector3(moveX,0,moveZ);
            
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(Vector3 newPosition){
            SubmitPositionRequestServerRpc();
        }
    }
}
