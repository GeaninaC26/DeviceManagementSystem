using Microsoft.Data.SqlClient;

namespace DeviceManagementSystem.Application.Abstractions
{
    public interface IDatabaseProvider
    {
        SqlConnection GetConnection(CancellationToken token);
    }
}