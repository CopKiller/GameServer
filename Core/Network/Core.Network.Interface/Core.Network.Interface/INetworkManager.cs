using System.Collections;
using System.Data;
using System.Net;
using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface INetworkManager
{
    bool StartListener(int port = 0);
    void StartClient();
    ICustomNetPeer ConnectToServer(string address, int port, string key);
    ICustomNetPeer ConnectToServer(IPEndPoint target, string key);
    void Stop();
    void PollEvents();
    void TriggerEvents();
    void Stop(bool sendDisconnectMessages);
    int GetPeersCount(CustomConnectionState peerState);
    IEnumerable<ICustomNetPeer> GetPeers(CustomConnectionState state);
    ICustomNetPeer GetFirstPeer();
    void DisconnectAll();
    void DisconnectAll(byte[] data, int start, int count);
    void DisconnectPeerForce(ICustomNetPeer peer);
    void DisconnectPeer(ICustomNetPeer peer);
    void DisconnectPeer(ICustomNetPeer peer, byte[] data);
    void DisconnectPeer(ICustomNetPeer peer, ICustomDataWriter writer);
    void DisconnectPeer(ICustomNetPeer peer, byte[] data, int start, int count);
    void CreateNtpRequest(IPEndPoint endPoint);
    void CreateNtpRequest(string ntpServerAddress, int port);
    void CreateNtpRequest(string ntpServerAddress);
}