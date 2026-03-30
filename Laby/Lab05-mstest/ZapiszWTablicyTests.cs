using Lab03;

namespace Lab05_mstest
{
    [TestClass]
    public sealed class ZapiszWTablicyTests
    {
        [TestMethod]
        public void should_multiply_elements_correctly()
        {
            // Arrange
            int[] input = { 1, 2, 3 };
            int mnoznik = 10;
            int[] expected = { 10, 20, 30 };

            // Act
            int[] result = Methods.zapiszWTablicy(input, mnoznik);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void should_not_modify_input_array()
        {
            // Arrange
            int[] input = { 1, 2, 3 };
            int[] inputConst = { 1, 2, 3 };
            int mnoznik = 10;
            int[] expected = { 10, 20, 30 };

            // Act
            int[] result = Methods.zapiszWTablicy(input, mnoznik);

            // Assert
            CollectionAssert.AreEqual(input, inputConst);
        }

        [TestMethod]
        public void should_return_empty_array_when_input_is_empty()
        {
            // Arrange
            int[] input = Array.Empty<int>();

            // Act
            int[] result = Methods.zapiszWTablicy(input, 5);

            // Assert
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void should_throw_when_array_is_null()
            => Assert.Throws<ArgumentNullException>(() => Methods.zapiszWTablicy(null, 5));
    }
}
