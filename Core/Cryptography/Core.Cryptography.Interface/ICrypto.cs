namespace Core.Cryptography.Interface;

public interface ICrypto
{
    string HashString(string value);
    
    bool CompareHash(string value, string hash);
}