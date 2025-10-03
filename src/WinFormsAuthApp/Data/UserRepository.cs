using System.Threading.Tasks;
using Npgsql;
using WinFormsAuthApp.Security;
using WinFormsAuthApp.Config;

namespace WinFormsAuthApp.Data
{
    public sealed class UserRepository
    {
        private readonly string _connString;

        public UserRepository()
        {
            _connString = Config.Config.GetConnectionString();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            const string sql = "SELECT 1 FROM users WHERE email = @email LIMIT 1";
            await using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("email", email);
            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }

        public async Task<int> RegisterAsync(string name, string email, string password)
        {
            var hash = PasswordHasher.ComputeSha256(password);
            const string sql = @"INSERT INTO users(name, email, password_hash)
                                 VALUES (@name, @email, @hash)
                                 RETURNING id;";
            await using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("hash", hash);
            var idObj = await cmd.ExecuteScalarAsync();
            return idObj is int id ? id : 0;
        }

        public async Task<bool> ValidateLoginAsync(string email, string password)
        {
            const string sql = "SELECT password_hash FROM users WHERE email = @email LIMIT 1";
            await using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("email", email);
            var result = await cmd.ExecuteScalarAsync();
            if (result is not string dbHash) return false;
            return PasswordHasher.Verify(password, dbHash);
        }
    }
}


