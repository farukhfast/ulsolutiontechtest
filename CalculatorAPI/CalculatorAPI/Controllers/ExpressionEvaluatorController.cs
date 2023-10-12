using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Xml.Linq;
using System;
using System.Linq.Expressions;
using CalculatorAPI.Model;

namespace CalculatorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpressionEvaluatorController : ControllerBase
    {
        private readonly ILogger<ExpressionEvaluatorController> _logger;

        public ExpressionEvaluatorController(ILogger<ExpressionEvaluatorController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "ExpressionEvaluator")]
        public IActionResult Post([FromBody] string model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            string input = model;
            ExpressionEvaluator.EvaluateExpression(ref input);
            return Ok(input);
        }
    }
}