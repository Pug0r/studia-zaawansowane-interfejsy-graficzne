using Lab03;

namespace Lab05_mstest
{
    [TestClass]
    public sealed class PoleKolaTests
    {
        [TestMethod]
        [DataRow(1, Math.PI)]
        [DataRow(2, 4 * Math.PI)]
        [DataRow(5.5, 30.25 * Math.PI)]
        public void should_calculate_area_correctly(double promien, double expected)
        {
            // Act
            double result = Methods.poleKola(promien);

            // Assert
            Assert.AreEqual(expected, result, 0.000001);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5.5)]
        public void should_throw_for_invalid_radius_and_have_correct_message(double promien)
        {
            // Arrange
            string expectedMessage = "promien musi byc wiekszy od 0";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Methods.poleKola(promien));
            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}