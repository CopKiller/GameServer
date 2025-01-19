using System.Collections;
using System.Data;
using System.Net;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Event;

namespace Core.Network.Interface;

public interface INetworkManager
{
    public IAdapterNetManager AdapterNetManager { get; }
    public INetworkEventsListener NetworkEventsListener { get; }
}