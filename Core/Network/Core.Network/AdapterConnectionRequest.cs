using System.Net;
using Core.Network.Interface;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Core.Network;

public sealed class AdapterConnectionRequest(ConnectionRequest connectionRequest) : IAdapterConnectionRequest
{
    public IPEndPoint RemoteEndPoint => connectionRequest.RemoteEndPoint;

    public IAdapterNetPeer AcceptIfKey(string key)
    {
        return new AdapterNetPeer(connectionRequest.AcceptIfKey(key));
    }

    public IAdapterNetPeer Accept()
    {
        return new AdapterNetPeer(connectionRequest.Accept());
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

    public void RejectForce(IAdapterDataWriter rejectData)
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

    public void Reject(IAdapterDataWriter rejectData)
    {
        connectionRequest.Reject(ValidateAndExtract(rejectData));
    }

    private static NetDataWriter ValidateAndExtract(IAdapterDataWriter rejectData)
    {
        if (rejectData is not AdapterDataWriter customDataWriter)
            throw new InvalidOperationException("Invalid ICustomDataWriter type. Expected CustomDataWriter.");

        return customDataWriter.GetNetDataWriter();
    }
}