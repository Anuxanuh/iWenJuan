using iWenJuan.Shared.Enums;

namespace iWenJuan.Shared.Dtos;

public class CsvOperation
{
	public CsvOperationType OperationType { get; set; }

	/// <summary>
	/// 操作的列名
	/// 仅当 OperationType 为 Select 时：为数组, 用 `,` 分隔列名
	/// </summary>
	public string Column { get; set; }

	/// <summary>
	/// 仅用作 Filter 操作
	/// </summary>
	public Operator? Condition { get; set; }
	/// <summary>
	/// 仅用作 Filter 操作
	/// </summary>
	public string? Value { get; set; }

	/// <summary>
	/// 聚合操作
	/// 仅当 OperationType 为 GroupBy, Aggregate 时有效
	/// Key为操作, Value为列名
	/// 仅当 Key 为 Count 时 Value 可以为 null
	/// </summary>
	public Dictionary<AggregateOperationType, string>? AggregateOperations { get; set; }
}
