using Lab03;

namespace Lab05_nunit
{
    [TestFixture]
    public sealed class SumaCyfrTests
    {
        private const string MsgNotThreeDigits = "liczba nie jest trzycyfrowa";
        private const string MsgDivisibleBy3 = "liczba podzielna przez 3";
        private const string MsgNotDivisibleBy3 = "liczba nie jest podzielna przez 3";

        [TestCase(120, MsgDivisibleBy3)]
        [TestCase(300, MsgDivisibleBy3)]
        [TestCase(999, MsgDivisibleBy3)]
        public void should_return_divisible_by_3_message(double liczba, string expected)
        {
            // Act
            string result = Methods.sumaCyfr(liczba);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(100, MsgNotDivisibleBy3)]
        [TestCase(101, MsgNotDivisibleBy3)]
        [TestCase(998, MsgNotDivisibleBy3)]
        public void should_return_not_divisible_by_3_message(double liczba, string expected)
        {
            // Act
            string result = Methods.sumaCyfr(liczba);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(99, MsgNotThreeDigits)]
        [TestCase(1000, MsgNotThreeDigits)]
        [TestCase(123.5, MsgNotThreeDigits)]
        [TestCase(-120, MsgNotThreeDigits)]
        public void should_return_invalid_number_message(double liczba, string expected)
        {
            // Act
            string result = Methods.sumaCyfr(liczba);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}