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

// Shows a confirmation popup before the reservation is sent.
const confirmModalElement = document.getElementById("reservationConfirmModal");

if (confirmModalElement) {
    const confirmModal = new bootstrap.Modal(confirmModalElement);
    let selectedForm = null;
    let offerModalElement = null;

    document.querySelectorAll(".jatak-reservation-form").forEach(function (form) {
        form.addEventListener("submit", function (event) {
            // Stops the form from being sent right away.
            event.preventDefault();

            selectedForm = form;
            offerModalElement = form.closest(".modal");

            const quantity = parseInt(form.querySelector(".jatak-quantity-input").value);
            const price = parseFloat(form.dataset.price);
            const totalPrice = price * quantity;

            document.getElementById("confirm-product-name").textContent = form.dataset.productName;
            document.getElementById("confirm-quantity").textContent = quantity;
            document.getElementById("confirm-price").textContent = formatPrice(price);
            document.getElementById("confirm-total-price").textContent = formatPrice(totalPrice);

            // Opens the confirmation popup when the offer popup is fully closed.
            offerModalElement.addEventListener("hidden.bs.modal", function () {
                confirmModal.show();
            }, { once: true });

            bootstrap.Modal.getInstance(offerModalElement).hide();
        });
    });

    // Sends the reservation when the customer confirms.
    document.getElementById("confirm-reservation-button").addEventListener("click", function () {
        selectedForm.submit();
    });

    // Goes back to the offer when the customer cancels.
    document.getElementById("confirm-cancel-button").addEventListener("click", function () {
        confirmModalElement.addEventListener("hidden.bs.modal", function () {
            bootstrap.Modal.getInstance(offerModalElement).show();
        }, { once: true });

        confirmModal.hide();
    });
}

// Formats a number as a Danish price, e.g. 120,00 kr.
function formatPrice(amount) {
    return amount.toLocaleString("da-DK", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }) + " kr.";
}