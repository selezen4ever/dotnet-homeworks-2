﻿namespace WebServer.Services.MathCalculator.Parser;

public interface IParser
{
    public ParsedExpression GetParsedExpression(string? expression);
}