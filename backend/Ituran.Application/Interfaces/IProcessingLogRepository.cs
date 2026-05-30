using Ituran.Domain.Entities;

namespace Ituran.Application.Interfaces;

public interface IProcessingLogRepository
{
    Task CreateAsync(ProcessingLog log);
}