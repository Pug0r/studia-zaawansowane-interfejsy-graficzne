const Methods = require("../skrypt.js");

describe("zamienElementy", () => {
	const msgIndexOutOfRange = "indeks spoza zakresu";

	it("should swap elements correctly", () => {
		const input = ["A", "B", "C"];
		const expected = ["C", "B", "A"];
		const index1 = 0;
		const index2 = 2;
		const flag = 1;

		const result = Methods.zamienElementy(input, index1, index2, flag);

		expect(result).toEqual(expected);
	});

	it("should return original array if flag is zero or less", () => {
		const input = [1, 2, 3];
		const expected = [1, 2, 3];
		const flag = 0;

		const result = Methods.zamienElementy(input, 0, 1, flag);

		expect(result).toEqual(expected);
	});

	it("should throw when indices out of range", () => {
		const input = ["data", "test"];
		const cases = [
			{ i1: 5, i2: 1 },
			{ i1: -1, i2: 0 },
			{ i1: 0, i2: 10 }
		];

		cases.forEach(({ i1, i2 }) => {
			expect(() => Methods.zamienElementy(input, i1, i2, 1))
				.toThrowError(msgIndexOutOfRange);
		});
	});
});
