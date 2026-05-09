using UnityEngine;
using Unity.Cinemachine;
public class ImpulseSource : MonoBehaviour
{
    public static ImpulseSource Instance { get; private set; }
    private void Start()
    {
        Instance = this;
    }
    public void Invoke()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }
}