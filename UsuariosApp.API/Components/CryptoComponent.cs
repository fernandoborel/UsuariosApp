using System.Security.Cryptography;
using System.Text;

namespace UsuariosApp.API.Components;

public class CryptoComponent
{
    public static string Sha256Encrypt(string value)
    {
        if(string.IsNullOrEmpty(value))
            return string.Empty;

        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = sha256.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}