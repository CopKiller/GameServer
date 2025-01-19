using System.Net;
using Core.Network.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Network;

public class NetService(IAdapterNetManager netManager) : INetService
{ 
    /// <summary>
    /// USE THIS METHOD IN CLIENTS, SERVERS NOT NEED TO CALL THIS METHOD
    /// SERVER INITIALIZER IN ConnectionManager
    /// </summary>
    public void Start()
    {
        netManager.Start();
    }

    public void Stop()
    {
        netManager.Stop();
    }

    public void PollEvents()
    {
        netManager.PollEvents();
    }

    public void TriggerEvents()
    {
        netManager.TriggerEvents();
    }

    public void Stop(bool sendDisconnectMessages)
    {
        netManager.Stop(sendDisconnectMessages);
    }
}