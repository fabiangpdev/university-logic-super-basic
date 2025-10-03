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

        public async Task<int> RegisterAsync(string name, string email, string password, string role = "user")
        {
            var hash = PasswordHasher.ComputeSha256(password);
            const string sql = @"INSERT INTO users(name, email, password_hash, role)
                                 VALUES (@name, @email, @hash, @role)
                                 RETURNING id;";
            await using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("hash", hash);
            cmd.Parameters.AddWithValue("role", role);
            var idObj = await cmd.ExecuteScalarAsync();
            return idObj is int id ? id : 0;
        }

        public async Task<(bool ok, int userId, string name, string role)> ValidateLoginAsync(string email, string password)
        {
            const string sql = "SELECT id, name, role, password_hash FROM users WHERE email = @email LIMIT 1";
            await using var conn = new NpgsqlConnection(_connString);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("email", email);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return (false, 0, string.Empty, "user");
            var userId = reader.GetInt32(0);
            var name = reader.GetString(1);
            var role = reader.GetString(2);
            var dbHash = reader.GetString(3);
            var ok = PasswordHasher.Verify(password, dbHash);
            return ok ? (true, userId, name, role) : (false, 0, string.Empty, "user");
        }
    }
}


