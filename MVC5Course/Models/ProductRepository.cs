using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
	{
        internal IQueryable<ProductViewModel> ProductViewModel()
        {
            var data = this.All()
                .Where(w => w.Active == true)
                .OrderByDescending(o => o.ProductId)
                .Take(10)
                .Select(s => new ProductViewModel()
                {
                    ProductId = s.ProductId,
                    ProductName = s.ProductName,
                    Price = s.Price,
                    Stock = s.Stock
                });

            return data;
        }

        internal Object TakeData(int takeNumber)
        {
            var data =
                this.All().OrderByDescending(p => p.ProductId)
                .Take(takeNumber)
                .ToList();

            return data;
        }

        internal Product Find(int id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }
        
	}

	public  interface IProductRepository : IRepository<Product>
	{

	}
}