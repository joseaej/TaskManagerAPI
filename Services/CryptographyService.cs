using System;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace TaskFlowAPI.Services
{
    public class CryptographyService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;

        /// <summary>
        /// Hashea una contraseña usando Argon2id, devolviendo salt+hash en Base64.
        /// </summary>
        public static string EncryptPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 1024 * 1024 // 1 GB RAM usage (~1 MiB)
            };

            byte[] hash = argon2.GetBytes(HashSize);

            byte[] saltAndHash = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, saltAndHash, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, saltAndHash, SaltSize, HashSize);

            return Convert.ToBase64String(saltAndHash);
        }

        /// <summary>
        /// Verifica si la contraseña coincide con el hash Argon2id almacenado (formato Base64 salt+hash).
        /// </summary>
        public static bool CheckPassword(string password, string hashWithSaltBase64)
        {
            try
            {
                byte[] saltAndHash = Convert.FromBase64String(hashWithSaltBase64);

                if (saltAndHash.Length != SaltSize + HashSize)
                    return false; // formato incorrecto

                byte[] salt = new byte[SaltSize];
                byte[] storedHash = new byte[HashSize];

                Buffer.BlockCopy(saltAndHash, 0, salt, 0, SaltSize);
                Buffer.BlockCopy(saltAndHash, SaltSize, storedHash, 0, HashSize);

                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = 8,
                    Iterations = 4,
                    MemorySize = 1024 * 1024
                };

                byte[] computedHash = argon2.GetBytes(HashSize);

                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
            }
            catch
            {
                // Base64 inválido u otro fallo
                return false;
            }
        }
    }
}
