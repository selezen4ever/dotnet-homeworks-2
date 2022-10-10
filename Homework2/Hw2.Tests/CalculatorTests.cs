using Hw2;
using Xunit;

namespace Hw2Tests
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData(15, 5, CalculatorOperation.Plus, 20)]
        [InlineData(15, 5, CalculatorOperation.Minus, 10)]
        [InlineData(15, 5, CalculatorOperation.Multiply, 75)]
        [InlineData(15, 5, CalculatorOperation.Divide, 3)]
        public void TestAllOperations(int value1, int value2, CalculatorOperation operation, int expectedValue)
        {
            var res = Calculator.Calculate(value1, operation, value2);
            Assert.Equal(expectedValue, res);
        }
        
        [Fact]
        public void TestInvalidOperation()
        {
            Assert.Throws<InvalidOperationException>(() => Calculator.Calculate(1, CalculatorOperation.Undefined, 3));
        }

        [Fact]
        public void TestDividingNonZeroByZero()
        {
            Assert.Throws<DivideByZeroException>(() => Calculator.Calculate(1, CalculatorOperation.Divide, 0));
        }

        [Fact]
        public void TestDividingZeroByNonZero()
        {
            var res = Calculator.Calculate(0, CalculatorOperation.Divide, 1);
            Assert.Equal(0, res);
        }
        
        [Fact]
        public void TestDividingZeroByZero()
        {
            Assert.Throws<DivideByZeroException>(
                () => Calculator.Calculate(0,CalculatorOperation.Divide, 0));
        }
    }
}