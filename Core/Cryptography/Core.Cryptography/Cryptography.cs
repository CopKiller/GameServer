using Core.Cryptography.Interface;

namespace Core.Cryptography;

using BCrypt.Net;

public class Cryptography : ICrypto
{
    public string HashString(string value)
    {
        return BCrypt.HashPassword(value);
    }

    public bool CompareHash(string value, string hash)
    {
        return BCrypt.Verify(value, hash);
    }
}
