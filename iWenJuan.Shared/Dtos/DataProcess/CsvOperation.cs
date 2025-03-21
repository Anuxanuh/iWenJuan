using iWenJuan.Shared.Enums;

namespace iWenJuan.Shared.Dtos;

public class CsvOperation
{
	public CsvOperationType OperationType { get; set; }
	public string Column { get; set; }
	public string Condition { get; set; }
	public string Value { get; set; }
}
