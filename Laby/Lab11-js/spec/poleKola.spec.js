const Methods = require("../skrypt.js");

describe("poleKola", () => {
    it("should calculate area correctly", () => {
        const cases = [
            { promien: 1, expected: Math.PI },
            { promien: 2, expected: 4 * Math.PI },
            { promien: 5.5, expected: 30.25 * Math.PI }
        ];

        cases.forEach(({ promien, expected }) => {
            const result = Methods.poleKola(promien);
            expect(result).toBeCloseTo(expected, 6);
        });
    });

    it("should throw for invalid radius and have correct message", () => {
        const cases = [0, -1, -5.5];
        const expectedMessage = "promien musi byc wiekszy od 0";

        cases.forEach((promien) => {
            expect(() => Methods.poleKola(promien)).toThrowError(expectedMessage);
        });
    });
});
