using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureProducts.BLL.Interfaces;
using AzureProducts.BLL.Infrastructure;
using AzureProducts.DAL.Models;
using AzureProducts.DAL.DataAccess;
using AutoMapper;

namespace AzureProducts.BLL.Services
{
    public class ProductsManager : IProductService
    {
        private readonly EntityFrameworkProductsRepository _db = new EntityFrameworkProductsRepository();

        static ProductsManager()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProductDto, Product>()
                .ForMember(d => d.ProductID, m => m.MapFrom(p => p.ProductID))
                .ForMember(d => d.Class, m => m.MapFrom(p => p.Class))
                .ForMember(d => d.Color, m => m.MapFrom(p => p.Color))
                .ForMember(d => d.DaysToManufacture, m => m.MapFrom(p => p.DaysToManufacture))
                .ForMember(d => d.DiscontinuedDate, m => m.MapFrom(p => p.DiscontinuedDate))
                .ForMember(d => d.FinishedGoodsFlag, m => m.MapFrom(p => p.FinishedGoodsFlag))
                .ForMember(d => d.ListPrice, m => m.MapFrom(p => p.ListPrice))
                .ForMember(d => d.MakeFlag, m => m.MapFrom(p => p.MakeFlag))
                .ForMember(d => d.ModifiedDate, m => m.MapFrom(p => p.ModifiedDate))
                .ForMember(d => d.Name, m => m.MapFrom(p => p.Name))
                .ForMember(d => d.ProductLine, m => m.MapFrom(p => p.ProductLine))
                .ForMember(d => d.ProductModelID, m => m.MapFrom(p => p.ProductModelID))
                .ForMember(d => d.ProductNumber, m => m.MapFrom(p => p.ProductNumber))
                .ForMember(d => d.ProductSubcategoryID, m => m.MapFrom(p => p.ProductSubcategoryID))
                .ForMember(d => d.ReorderPoint, m => m.MapFrom(p => p.ReorderPoint))
                .ForMember(d => d.rowguid, m => m.MapFrom(p => p.rowguid))
                .ForMember(d => d.SafetyStockLevel, m => m.MapFrom(p => p.SafetyStockLevel))
                .ForMember(d => d.SellEndDate, m => m.MapFrom(p => p.SellEndDate))
                .ForMember(d => d.SellStartDate, m => m.MapFrom(p => p.SellStartDate))
                .ForMember(d => d.Size, m => m.MapFrom(p => p.Size))
                .ForMember(d => d.SizeUnitMeasureCode, m => m.MapFrom(p => p.SizeUnitMeasureCode))
                .ForMember(d => d.StandardCost, m => m.MapFrom(p => p.StandardCost))
                .ForMember(d => d.Style, m => m.MapFrom(p => p.Style))
                .ForMember(d => d.Weight, m => m.MapFrom(p => p.Weight))
                .ForMember(d => d.WeightUnitMeasureCode, m => m.MapFrom(p => p.WeightUnitMeasureCode));
            });
        }

        public void AddProduct(ProductDto productDto)
        {
            productDto.ModifiedDate = DateTime.Now;
            productDto.rowguid = Guid.NewGuid();

            var product = Mapper.Map<Product>(productDto);

            _db.Add(product);
            _db.Save();
        }

        public void DeleteProductById(int id)
        {
            _db.Delete(id);
            _db.Save();
        }
        
        public IEnumerable<ProductDto> GetAllProducts()
        {
            var products = _db.GetAll();
            return Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
        }

        public ProductDto GetProductById(int id)
        {
            var product = _db.Get(id);
            return Mapper.Map<ProductDto>(product);
        }

        public void UpdateProduct(ProductDto productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException();
            if (_db.Get(productDto.ProductID) == null)
                throw new OperationCanceledException("Product to be updated wasn't found");

            var product = Mapper.Map<Product>(productDto);

            product.ModifiedDate = DateTime.Now;

            _db.Update(product);
            _db.Save();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
