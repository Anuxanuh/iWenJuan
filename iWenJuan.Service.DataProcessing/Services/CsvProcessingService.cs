using CsvHelper;
using CsvHelper.Configuration;
using iWenJuan.Service.DataProcessing.Interface;
using iWenJuan.Service.DataProcessing.Utils;
using iWenJuan.Shared.Dtos;
using iWenJuan.Shared.Enums;
using iWenJuan.Shared.Extension;
using System.Globalization;
using System.Text;

namespace iWenJuan.Service.DataProcessing.Services;

/// <summary>
/// CSV处理服务类，实现ICsvProcessingService接口
/// </summary>
public class CsvProcessingService : ICsvProcessingService
{
	/// <summary>
	/// 异步处理CSV文件的方法，接受一个CSV文件流和一组操作
	/// </summary>
	/// <param name="csvStream">CSV文件流</param>
	/// <param name="operations">操作列表</param>
	/// <returns>处理后的CSV内容的内存流</returns>
	public async Task<byte[]> ProcessCsvAsync(Stream csvStream, List<CsvOperation> operations)
	{
		// 文件数据
		List<string> headers = [];
		List<Dictionary<string, string>> fileData = [];

		// 解析CSV文件
		using var reader = new StreamReader(csvStream, Encoding.UTF8);
		using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);

		// 读取表头
		if (csv.Read() && csv.ReadHeader())
		{
			headers = [.. csv.HeaderRecord!];
		}
		// 读取数据
		while (csv.Read())
		{
			var record = csv.GetRecord<dynamic>() as IDictionary<string, object>;
			var dict = new Dictionary<string, string>();
			foreach (var header in headers)
			{
				dict[header] = record[header]?.ToString() ?? string.Empty;
			}
			fileData.Add(dict);
		}

		// 对每个操作进行处理
		foreach (var operation in operations)
		{
			fileData = ApplyOperation(fileData, operation);
		}

		// 创建内存流用于存储处理后的CSV内容
		var memoryStream = new MemoryStream();
		using var writer = new StreamWriter(memoryStream);
		using var csvWriter = new CsvWriter(writer, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture));
		// 异步写入处理后的记录到内存流
		await csvWriter.WriteRecordsAsync(fileData);
		await writer.FlushAsync();
		return memoryStream.ToArray(); // 返回
	}

	/// <summary>
	/// 根据操作类型应用相应的处理方法
	/// </summary>
	/// <param name="records">CSV记录列表</param>
	/// <param name="operation">操作</param>
	/// <returns>处理后的CSV记录列表</returns>
	private List<Dictionary<string, string>> ApplyOperation(List<Dictionary<string, string>> records, CsvOperation operation)
	{
		switch (operation.OperationType)
		{
			case CsvOperationType.Select:
				return [.. records.Select(r => SelectColumns(r, operation.Column!))];
			case CsvOperationType.Filter:
				return [.. records.Where(r => FilterRecords(r, operation.Column!, operation.Condition!.Value, operation.Value!))];
			case CsvOperationType.OrderBy:
				return OrderByColumn(records, operation.Column!);
			case CsvOperationType.GroupBy:
				return GroupByColumn(records, operation.Column!, operation.AggregateOperations!);
			case CsvOperationType.Aggregate:
				return AggregateColumn(records, operation.Column!, operation.AggregateOperations!);
			default:
				return records;
		}
	}

	/// <summary>
	/// 选择指定列
	/// </summary>
	/// <param name="record">CSV记录</param>
	/// <param name="columns">列名，逗号分隔</param>
	/// <returns>包含指定列的新记录</returns>
	private Dictionary<string, string> SelectColumns(Dictionary<string, string> record, string columns)
	{
		var selectedColumns = columns.Split(',');

		var newRecord = new Dictionary<string, string>();

		foreach (var column in selectedColumns)
		{
			if (record.ContainsKey(column))
			{
				newRecord[column] = record[column];
			}
		}

		return newRecord;
	}

	/// <summary>
	/// 根据条件过滤记录
	/// </summary>
	/// <param name="record">CSV记录</param>
	/// <param name="column">列名</param>
	/// <param name="condition">条件</param>
	/// <param name="value">值</param>
	/// <returns>是否满足条件</returns>
	private bool FilterRecords(Dictionary<string, string> record, string column, Operator condition, string value)
	{
		if (!record.ContainsKey(column))
		{
			return false;
		}

		var recordValue = record[column];

		if (recordValue == null)
		{
			return false;
		}

		return condition switch
		{
			Operator.Equals => recordValue == value,
			Operator.NotEquals => recordValue != value,
			Operator.Contains => recordValue.Contains(value),
			Operator.GreaterThan => StringOrNumericComparer.Compare(recordValue, value) > 0,
			Operator.LessThan => StringOrNumericComparer.Compare(recordValue, value) < 0,
			Operator.GreaterThanOrEquals => StringOrNumericComparer.Compare(recordValue, value) >= 0,
			Operator.LessThanOrEquals => StringOrNumericComparer.Compare(recordValue, value) <= 0,
			_ => false,
		};
	}

	/// <summary>
	/// 根据指定列进行排序
	/// </summary>
	/// <param name="records">CSV记录列表</param>
	/// <param name="column">列名</param>
	/// <returns>排序后的记录列表</returns>
	private List<Dictionary<string, string>> OrderByColumn(List<Dictionary<string, string>> records, string column)
	{
		return records.OrderBy(r => ((IDictionary<string, object>) r)[column]).ToList();
	}

	private List<Dictionary<string, string>> GroupByColumn(List<Dictionary<string, string>> records, string column, Dictionary<AggregateOperationType, string> aggregateOperations)
	{
		List<Dictionary<string, string>> newResult = [];

		var tmp = records.GroupBy(r => r[column]);

		foreach (var group in tmp)
		{
			var newRecord = new Dictionary<string, string>
			{
				[column] = group.Key
			};
			foreach (var (aggregateOperation, c) in aggregateOperations)
			{
				newRecord[$"{c}_{aggregateOperation.GetDisplayName()}"] = SubAggregateColumn(group, c, aggregateOperation);
			}
			newResult.Add(newRecord);
		}

		return newResult;
	}

	private List<Dictionary<string, string>> AggregateColumn(IEnumerable<IDictionary<string, string>> records, string column, Dictionary<AggregateOperationType, string> aggregateOperations)
	{
		Dictionary<string, string> newResult = [];

		foreach (var (aggregateOperation, c) in aggregateOperations)
		{
			newResult[$"{c}_{aggregateOperation.GetDisplayName()}"] = SubAggregateColumn(records, c, aggregateOperation);
		}

		return [newResult];
	}

	private string SubAggregateColumn(IEnumerable<IDictionary<string, string>> records, string? column, AggregateOperationType aggregateOperation)
	{
		switch (aggregateOperation)
		{
			case AggregateOperationType.Count:
				if (string.IsNullOrEmpty(column))
				{
					return records.Count().ToString();
				}
				else
				{
					return records.Where(r => string.IsNullOrWhiteSpace(r[column])).Count().ToString();
				}
			case AggregateOperationType.Sum:
				return records.Where(r => r.CanConvertToDouble(column!)).Sum(r => double.Parse(r[column!])).ToString();
			case AggregateOperationType.Average:
				return records.Where(r => r.CanConvertToDouble(column!)).Average(r => double.Parse(r[column!])).ToString();
			case AggregateOperationType.Min:
				return records.Where(r => r.CanConvertToDouble(column!)).Min(r => double.Parse(r[column!])).ToString();
			case AggregateOperationType.Max:
				return records.Where(r => r.CanConvertToDouble(column!)).Max(r => double.Parse(r[column!])).ToString();
			default:
				break;
		}
		return "";
	}
}
