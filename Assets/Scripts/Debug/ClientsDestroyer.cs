using erulathra;
using UnityEngine;

public class ClientsDestroyer : MonoBehaviour
{
    public void DisableClients()
    {
        ClientSubsystem clientSubsystem = SceneSubsystemManager.GetSubsystem<ClientSubsystem>();
        clientSubsystem.enabled = false;
    }
}
