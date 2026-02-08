using Pratice.Models;
using Pratice.Models.Enums;

namespace Pratice.ViewModels;

public class NetWorthViewModel
{
    public List<Asset> Assets { get; set; } = new();
    public List<Liability> Liabilities { get; set; } = new();

    public decimal TotalAssets => Assets.Sum(a => a.Value);
    public decimal TotalLiabilities => Liabilities.Sum(l => l.Amount);
    public decimal NetWorth => TotalAssets - TotalLiabilities;

    public Dictionary<AssetType, decimal> AssetsByType { get; set; } = new();

    public string AssetBreakdownJson { get; set; } = "{}";
}
