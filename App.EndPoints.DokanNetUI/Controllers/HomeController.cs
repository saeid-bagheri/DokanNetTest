﻿using App.Domain.Core.Services.Buyers.Queries;
using App.EndPoints.DokanNetUI.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App.EndPoints.DokanNetUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGetOpenAuctions _getOpenAuctions;
        private readonly IGetNormalProducts _getNormalProducts;
        private readonly IGetParentCategories _getParentCategories;

        public HomeController(IGetOpenAuctions getOpenAuctions, IMapper mapper,
                              IGetNormalProducts getNormalProducts, IGetParentCategories getParentCategories)
        {
            _mapper = mapper;
            _getOpenAuctions = getOpenAuctions;
            _getNormalProducts = getNormalProducts;
            _getParentCategories = getParentCategories;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var homeVM = new HomeVM();

            //get open auctions
            _mapper.Map(await _getOpenAuctions.Execute(cancellationToken), homeVM.OpenAuctions);

            //get last 10 normal products
            _mapper.Map((await _getNormalProducts.Execute(cancellationToken)).Take(10), homeVM.NormalProducts);

            //get parent categories
            _mapper.Map((await _getParentCategories.Execute(cancellationToken)), homeVM.ParentCategories);

            return View(homeVM);
        }





















        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}