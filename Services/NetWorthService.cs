using System.Text.Json;
using Pratice.Data;
using Pratice.Models;
using Pratice.Models.Enums;
using Pratice.Services.Interfaces;
using Pratice.ViewModels;

namespace Pratice.Services;

public class NetWorthService : INetWorthService
{
    private readonly ApplicationDbContext _context;

    public NetWorthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Asset> GetAllAssets(string userId) =>
        _context.Assets.Where(a => a.UserId == userId).OrderBy(a => a.Type).ThenBy(a => a.Name).ToList();

    public Asset? GetAssetById(Guid id, string userId) =>
        _context.Assets.FirstOrDefault(a => a.Id == id && a.UserId == userId);

    public void CreateAsset(Asset asset, string userId)
    {
        asset.Id = Guid.NewGuid();
        asset.UserId = userId;
        asset.CreatedAt = DateTime.UtcNow;
        _context.Assets.Add(asset);
        _context.SaveChanges();
    }

    public void UpdateAsset(Asset asset, string userId)
    {
        var existing = _context.Assets.FirstOrDefault(a => a.Id == asset.Id && a.UserId == userId);
        if (existing == null) return;
        existing.Name = asset.Name;
        existing.Value = asset.Value;
        existing.Type = asset.Type;
        existing.Notes = asset.Notes;
        existing.AsOfDate = asset.AsOfDate;
        existing.UpdatedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }

    public void DeleteAsset(Guid id, string userId)
    {
        var asset = _context.Assets.FirstOrDefault(a => a.Id == id && a.UserId == userId);
        if (asset == null) return;
        _context.Assets.Remove(asset);
        _context.SaveChanges();
    }

    public List<Liability> GetAllLiabilities(string userId) =>
        _context.Liabilities.Where(l => l.UserId == userId).OrderBy(l => l.Name).ToList();

    public Liability? GetLiabilityById(Guid id, string userId) =>
        _context.Liabilities.FirstOrDefault(l => l.Id == id && l.UserId == userId);

    public void CreateLiability(Liability liability, string userId)
    {
        liability.Id = Guid.NewGuid();
        liability.UserId = userId;
        liability.CreatedAt = DateTime.UtcNow;
        _context.Liabilities.Add(liability);
        _context.SaveChanges();
    }

    public void UpdateLiability(Liability liability, string userId)
    {
        var existing = _context.Liabilities.FirstOrDefault(l => l.Id == liability.Id && l.UserId == userId);
        if (existing == null) return;
        existing.Name = liability.Name;
        existing.Amount = liability.Amount;
        existing.InterestRate = liability.InterestRate;
        existing.MinimumPayment = liability.MinimumPayment;
        existing.DueDate = liability.DueDate;
        existing.Notes = liability.Notes;
        existing.UpdatedAt = DateTime.UtcNow;
        _context.SaveChanges();
    }

    public void DeleteLiability(Guid id, string userId)
    {
        var liability = _context.Liabilities.FirstOrDefault(l => l.Id == id && l.UserId == userId);
        if (liability == null) return;
        _context.Liabilities.Remove(liability);
        _context.SaveChanges();
    }

    public decimal GetTotalAssets(string userId) =>
        _context.Assets.Where(a => a.UserId == userId).Sum(a => a.Value);

    public decimal GetTotalLiabilities(string userId) =>
        _context.Liabilities.Where(l => l.UserId == userId).Sum(l => l.Amount);

    public decimal GetNetWorth(string userId) =>
        GetTotalAssets(userId) - GetTotalLiabilities(userId);

    public Dictionary<AssetType, decimal> GetAssetsByType(string userId) =>
        _context.Assets.Where(a => a.UserId == userId)
            .GroupBy(a => a.Type)
            .ToDictionary(g => g.Key, g => g.Sum(a => a.Value));

    public NetWorthViewModel GetSummary(string userId)
    {
        var assets = GetAllAssets(userId);
        var liabilities = GetAllLiabilities(userId);
        var assetsByType = GetAssetsByType(userId);

        var chartData = new
        {
            labels = assetsByType.Keys.Select(k => k.ToString()).ToArray(),
            values = assetsByType.Values.Select(v => (double)v).ToArray(),
            colors = assetsByType.Keys.Select(k => GetAssetTypeColor(k)).ToArray()
        };

        return new NetWorthViewModel
        {
            Assets = assets,
            Liabilities = liabilities,
            AssetsByType = assetsByType,
            AssetBreakdownJson = JsonSerializer.Serialize(chartData)
        };
    }

    private static string GetAssetTypeColor(AssetType type) => type switch
    {
        AssetType.Cash => "#28a745",
        AssetType.BankAccount => "#17a2b8",
        AssetType.Investment => "#6f42c1",
        AssetType.Retirement => "#fd7e14",
        AssetType.RealEstate => "#20c997",
        AssetType.Vehicle => "#6c757d",
        AssetType.Cryptocurrency => "#ffc107",
        AssetType.Other => "#adb5bd",
        _ => "#6c757d"
    };
}
