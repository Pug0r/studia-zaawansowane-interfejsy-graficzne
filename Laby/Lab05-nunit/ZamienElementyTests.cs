using Lab03;

namespace Lab05_nunit
{
    [TestFixture]
    public sealed class ZamienElementyTests
    {
        private const string MsgIndexOutOfRange = "indeks spoza zakresu";

        [Test]
        public void should_swap_elements_correctly()
        {
            // Arrange
            object[] input = { "A", "B", "C" };
            object[] expected = { "C", "B", "A" };
            int index1 = 0;
            int index2 = 2;
            int flag = 1;

            // Act
            var result = Methods.zamienElementy(input, index1, index2, flag);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void should_return_original_array_if_flag_is_zero_or_less()
        {
            // Arrange
            object[] input = { 1, 2, 3 };
            object[] expected = { 1, 2, 3 };
            int flag = 0;

            // Act
            var result = Methods.zamienElementy(input, 0, 1, flag);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(5, 1)]
        [TestCase(-1, 0)]
        [TestCase(0, 10)]
        public void should_throw_when_indices_out_of_range(int i1, int i2)
        {
            // Arrange
            object[] input = { "data", "test" };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                Methods.zamienElementy(input, i1, i2, 1));

            Assert.That(ex.Message, Is.EqualTo(MsgIndexOutOfRange));
        }
    }
}