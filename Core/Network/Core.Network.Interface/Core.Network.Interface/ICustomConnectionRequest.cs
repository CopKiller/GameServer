using System.Net;

namespace Core.Network.Interface;

public interface ICustomConnectionRequest
{
    // TODO: Implement this property if needed...
    //public NetDataReader Data => InternalPacket.Data;
    IPEndPoint RemoteEndPoint { get; }
    ICustomNetPeer AcceptIfKey(string key);
    ICustomNetPeer Accept();
    void Reject(byte[] rejectData, int start, int length, bool force);
    void Reject(byte[] rejectData, int start, int length);
    void RejectForce(byte[] rejectData, int start, int length);
    void RejectForce();
    void RejectForce(byte[] rejectData);
    void RejectForce(ICustomDataWriter rejectData);
    void Reject();
    void Reject(byte[] rejectData);
    void Reject(ICustomDataWriter rejectData);
}