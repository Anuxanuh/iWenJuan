using CsvHelper;
using CsvHelper.Configuration;
using iWenJuan.Service.DataProcessing.Interface;
using iWenJuan.Shared.Dtos;
using iWenJuan.Shared.Enums;
using System.Dynamic;
using System.Globalization;

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
	public async Task<MemoryStream> ProcessCsvAsync(Stream csvStream, List<CsvOperation> operations)
	{
		// 配置CSV读取器，设置标题匹配时忽略大小写
		var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			PrepareHeaderForMatch = args => args.Header.ToLower()
		};

		// 使用StreamReader读取CSV文件流
		using (var reader = new StreamReader(csvStream))
		// 使用CsvReader解析CSV内容
		using (var csv = new CsvReader(reader, configuration))
		{
			// 获取CSV记录并转换为动态对象列表
			var records = csv.GetRecords<dynamic>().ToList();

			// 对每个操作进行处理
			foreach (var operation in operations)
			{
				records = ApplyOperation(records, operation);
			}

			// 创建内存流用于存储处理后的CSV内容
			var memoryStream = new MemoryStream();
			using (var writer = new StreamWriter(memoryStream))
			using (var csvWriter = new CsvWriter(writer, configuration))
			{
				// 异步写入处理后的记录到内存流
				await csvWriter.WriteRecordsAsync(records);
				await writer.FlushAsync();
				memoryStream.Position = 0; // 重置内存流位置
				return memoryStream; // 返回内存流
			}
		}
	}

	/// <summary>
	/// 根据操作类型应用相应的处理方法
	/// </summary>
	/// <param name="records">CSV记录列表</param>
	/// <param name="operation">操作</param>
	/// <returns>处理后的CSV记录列表</returns>
	private List<dynamic> ApplyOperation(List<dynamic> records, CsvOperation operation)
	{
		switch (operation.OperationType)
		{
			case CsvOperationType.Select:
				return records.Select(r => SelectColumns(r, operation.Column)).ToList();
			case CsvOperationType.Filter:
				return records.Where(r => FilterRecords(r, operation.Column, operation.Condition, operation.Value)).ToList();
			case CsvOperationType.GroupBy:
				return GroupByColumn(records, operation.Column);
			case CsvOperationType.OrderBy:
				return OrderByColumn(records, operation.Column);
			case CsvOperationType.Count:
				return AggregateColumn(records, operation.Column, "count");
			case CsvOperationType.Sum:
				return AggregateColumn(records, operation.Column, "sum");
			case CsvOperationType.Average:
				return AggregateColumn(records, operation.Column, "average");
			case CsvOperationType.Min:
				return AggregateColumn(records, operation.Column, "min");
			case CsvOperationType.Max:
				return AggregateColumn(records, operation.Column, "max");
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
	private dynamic SelectColumns(dynamic record, string columns)
	{
		var selectedColumns = columns.Split(',');
		var newRecord = new ExpandoObject() as IDictionary<string, Object>;

		foreach (var column in selectedColumns)
		{
			if (((IDictionary<string, object>) record).ContainsKey(column))
			{
				newRecord[column] = ((IDictionary<string, object>) record)[column];
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
	private bool FilterRecords(dynamic record, string column, string condition, string value)
	{
		if (!((IDictionary<string, object>) record).ContainsKey(column))
		{
			return false;
		}

		var recordValue = ((IDictionary<string, object>) record)[column].ToString();

		switch (condition)
		{
			case "==":
				return recordValue == value;
			case "!=":
				return recordValue != value;
			case ">":
				return double.Parse(recordValue) > double.Parse(value);
			case "<":
				return double.Parse(recordValue) < double.Parse(value);
			case ">=":
				return double.Parse(recordValue) >= double.Parse(value);
			case "<=":
				return double.Parse(recordValue) <= double.Parse(value);
			default:
				return false;
		}
	}

	/// <summary>
	/// 根据指定列进行分组
	/// </summary>
	/// <param name="records">CSV记录列表</param>
	/// <param name="column">列名</param>
	/// <returns>分组后的记录列表</returns>
	private List<dynamic> GroupByColumn(List<dynamic> records, string column)
	{
		return records.GroupBy(r => ((IDictionary<string, object>) r)[column])
					  .Select(g => new { Key = g.Key, Count = g.Count() })
					  .ToList<dynamic>();
	}

	/// <summary>
	/// 根据指定列进行排序
	/// </summary>
	/// <param name="records">CSV记录列表</param>
	/// <param name="column">列名</param>
	/// <returns>排序后的记录列表</returns>
	private List<dynamic> OrderByColumn(List<dynamic> records, string column)
	{
		return records.OrderBy(r => ((IDictionary<string, object>) r)[column]).ToList();
	}

	/// <summary>
	/// 对指定列进行聚合操作
	/// </summary>
	/// <param name="records">CSV记录列表</param>
	/// <param name="column">列名</param>
	/// <param name="operation">聚合操作类型</param>
	/// <returns>聚合结果</returns>
	private List<dynamic> AggregateColumn(List<dynamic> records, string column, string operation)
	{
		var values = records.Select(r => double.Parse(((IDictionary<string, object>) r)[column].ToString())).ToList();

		switch (operation)
		{
			case "count":
				return new List<dynamic> { new { Count = values.Count } };
			case "sum":
				return new List<dynamic> { new { Sum = values.Sum() } };
			case "average":
				return new List<dynamic> { new { Average = values.Average() } };
			case "min":
				return new List<dynamic> { new { Min = values.Min() } };
			case "max":
				return new List<dynamic> { new { Max = values.Max() } };
			default:
				return records;
		}
	}
}
