using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Pratice.Data;
using Pratice.Models;
using Pratice.Services.Interfaces;
using Pratice.ViewModels;

namespace Pratice.Services;

public class RetirementService : IRetirementService
{
    private readonly ApplicationDbContext _context;

    public RetirementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public RetirementProfile? GetByUserId(string userId)
    {
        return _context.RetirementProfiles
            .Include(r => r.StepUps)
            .FirstOrDefault(r => r.UserId == userId);
    }

    public bool HasProfile(string userId)
    {
        return _context.RetirementProfiles.Any(r => r.UserId == userId);
    }

    public void Create(RetirementProfile profile)
    {
        profile.Id = Guid.NewGuid();
        profile.CreatedAt = DateTime.UtcNow;
        profile.UpdatedAt = DateTime.UtcNow;
        _context.RetirementProfiles.Add(profile);
        _context.SaveChanges();
    }

    public void Update(RetirementProfile profile)
    {
        profile.UpdatedAt = DateTime.UtcNow;
        _context.RetirementProfiles.Update(profile);
        _context.SaveChanges();
    }

    public void SaveStepUps(Guid retirementProfileId, List<StepUpEntry> stepUps)
    {
        // Remove existing step-ups
        var existing = _context.InvestmentStepUps
            .Where(s => s.RetirementProfileId == retirementProfileId)
            .ToList();
        _context.InvestmentStepUps.RemoveRange(existing);

        // Add new step-ups
        foreach (var entry in stepUps)
        {
            _context.InvestmentStepUps.Add(new InvestmentStepUp
            {
                Id = Guid.NewGuid(),
                RetirementProfileId = retirementProfileId,
                AfterMonth = entry.AfterMonth,
                Amount = entry.Amount
            });
        }

        _context.SaveChanges();
    }

    public RetirementViewModel CalculateRetirementPlan(
        int currentAge, int retirementAge, decimal monthlyInvestment,
        decimal monthlyExpenses, decimal annualRate, decimal goalAmount,
        List<StepUpEntry> stepUps)
    {
        int yearsToRetirement = retirementAge - currentAge;
        if (yearsToRetirement <= 0) yearsToRetirement = 1;

        int totalMonths = yearsToRetirement * 12;
        double monthlyRate = (double)(annualRate / 12 / 100);

        // Sort step-ups by AfterMonth
        var sortedStepUps = stepUps
            .OrderBy(s => s.AfterMonth)
            .ToList();

        // Month-by-month simulation
        double corpus = 0;
        decimal totalInvested = 0;
        int? monthsToReachGoal = null;

        var labels = new List<string>();
        var investedData = new List<decimal>();
        var valueData = new List<decimal>();
        var breakdownTable = new List<YearlyBreakdown>();

        int stepUpIndex = 0;
        decimal currentMonthlyAmount = monthlyInvestment;

        for (int month = 1; month <= totalMonths; month++)
        {
            // Check if we need to step up this month
            while (stepUpIndex < sortedStepUps.Count &&
                   month > sortedStepUps[stepUpIndex].AfterMonth)
            {
                currentMonthlyAmount = sortedStepUps[stepUpIndex].Amount;
                stepUpIndex++;
            }

            // Compound: existing corpus grows + new investment
            corpus = corpus * (1 + monthlyRate) + (double)currentMonthlyAmount;
            totalInvested += currentMonthlyAmount;

            // Check if goal reached
            if (monthsToReachGoal == null && (decimal)corpus >= goalAmount)
            {
                monthsToReachGoal = month;
            }

            // Year-end snapshot
            if (month % 12 == 0)
            {
                int year = month / 12;
                decimal corpusDecimal = (decimal)Math.Round(corpus, 2);

                labels.Add($"Year {year}");
                investedData.Add(Math.Round(totalInvested, 2));
                valueData.Add(corpusDecimal);

                breakdownTable.Add(new YearlyBreakdown
                {
                    Year = year,
                    Age = currentAge + year,
                    AmountInvested = Math.Round(totalInvested, 2),
                    ExpectedValue = corpusDecimal,
                    ReturnsEarned = Math.Round(corpusDecimal - totalInvested, 2),
                    MonthlyInvestmentAmount = currentMonthlyAmount
                });
            }
        }

        decimal expectedCorpus = (decimal)Math.Round(corpus, 2);
        bool goalAchievable = expectedCorpus >= goalAmount;

        // Recommended monthly investment to reach goal (flat, no step-ups)
        decimal recommendedInvestment = CalculateRecommendedInvestment(
            goalAmount, yearsToRetirement, annualRate);

        var chartData = new
        {
            labels,
            invested = investedData,
            value = valueData,
            goalAmount
        };

        return new RetirementViewModel
        {
            YearsToRetirement = yearsToRetirement,
            TotalInvested = totalInvested,
            ExpectedCorpus = expectedCorpus,
            EstimatedReturns = expectedCorpus - totalInvested,
            RecommendedMonthlyInvestment = recommendedInvestment,
            RetirementCorpusNeeded = goalAmount,
            MonthlySavings = 0,
            MonthsToReachGoal = monthsToReachGoal,
            GoalAchievable = goalAchievable,
            YearlyBreakdownJson = JsonSerializer.Serialize(chartData),
            YearlyBreakdownTable = breakdownTable,
            HasProfile = true
        };
    }

    public decimal CalculateRecommendedInvestment(
        decimal goalAmount, int yearsToRetirement, decimal annualRate)
    {
        // Reverse SIP: P = FV * i / (((1 + i)^n - 1) * (1 + i))
        double i = (double)(annualRate / 12 / 100);
        double n = yearsToRetirement * 12;
        double fv = (double)goalAmount;

        if (n <= 0 || i <= 0) return 0;

        double monthlyRequired = fv * i / ((Math.Pow(1 + i, n) - 1) * (1 + i));

        return (decimal)Math.Round(Math.Max(monthlyRequired, 0), 2);
    }
}
