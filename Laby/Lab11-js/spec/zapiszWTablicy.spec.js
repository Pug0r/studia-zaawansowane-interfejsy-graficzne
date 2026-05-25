const Methods = require("../skrypt.js");

describe("zapiszWTablicy", () => {
	it("should multiply elements correctly", () => {
		const input = [1, 2, 3];
		const mnoznik = 10;
		const expected = [10, 20, 30];

		const result = Methods.zapiszWTablicy(input, mnoznik);

		expect(result).toEqual(expected);
	});

	it("should not modify input array", () => {
		const input = [1, 2, 3];
		const inputConst = [1, 2, 3];
		const mnoznik = 10;
		const expected = [10, 20, 30];

		const result = Methods.zapiszWTablicy(input, mnoznik);

		expect(input).toEqual(inputConst);
		expect(result).toEqual(expected);
	});

	it("should return empty array when input is empty", () => {
		const input = [];

		const result = Methods.zapiszWTablicy(input, 5);

		expect(result.length).toBe(0);
	});

	it("should throw when array is null", () => {
		expect(() => Methods.zapiszWTablicy(null, 5)).toThrowError();
	});
});
