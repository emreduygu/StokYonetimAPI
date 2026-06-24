using StokYonetimAPI.Application.DTOs;
using StokYonetimAPI.Application.PagedResult;
using StokYonetimAPI.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StokYonetimAPI.Tests
{
    public static class ProductTestData
    {
        public static List<Product> GetSampleProducts() => [        new Product { Id = 1, Name = "Apple",  StockQuantity = 100, UnitPrice = 5.99m,  UserID = 1 },
        new Product { Id = 2, Name = "Banana", StockQuantity = 50,  UnitPrice = 2.49m,  UserID = 1 },
        new Product { Id = 3, Name = "Cherry", StockQuantity = 0,   UnitPrice = 12.99m, UserID = 2 },];

        public static CreateProductDTO ValidCreateDto() => new()
        {
            Id = 10,
            Name = "Orange",
            StockQuantity = 30,
            UnitPrice = 3.99m
        };

        public static PagedResult<Product> GetPagedProducts() => new()
        {
            Items = GetSampleProducts(),
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 3,
            TotalPages = 1

        };

}   } 