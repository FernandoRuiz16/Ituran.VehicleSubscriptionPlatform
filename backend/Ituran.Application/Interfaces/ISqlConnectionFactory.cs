using System.Data;

namespace Ituran.Application.Interfaces;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}