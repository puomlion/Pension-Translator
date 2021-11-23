using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pension_Translator.Models;
using System.Data.SqlClient;
namespace Pension_Translator.Controllers
{
    public class HomeController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        List<Address> addresses = new List<Address>();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            con.ConnectionString = Pension_Translator.Properties.Resources.ConnectionString;
        }

        public IActionResult Index()
        {
            FetchData();
            return View(addresses);
        }
        private void FetchData()
        {
            if (addresses.Count > 0)
            {
                addresses.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [NotesID],[Title],[Notes],[LanguageCode] FROM [master].[dbo].[NOTES]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    addresses.Add(new Address()
                    {
                        NotesID = dr["NotesID"].ToString()
                    ,
                        Title = dr["Title"].ToString()
                    ,
                        Notes = dr["Notes"].ToString()
                    ,
                        LanguageCode = dr["LanguageCode"].ToString()
                    });
                }
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
