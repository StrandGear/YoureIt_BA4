using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLayering : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            LayerManager.Instance.ClearLayerList();
            GameStates.Instance.SetGameState(GameState.Playmode);
            /*            if (Singleton.GetInstance<PlayerScan>().IsScanning)
                        {
                            LayerManager.Instance.ClearLayerList();

                            Singleton.GetInstance<PlayerScan>().StopScanning();
                        }*/

        }
    }
}
