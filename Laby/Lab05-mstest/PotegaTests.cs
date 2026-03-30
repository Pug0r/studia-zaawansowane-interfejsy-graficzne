using Lab03;

namespace Lab05_mstest
{
    [TestClass]
    public sealed class PotegaTests
    {
        [TestMethod]
        [DataRow(2, 3, 1, 8.0)]    
        [DataRow(5, 0, 1, 1.0)]    
        [DataRow(7, 1, 1, 7.0)]    
        [DataRow(-2, 3, 1, -8.0)] 
        [DataRow(-2, 4, 1, 16.0)]  
        [DataRow(0, 5, 1, 0.0)]
        public void should_raise_to_power_correctly(int podstawa, int wykladnik, int kontrolka, double expected)
        {
            // Act
            double result = Methods.potega(podstawa, wykladnik, kontrolka);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void should_control_flag_work()
        {
            // Arrange
            var expectedMessage = "trzeci argument jest mniejszy od 0";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Methods.potega(2, 2, 0));
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [TestMethod]
        public void should_throw_when_exponent_smaller_than_zero()
        {
            // Arrange
            var expectedMessage = "wykladnik mniejszy od 0";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Methods.potega(2, -1, 1));
            Assert.AreEqual(expectedMessage, ex.Message);
        }

    }
}
