const Methods = require("../skrypt.js");

describe("sumaCyfr", () => {
	const msgNotThreeDigits = "liczba nie jest trzycyfrowa";
	const msgDivisibleBy3 = "liczba podzielna przez 3";
	const msgNotDivisibleBy3 = "liczba nie jest podzielna przez 3";

	it("should return divisible by 3 message", () => {
		const cases = [
			{ liczba: 120, expected: msgDivisibleBy3 },
			{ liczba: 300, expected: msgDivisibleBy3 },
			{ liczba: 999, expected: msgDivisibleBy3 }
		];

		cases.forEach(({ liczba, expected }) => {
			const result = Methods.sumaCyfr(liczba);
			expect(result).toBe(expected);
		});
	});

	it("should return not divisible by 3 message", () => {
		const cases = [
			{ liczba: 100, expected: msgNotDivisibleBy3 },
			{ liczba: 101, expected: msgNotDivisibleBy3 },
			{ liczba: 998, expected: msgNotDivisibleBy3 }
		];

		cases.forEach(({ liczba, expected }) => {
			const result = Methods.sumaCyfr(liczba);
			expect(result).toBe(expected);
		});
	});

	it("should return invalid number message", () => {
		const cases = [
			{ liczba: 99, expected: msgNotThreeDigits },
			{ liczba: 1000, expected: msgNotThreeDigits },
			{ liczba: 123.5, expected: msgNotThreeDigits },
			{ liczba: -120, expected: msgNotThreeDigits }
		];

		cases.forEach(({ liczba, expected }) => {
			const result = Methods.sumaCyfr(liczba);
			expect(result).toBe(expected);
		});
	});
});
