using Core.Network.Interface;

namespace Core.Network.SerializationObjects.Response;

public class ResponseDto : IAdapterSerializable
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public void Serialize(IAdapterDataWriter writer)
    {
        writer.Put(Success);
        writer.Put(Message);
    }

    public void Deserialize(IAdapterDataReader reader)
    {
        Success = reader.GetBool();
        Message = reader.GetString();
    }
}