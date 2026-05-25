const Methods = require("../skrypt.js");

describe("potega", () => {
	it("should raise to power correctly", () => {
		const cases = [
			{ podstawa: 2, wykladnik: 3, kontrolka: 1, expected: 8.0 },
			{ podstawa: 5, wykladnik: 0, kontrolka: 1, expected: 1.0 },
			{ podstawa: 7, wykladnik: 1, kontrolka: 1, expected: 7.0 },
			{ podstawa: -2, wykladnik: 3, kontrolka: 1, expected: -8.0 },
			{ podstawa: -2, wykladnik: 4, kontrolka: 1, expected: 16.0 },
			{ podstawa: 0, wykladnik: 5, kontrolka: 1, expected: 0.0 }
		];

		cases.forEach(({ podstawa, wykladnik, kontrolka, expected }) => {
			const result = Methods.potega(podstawa, wykladnik, kontrolka);
			expect(result).toBeCloseTo(expected, 10);
		});
	});

	it("should control flag work", () => {
		const expectedMessage = "trzeci argument jest mniejszy od 0";
		expect(() => Methods.potega(2, 2, 0)).toThrowError(expectedMessage);
	});

	it("should throw when exponent smaller than zero", () => {
		const expectedMessage = "wykladnik mniejszy od 0";
		expect(() => Methods.potega(2, -1, 1)).toThrowError(expectedMessage);
	});
});
