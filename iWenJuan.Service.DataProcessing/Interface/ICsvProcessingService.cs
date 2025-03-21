
using iWenJuan.Shared.Dtos;

namespace iWenJuan.Service.DataProcessing.Interface;

public interface ICsvProcessingService
{
	Task<MemoryStream> ProcessCsvAsync(Stream csvStream, List<CsvOperation> operations);
}
