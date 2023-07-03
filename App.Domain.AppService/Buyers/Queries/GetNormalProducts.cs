﻿using App.Domain.Core.DataAccess;
using App.Domain.Core.DtoModels;
using App.Domain.Core.Services.Buyers.Queries;
using App.Infrastructures.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Service.Buyers.Queries
{
    public class GetNormalProducts : IGetNormalProducts
    {
        private readonly IProductRepository _productRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IStoreRepository _storeRepository;


        public GetNormalProducts(IProductRepository productRepository, ISellerRepository sellerRepository,
                                 IStoreRepository storeRepository)
        {
            _productRepository = productRepository;
            _sellerRepository = sellerRepository;
            _storeRepository = storeRepository;
        }

        public async Task<List<ProductDto>> Execute(CancellationToken cancellationToken)
        {
            var normalProducts = (await _productRepository.GetAll(cancellationToken))
                .Where(p => !p.IsAuction && p.IsConfirmed && p.Stock > 0)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
            foreach (var product in normalProducts)
            {
                product.SellerName = (await _sellerRepository.GetById(product.StoreId, cancellationToken)).FirstName + " " +
                                    (await _sellerRepository.GetById(product.StoreId, cancellationToken)).LastName;
                product.StoreTitle = (await _storeRepository.GetById(product.StoreId, cancellationToken)).Title;
            }
            return normalProducts;
        }
    }
}
