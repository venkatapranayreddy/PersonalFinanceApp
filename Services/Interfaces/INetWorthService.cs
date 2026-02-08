using Pratice.Models;
using Pratice.Models.Enums;
using Pratice.ViewModels;

namespace Pratice.Services.Interfaces;

public interface INetWorthService
{
    List<Asset> GetAllAssets(string userId);
    Asset? GetAssetById(Guid id, string userId);
    void CreateAsset(Asset asset, string userId);
    void UpdateAsset(Asset asset, string userId);
    void DeleteAsset(Guid id, string userId);

    List<Liability> GetAllLiabilities(string userId);
    Liability? GetLiabilityById(Guid id, string userId);
    void CreateLiability(Liability liability, string userId);
    void UpdateLiability(Liability liability, string userId);
    void DeleteLiability(Guid id, string userId);

    decimal GetTotalAssets(string userId);
    decimal GetTotalLiabilities(string userId);
    decimal GetNetWorth(string userId);
    Dictionary<AssetType, decimal> GetAssetsByType(string userId);
    NetWorthViewModel GetSummary(string userId);
}
