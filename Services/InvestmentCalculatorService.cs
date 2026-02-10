using System.Text.Json;
using Pratice.Services.Interfaces;
using Pratice.ViewModels;

namespace Pratice.Services;

public class InvestmentCalculatorService : IInvestmentCalculatorService
{
    public InvestmentCalculatorViewModel CalculateSip(decimal monthlyAmount, int years, decimal annualRate)
    {
        var monthlyRate = annualRate / 12 / 100;
        var totalMonths = years * 12;
        var totalInvested = monthlyAmount * totalMonths;

        // SIP Future Value: P × ((1 + i)^n - 1) / i × (1 + i)
        double i = (double)monthlyRate;
        double p = (double)monthlyAmount;
        double n = totalMonths;

        double futureValue = p * ((Math.Pow(1 + i, n) - 1) / i) * (1 + i);
        var totalValue = (decimal)Math.Round(futureValue, 2);

        // Year-by-year breakdown
        var labels = new List<string>();
        var investedData = new List<decimal>();
        var valueData = new List<decimal>();

        for (int year = 1; year <= years; year++)
        {
            int months = year * 12;
            decimal invested = monthlyAmount * months;
            double fv = p * ((Math.Pow(1 + i, months) - 1) / i) * (1 + i);

            labels.Add($"Year {year}");
            investedData.Add(Math.Round(invested, 2));
            valueData.Add((decimal)Math.Round(fv, 2));
        }

        var chartData = new
        {
            labels,
            invested = investedData,
            value = valueData
        };

        return new InvestmentCalculatorViewModel
        {
            CalculatorType = "SIP",
            MonthlyInvestment = monthlyAmount,
            TimePeriodYears = years,
            ExpectedReturnRate = annualRate,
            TotalInvested = totalInvested,
            TotalValue = totalValue,
            EstimatedReturns = totalValue - totalInvested,
            HasResult = true,
            YearlyBreakdownJson = JsonSerializer.Serialize(chartData)
        };
    }

    public InvestmentCalculatorViewModel CalculateLumpSum(decimal principal, int years, decimal annualRate)
    {
        // Lump Sum Future Value: P × (1 + r/100)^n
        double p = (double)principal;
        double r = (double)(annualRate / 100);

        double futureValue = p * Math.Pow(1 + r, years);
        var totalValue = (decimal)Math.Round(futureValue, 2);

        // Year-by-year breakdown
        var labels = new List<string>();
        var investedData = new List<decimal>();
        var valueData = new List<decimal>();

        for (int year = 1; year <= years; year++)
        {
            double fv = p * Math.Pow(1 + r, year);

            labels.Add($"Year {year}");
            investedData.Add(principal);
            valueData.Add((decimal)Math.Round(fv, 2));
        }

        var chartData = new
        {
            labels,
            invested = investedData,
            value = valueData
        };

        return new InvestmentCalculatorViewModel
        {
            CalculatorType = "LumpSum",
            LumpSumAmount = principal,
            TimePeriodYears = years,
            ExpectedReturnRate = annualRate,
            TotalInvested = principal,
            TotalValue = totalValue,
            EstimatedReturns = totalValue - principal,
            HasResult = true,
            YearlyBreakdownJson = JsonSerializer.Serialize(chartData)
        };
    }
}
