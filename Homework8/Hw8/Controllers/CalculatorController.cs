using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        [FromQuery] string? val1,
        [FromQuery] string? operation,
        [FromQuery] string? val2)
    {
        if (!double.TryParse(val1, NumberStyles.Float, CultureInfo.InvariantCulture, out var value1) 
            || !double.TryParse(val2, NumberStyles.Float, CultureInfo.InvariantCulture, out var value2))
            return BadRequest(Messages.InvalidNumberMessage);
        if (value2 == 0 && operation == "Divide")
            return BadRequest(Messages.DivisionByZeroMessage);
        
        return operation switch
        {
            "Plus" => Ok(calculator.Plus(value1, value2)),
            "Minus" => Ok(calculator.Minus(value1, value2)),
            "Multiply" => Ok(calculator.Multiply(value1, value2)),
            "Divide" => Ok(calculator.Divide(value1, value2)),
            _ => BadRequest(Messages.InvalidOperationMessage)
        };
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}