using Auth_API.Dal;
using Microsoft.EntityFrameworkCore;

namespace Auth_API.Tests.IntegrationTests
{
    public class DbFixture : IDisposable
    {
        private readonly DataContext _dbContext;
        private bool _disposed;

        public DbFixture()
        {
            string connectionString = "database=auth;keepalive=5;server=127.0.0.1;port=3306;user id=root;password=qwerty;connectiontimeout=5";
            DbContextOptionsBuilder<DataContext>? builder = new();

            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            _dbContext = new DataContext(builder.Options);
            _dbContext.Database.Migrate();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                // remove the temp db from the server once all tests are done
                _dbContext.Database.EnsureDeleted();
            }

            _disposed = true;
        }
    }
}
