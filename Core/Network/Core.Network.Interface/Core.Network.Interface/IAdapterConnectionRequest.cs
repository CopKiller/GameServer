using System.Net;

namespace Core.Network.Interface;

public interface IAdapterConnectionRequest
{
    // TODO: Implement this property if needed...
    //public NetDataReader Data => InternalPacket.Data;
    IPEndPoint RemoteEndPoint { get; }
    IAdapterNetPeer AcceptIfKey(string key);
    IAdapterNetPeer Accept();
    void Reject(byte[] rejectData, int start, int length, bool force);
    void Reject(byte[] rejectData, int start, int length);
    void RejectForce(byte[] rejectData, int start, int length);
    void RejectForce();
    void RejectForce(byte[] rejectData);
    void RejectForce(IAdapterDataWriter rejectData);
    void Reject();
    void Reject(byte[] rejectData);
    void Reject(IAdapterDataWriter rejectData);
}