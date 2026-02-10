using Pratice.ViewModels;

namespace Pratice.Services.Interfaces;

public interface IInvestmentCalculatorService
{
    InvestmentCalculatorViewModel CalculateSip(decimal monthlyAmount, int years, decimal annualRate);
    InvestmentCalculatorViewModel CalculateLumpSum(decimal principal, int years, decimal annualRate);
}
