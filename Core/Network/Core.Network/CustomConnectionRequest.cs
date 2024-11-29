using System.Net;
using Core.Network.Interface;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Core.Network;

public sealed class CustomConnectionRequest(ConnectionRequest connectionRequest) : ICustomConnectionRequest
{
    public IPEndPoint RemoteEndPoint => connectionRequest.RemoteEndPoint;
    
    public ICustomNetPeer AcceptIfKey(string key)
    {
        return new CustomNetPeer(connectionRequest.AcceptIfKey(key));
    }

    public ICustomNetPeer Accept()
    {
        return new CustomNetPeer(connectionRequest.Accept());
    }

    public void Reject(byte[] rejectData, int start, int length, bool force)
    {
        connectionRequest.Reject(rejectData, start, length, force);
    }

    public void Reject(byte[] rejectData, int start, int length)
    {
        connectionRequest.Reject(rejectData, start, length);
    }

    public void RejectForce(byte[] rejectData, int start, int length)
    {
        connectionRequest.RejectForce(rejectData, start, length);
    }

    public void RejectForce()
    {
        connectionRequest.RejectForce();
    }

    public void RejectForce(params byte[] rejectData)
    {
        connectionRequest.RejectForce(rejectData);
    }

    public void RejectForce(ICustomDataWriter rejectData)
    {
        connectionRequest.RejectForce(ValidateAndExtract(rejectData));
    }

    public void Reject()
    {
        connectionRequest.Reject();
    }

    public void Reject(byte[] rejectData)
    {
        connectionRequest.Reject(rejectData);
    }

    public void Reject(ICustomDataWriter rejectData)
    {
        connectionRequest.Reject(ValidateAndExtract(rejectData));
    }
    
    private static NetDataWriter ValidateAndExtract(ICustomDataWriter rejectData)
    {
        if (rejectData is not CustomDataWriter customDataWriter)
            throw new InvalidOperationException("Invalid ICustomDataWriter type. Expected CustomDataWriter.");
    
        return customDataWriter.GetNetDataWriter();
    }
}