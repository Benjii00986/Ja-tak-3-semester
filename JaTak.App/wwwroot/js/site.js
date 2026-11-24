// Keeps the selected quantity between 1 and the number of items left.
document.querySelectorAll(".jatak-quantity-input").forEach(function (input) {
    input.addEventListener("change", function () {
        const min = parseInt(input.min);
        const max = parseInt(input.max);
        let value = parseInt(input.value);

        if (isNaN(value) || value < min) {
            value = min;
        } else if (value > max) {
            value = max;
        }

        input.value = value;
    });
});