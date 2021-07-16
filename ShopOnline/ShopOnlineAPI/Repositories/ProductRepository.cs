using Microsoft.EntityFrameworkCore;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ShopOnlineDBContext context) : base(context)
        {
        }


        public async Task<List<Product>> GetProductListByKeyWords(string keywords)
        {
            string[] words = keywords.Split(' ');

            StringBuilder sqlWhereConditions = new StringBuilder();


            //With Sign
            sqlWhereConditions.Append("(");
            for (int i = 0; i < words.Length; i++)
            {
                sqlWhereConditions.Append($"ProductName LIKE '%{words[i]}%' ");

                if(i < words.Length - 1)
                {
                    sqlWhereConditions.Append("AND ");
                }
            }
            sqlWhereConditions.Append(")");


            sqlWhereConditions.Append(" OR ");
            words = ConvertToUnSign.Convert(keywords).Split(' ');

            //With NoSign
            sqlWhereConditions.Append("(");
            for (int i = 0; i < words.Length; i++)
            {
                sqlWhereConditions.Append($"ProductNameNoSign LIKE '%{words[i]}%' ");

                if (i < words.Length - 1)
                {
                    sqlWhereConditions.Append("AND ");
                }
            }
            sqlWhereConditions.Append(")");


            return await context.Set<Product>()
                .FromSqlRaw($"SELECT * FROM Product WHERE {sqlWhereConditions}")
                .ToListAsync();
        }
    }
}
