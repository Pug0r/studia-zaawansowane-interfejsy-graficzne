// JavaScript source code

const Methods = {
    potega(podstawa, wykladnik, kontrolka) {
        if (kontrolka <= 0) {
            throw new Error("trzeci argument jest mniejszy od 0");
        }
        if (wykladnik < 0) {
            throw new Error("wykladnik mniejszy od 0");
        }
        return Math.pow(podstawa, wykladnik);
    },

    zapiszWTablicy(tablica, mnoznik) {
        return tablica.map(x => x * mnoznik);
    },

    poleKola(promien) {
        if (promien <= 0) {
            throw new Error("promien musi byc wiekszy od 0");
        }
        return promien * promien * Math.PI;
    },

    sumaCyfr(liczba) {
        const isInteger = (liczba % 1 === 0);
        const hasThreeLeadingDigits = liczba >= 100 && liczba < 1000;
        if (isInteger === false || hasThreeLeadingDigits === false) {
            return "liczba nie jest trzycyfrowa";
        }

        return (liczba % 3 === 0) ? "liczba podzielna przez 3" : "liczba nie jest podzielna przez 3";
    },

    zamienElementy(tablica, indeksJeden, indeksDwa, flaga) {
        if (flaga <= 0) {
            return tablica;
        }

        const isValidIndex = (i) => i >= 0 && i < tablica.length;

        if (isValidIndex(indeksJeden) === false || isValidIndex(indeksDwa) === false) {
            throw new Error("indeks spoza zakresu");
        }

        [tablica[indeksJeden], tablica[indeksDwa]] = [tablica[indeksDwa], tablica[indeksJeden]];
        return tablica;
    }
};

// export for tests
if (typeof module !== "undefined" && module.exports) {
    module.exports = Methods;
}


// use in html page 
if (typeof window !== "undefined" && typeof document !== "undefined") {
    window.Methods = Methods;

    document.addEventListener("DOMContentLoaded", () => {
        const button = document.getElementById("callFunctions");
        if (!button) {
            return;
        }

        const getValue = (id) => {
            const el = document.getElementById(id);
            return el ? el.value : "";
        };

        const setResult = (id, value) => {
            const el = document.getElementById(id);
            if (el) {
                el.value = value;
            }
        };

        const parseNumberArray = (raw) => {
            if (raw.trim() === "") {
                return [];
            }
            return raw.split(",").map(item => Number(item.trim()));
        };

        const parseStringArray = (raw) => {
            if (raw.trim() === "") {
                return [];
            }
            return raw.split(",").map(item => item.trim());
        };

        const formatArray = (arr) => Array.isArray(arr) ? arr.join(", ") : String(arr);

        button.addEventListener("click", () => {
            try {
                const podstawa = parseInt(getValue("PotegaInputArg1"), 10);
                const wykladnik = parseInt(getValue("PotegaInputArg2"), 10);
                const kontrolka = parseInt(getValue("PotegaInputArg3"), 10);
                const result = Methods.potega(podstawa, wykladnik, kontrolka);
                setResult("PotegaResult", String(result));
            } catch (error) {
                setResult("PotegaResult", error.message);
            }

            try {
                const tablica = parseNumberArray(getValue("zapiszWTablicyArg1"));
                const mnoznik = Number(getValue("zapiszWTablicyArg2"));
                const result = Methods.zapiszWTablicy(tablica, mnoznik);
                setResult("zapiszWTablicyResult", formatArray(result));
            } catch (error) {
                setResult("zapiszWTablicyResult", error.message);
            }

            try {
                const promien = Number(getValue("poleKolaArg1"));
                const result = Methods.poleKola(promien);
                setResult("poleKolaResult", String(result));
            } catch (error) {
                setResult("poleKolaResult", error.message);
            }

            try {
                const liczba = Number(getValue("sumaCyfrArg1"));
                const result = Methods.sumaCyfr(liczba);
                setResult("sumaCyfrResult", String(result));
            } catch (error) {
                setResult("sumaCyfrResult", error.message);
            }

            try {
                const tablica = parseStringArray(getValue("zamienElementyArg1"));
                const indeksJeden = parseInt(getValue("zamienElementyArg2"), 10);
                const indeksDwa = parseInt(getValue("zamienElementyArg3"), 10);
                const flaga = parseInt(getValue("zamienElementyArg4"), 10);
                const result = Methods.zamienElementy(tablica, indeksJeden, indeksDwa, flaga);
                setResult("zamienElementyResult", formatArray(result));
            } catch (error) {
                setResult("zamienElementyResult", error.message);
            }
        });
    });
}
