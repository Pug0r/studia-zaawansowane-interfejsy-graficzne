using Lab03;

namespace Lab05_nunit
{
    [TestFixture]
    public sealed class PoleKolaTests
    {
        [TestCase(1, Math.PI)]
        [TestCase(2, 4 * Math.PI)]
        [TestCase(5.5, 30.25 * Math.PI)]
        public void should_calculate_area_correctly(double promien, double expected)
        {
            // Act
            double result = Methods.poleKola(promien);

            // Assert
            Assert.That(result, Is.EqualTo(expected).Within(0.000001));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5.5)]
        public void should_throw_for_invalid_radius_and_have_correct_message(double promien)
        {
            // Arrange
            string expectedMessage = "promien musi byc wiekszy od 0";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Methods.poleKola(promien));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
    }
}