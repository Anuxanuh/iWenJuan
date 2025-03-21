using iWenJuan.Service.Survey.Models;
using Microsoft.EntityFrameworkCore;
using SurveyModel = iWenJuan.Service.Survey.Models.Survey;

namespace iWenJuan.Service.Survey.Data;

public class SurveyDbContext : DbContext
{
	public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options)
	{
	}

	public DbSet<SurveyModel> Surveys { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<Option> Options { get; set; }
	public DbSet<Answer> Answers { get; set; }
	public DbSet<Condition> Conditions { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<SurveyModel>()
			.HasMany(s => s.Questions)
			.WithOne(q => q.Survey)
			.HasForeignKey(q => q.SurveyId);

		modelBuilder.Entity<Question>()
			.HasMany(q => q.Options)
			.WithOne(o => o.Question)
			.HasForeignKey(o => o.QuestionId);

		modelBuilder.Entity<Answer>()
			.HasOne(a => a.Question)
			.WithMany(q => q.Answers)
			.HasForeignKey(a => a.QuestionId);

		modelBuilder.Entity<Condition>()
				.HasOne(c => c.Question)
				.WithMany(q => q.Conditions)
				.HasForeignKey(c => c.QuestionId);

		modelBuilder.Entity<Condition>()
			.HasOne(c => c.NextQuestion)
			.WithMany()
			.HasForeignKey(c => c.NextQuestionId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}