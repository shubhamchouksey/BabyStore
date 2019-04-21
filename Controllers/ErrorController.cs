using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BabyStore.Controllers
{
    public class ErrorController : Controller
    {
        //GET: Error
        public IActionResult FileUploadLimitExceeded()
        {
            return View();
        }
        public IActionResult PageNotFound()
        {
            return View();
        }
        public IActionResult InternalServerError()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}