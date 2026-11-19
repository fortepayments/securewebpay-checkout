(function () {
  const ccTypes = [
    { value: "10", label: "Sale" },
    { value: "11", label: "Auth" },
    { value: "13", label: "Credit" }
  ];
  const ecTypes = [
    { value: "20", label: "Sale" },
    { value: "21", label: "Auth" },
    { value: "23", label: "Credit" }
  ];

  const secureFields = document.getElementById("secure-fields");
  const typeSelect = document.getElementById("PgTransactionType");
  if (!secureFields || !typeSelect) {
    return;
  }

  function selectedValue(name) {
    const checked = document.querySelector('input[name="' + name + '"]:checked');
    return checked ? checked.value : "";
  }

  function fillTypes(method) {
    const types = method === "ec" ? ecTypes : ccTypes;
    const current = typeSelect.value;
    typeSelect.innerHTML = "";
    types.forEach(function (item) {
      const option = document.createElement("option");
      option.value = item.value;
      option.textContent = item.label;
      typeSelect.appendChild(option);
    });
    const match = types.some(function (item) { return item.value === current; });
    typeSelect.value = match ? current : types[0].value;
  }

  function syncUi() {
    const secure = selectedValue("Mode") === "trans";
    secureFields.style.display = secure ? "block" : "none";
    fillTypes(selectedValue("PaymentMethod"));
  }

  document.querySelectorAll('input[name="Mode"], input[name="PaymentMethod"]').forEach(function (input) {
    input.addEventListener("change", syncUi);
  });

  syncUi();
})();
