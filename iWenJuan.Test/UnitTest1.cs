using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using static Abstracta.JmeterDsl.JmeterDsl;

namespace iWenJuan.Test;

class AnswerDto
{
	public int QuestionId { get; set; }
	public string AnswerText { get; set; }
}

public class Tests
{
	[SetUp]
	public void Setup()
	{
	}

	/// <summary>
	/// 性能测试：模拟 500 个用户并发访问主界面
	/// </summary>
	[Test]
	public void PerformanceTest()
	{
		List<AnswerDto> postRaw = [
			new AnswerDto{
				QuestionId = 128,
				AnswerText = "A"
			},
			new AnswerDto{
				QuestionId = 129,
				AnswerText = "测试答案2"
			}
		];

		var stats = TestPlan(
			ThreadGroup(500, 10, // 500 个线程（用户），每个用户执行 10 次请求
				HttpSampler("https://localhost:7270/api/Answers")
				.Post(JsonSerializer.Serialize(postRaw),
					  new MediaTypeHeaderValue(MediaTypeNames.Application.Json))
			),
			JtlWriter("results.jtl") // 记录测试数据
		).Run();

		// 断言：99% 的请求时间必须低于 3 秒
		Assert.That(stats.Overall.SampleTimePercentile99, Is.LessThan(TimeSpan.FromSeconds(3)));
	}
}
