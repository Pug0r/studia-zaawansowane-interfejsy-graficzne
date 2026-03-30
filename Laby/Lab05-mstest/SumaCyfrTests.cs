using Lab03;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab05_mstest
{
    [TestClass]
    public sealed class SumaCyfrTests
    {
        private const string MsgNotThreeDigits = "liczba nie jest trzycyfrowa";
        private const string MsgDivisibleBy3 = "liczba podzielna przez 3";
        private const string MsgNotDivisibleBy3 = "liczba nie jest podzielna przez 3";

        [TestMethod]
        [DataRow(120, MsgDivisibleBy3)]
        [DataRow(300, MsgDivisibleBy3)]
        [DataRow(999, MsgDivisibleBy3)]
        public void should_return_divisible_by_3_message(double liczba, string expected)
        {
            // Act
            string result = Methods.sumaCyfr(liczba);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(100, MsgNotDivisibleBy3)]
        [DataRow(101, MsgNotDivisibleBy3)]
        [DataRow(998, MsgNotDivisibleBy3)]
        public void should_return_not_divisible_by_3_message(double liczba, string expected)
        {
            // Act
            string result = Methods.sumaCyfr(liczba);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(99, MsgNotThreeDigits)]
        [DataRow(1000, MsgNotThreeDigits)]
        [DataRow(123.5, MsgNotThreeDigits)]
        [DataRow(-120, MsgNotThreeDigits)]
        public void should_return_invalid_number_message(double liczba, string expected)
        {
            // Act
            string result = Methods.sumaCyfr(liczba);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}