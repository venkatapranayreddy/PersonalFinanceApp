using Pratice.Models;
using Pratice.ViewModels;

namespace Pratice.Services.Interfaces;

public interface IRetirementService
{
    RetirementProfile? GetByUserId(string userId);
    bool HasProfile(string userId);
    void Create(RetirementProfile profile);
    void Update(RetirementProfile profile);
    void SaveStepUps(Guid retirementProfileId, List<StepUpEntry> stepUps);
    RetirementViewModel CalculateRetirementPlan(
        int currentAge, int retirementAge, decimal monthlyInvestment,
        decimal monthlyExpenses, decimal annualRate, decimal goalAmount,
        List<StepUpEntry> stepUps);
    decimal CalculateRecommendedInvestment(decimal goalAmount, int yearsToRetirement, decimal annualRate);
}
