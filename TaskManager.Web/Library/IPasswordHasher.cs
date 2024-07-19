using System.Security.Cryptography;
using System.Text;

namespace TaskManager.Web.Library
{
    public interface IPasswordHasher
    {
        string HashPassword(string password, out byte[] salt);
        bool VerifyHashedPassword(string password, string hashedPassword, byte[] salt);
    }
}