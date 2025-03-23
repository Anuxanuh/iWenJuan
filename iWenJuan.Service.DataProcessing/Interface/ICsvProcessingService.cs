
using iWenJuan.Shared.Dtos;

namespace iWenJuan.Service.DataProcessing.Interface;

public interface ICsvProcessingService
{
	Task<byte[]> ProcessCsvAsync(Stream csvStream, List<CsvOperation> operations);
}
