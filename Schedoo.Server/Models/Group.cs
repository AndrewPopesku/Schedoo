using System.Security.Cryptography;
using System.Text;

namespace Schedoo.Server.Models;

public class Group
{
    public string Id { get; set; }
    public string Name { get; set; }

    public Group(string name)
    {
        Name = name;
        Id = GenerateId(name);
    }
    
    private string GenerateId(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}