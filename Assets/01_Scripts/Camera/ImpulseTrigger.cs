using UnityEngine;
using Cinemachine;

public class CameraShakeTrigger : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    public void TriggerImpulse()
    {
        impulseSource.GenerateImpulse();
    }
}
