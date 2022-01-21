using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(Product product)
        {
            var obj = dbSet.FirstOrDefault(p => p.Id == product.Id);

            if (obj != null)
            {
                obj.Title = product.Title;
                obj.ISBN = product.ISBN;
                obj.Price = product.Price;
                obj.Price50 = product.Price50;
                obj.Price100 = product.Price100;
                obj.ListPrice = product.ListPrice;
                obj.Description = product.Description;
                obj.CategoryId = product.CategoryId;
                obj.CoverTypeId = product.CoverTypeId;
                obj.Author = product.Author;

                if(product.ImageUrl != null)
                {
                    obj.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
