using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pratice.Models;
using Pratice.Models.Enums;
using Pratice.Services.Interfaces;

namespace Pratice.Controllers;

public class NetWorthController : BaseController
{
    private readonly INetWorthService _netWorthService;

    public NetWorthController(INetWorthService netWorthService)
    {
        _netWorthService = netWorthService;
    }

    public IActionResult Index()
    {
        var viewModel = _netWorthService.GetSummary(UserId);
        return View(viewModel);
    }

    public IActionResult CreateAsset()
    {
        ViewBag.AssetTypes = new SelectList(Enum.GetValues<AssetType>());
        return View(new Asset { AsOfDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateAsset(Asset asset)
    {
        if (ModelState.IsValid)
        {
            _netWorthService.CreateAsset(asset, UserId);
            TempData["Success"] = "Asset added successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.AssetTypes = new SelectList(Enum.GetValues<AssetType>(), asset.Type);
        return View(asset);
    }

    public IActionResult EditAsset(Guid id)
    {
        var asset = _netWorthService.GetAssetById(id, UserId);
        if (asset == null)
        {
            TempData["Error"] = "Asset not found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.AssetTypes = new SelectList(Enum.GetValues<AssetType>(), asset.Type);
        return View(asset);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditAsset(Asset asset)
    {
        if (ModelState.IsValid)
        {
            _netWorthService.UpdateAsset(asset, UserId);
            TempData["Success"] = "Asset updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.AssetTypes = new SelectList(Enum.GetValues<AssetType>(), asset.Type);
        return View(asset);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteAsset(Guid id)
    {
        _netWorthService.DeleteAsset(id, UserId);
        TempData["Success"] = "Asset deleted successfully!";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult CreateLiability()
    {
        return View(new Liability());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateLiability(Liability liability)
    {
        if (ModelState.IsValid)
        {
            _netWorthService.CreateLiability(liability, UserId);
            TempData["Success"] = "Liability added successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(liability);
    }

    public IActionResult EditLiability(Guid id)
    {
        var liability = _netWorthService.GetLiabilityById(id, UserId);
        if (liability == null)
        {
            TempData["Error"] = "Liability not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(liability);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditLiability(Liability liability)
    {
        if (ModelState.IsValid)
        {
            _netWorthService.UpdateLiability(liability, UserId);
            TempData["Success"] = "Liability updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(liability);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteLiability(Guid id)
    {
        _netWorthService.DeleteLiability(id, UserId);
        TempData["Success"] = "Liability deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
}
