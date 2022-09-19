using Hw2;
using Xunit;

namespace Hw2Tests
{
    public class ParserTests
    {
        [Theory]
        [InlineData("+", CalculatorOperation.Plus)]
        [InlineData("-", CalculatorOperation.Minus)]
        [InlineData("*", CalculatorOperation.Multiply)]
        [InlineData("/", CalculatorOperation.Divide)]
        public void TestCorrectOperations(string operation, CalculatorOperation operationExpected)
        {
            var op = Parser.ParseOperation(operation);
            Assert.Equal(operationExpected, op);
        }

        [Theory]
        [InlineData("f", "+", "3")]
        [InlineData("3", "+", "f")]
        [InlineData("a", "+", "f")]
        public void TestParserWrongValues(string val1, string operation, string val2)
        {
            Assert.Throws<ArgumentException>(
                () =>
                {
                    Parser.ParseCalcArguments(
                        new[] { val1, operation, val2 },
                        out _,
                        out _,
                        out _);
                });
        }
        
        [Fact]
        public void TestParserWrongOperation()
        {
            var operation = Parser.ParseOperation("&");
            Assert.Equal(CalculatorOperation.Undefined, operation);
        }

        [Fact]
        public void TestParserWrongLength()
        {
           Assert.Throws<ArgumentException>(
                () =>
                {
                    Parser.ParseCalcArguments(
                        new[] { "3", "+", "5", "." },
                        out _,
                        out _,
                        out _);
                });
        }
    }
}