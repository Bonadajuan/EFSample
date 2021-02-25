using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EFSample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace EFSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdvDbContext _db;

        // public HomeController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;
        // }

        public HomeController(ILogger<HomeController> logger, AdvDbContext dbContext) 
        {
            _logger = logger;
            _db = dbContext;
        }

        public IActionResult Index()
        {
            return View();
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

        //Obtener una lista de todos los productos
        public IActionResult GetAll() 
        {
            List<Product> list;
            string sql = "EXEC SalesLT.Product_GetAll";
            list = _db.Products.FromSqlRaw<Product>(sql).ToList();
            Debugger.Break();
            return View("Index");
        }

        //Obtener un producto
        public IActionResult GetAProduct() 
        {
            List<Product> list;
            string sql = "EXEC SalesLT.Product_Get @ProductID";
            
            List<SqlParameter> parms = new List<SqlParameter> 
            {
                // Create parameter(s)    
                new SqlParameter { ParameterName = "@ProductID", Value = 706 }
            };
            
            list = _db.Products.FromSqlRaw<Product>(sql, parms.ToArray()).ToList();
            
            Debugger.Break();
            
            return View("Index");
        }
        public IActionResult CountAll() 
        {
            ScalarInt value;
            string sql = "EXEC SalesLT.Product_CountAll";
            // Como devuelve un valor se convierte al mismo en una lista enumerable
            value = _db.ScalarIntValue.FromSqlRaw<ScalarInt>(sql).AsEnumerable().FirstOrDefault();
            Debugger.Break();
            return View("Index");
        }
        public IActionResult UpdateListPrice() 
        {
            int rowsAffected;
            string sql = "EXEC SalesLT.Product_UpdateListPrice @ProductID, @ListPrice";
            
            List<SqlParameter> parms = new List<SqlParameter>
            { 
                // Create parameters    
                new SqlParameter { ParameterName = "@ProductID", Value = 706 },
                new SqlParameter { ParameterName = "@ListPrice", Value = 1500 }  
            };
            
            // Se utiliza el método ExecuteSqlRaw () para llamar a un procedimiento almacenado de modificación de datos.
            rowsAffected = _db.Database.ExecuteSqlRaw(sql, parms.ToArray());
            
            Debugger.Break();
        
            return View("Index");
        }

        public IActionResult MultipleResultSets() 
        {  
            List<Product> black = new List<Product>();  
            List<Product> red = new List<Product>();  
            DbCommand cmd;  
            DbDataReader rdr;  
            
            string sql = "EXEC SalesLT.MultipleResultsColors";
            
            // Build command object  
            cmd = _db.Database.GetDbConnection().CreateCommand();  
            cmd.CommandText = sql;  
        
            // Open database connection  
            _db.Database.OpenConnection();  
            
            // Create a DataReader  
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);  
        
            // Build collection of Black products  
            while (rdr.Read()) 
            {    
                black.Add(new Product    
                {      
                    ProductID = rdr.GetInt32(0),      
                    Name = rdr.GetString(1),      
                    ProductNumber = rdr.GetString(2)    
                });  
            }
            
            // Advance to the next result set  
            rdr.NextResult();
            
            // Build collection of Red products  
            while (rdr.Read()) 
            {
                red.Add(new Product
                {
                    ProductID = rdr.GetInt32(0),
                    Name = rdr.GetString(1),
                    ProductNumber = rdr.GetString(2)
                });
            }
            
            Debugger.Break();
            
            // Close Reader and Database Connection  
            rdr.Close();    
            return View("Index");
        }


    }
}
