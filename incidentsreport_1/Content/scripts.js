﻿/* 
    The addressAutocomplete takes as parameters:
  - a container element (div)
  - callback to notify about address selection
  - geocoder options:
         - placeholder - placeholder text for an input element
     - type - location type
*/
function addressAutocomplete(containerElement, callback, options) {
    // create input element
    var inputElement = document.createElement("input");
    inputElement.setAttribute("type", "text");
    inputElement.setAttribute("placeholder", options.placeholder);
    if (containerElement == null) {
        return false;
    }
    containerElement.appendChild(inputElement);

    // add input field clear button
    var clearButton = document.createElement("div");
    clearButton.classList.add("clear-button");
    addIcon(clearButton);
    clearButton.addEventListener("click", (e) => {
        e.stopPropagation();
        inputElement.value = '';
        callback(null);
        clearButton.classList.remove("visible");
        closeDropDownList();
    });
    containerElement.appendChild(clearButton);

    /* Current autocomplete items data (GeoJSON.Feature) */
    var currentItems;

    /* Active request promise reject function. To be able to cancel the promise when a new request comes */
    var currentPromiseReject;

    /* Focused item in the autocomplete list. This variable is used to navigate with buttons */
    var focusedItemIndex;

    /* Execute a function when someone writes in the text field: */
    inputElement.addEventListener("input", function (e) {
        var currentValue = this.value;

        /* Close any already open dropdown list */
        closeDropDownList();

        // Cancel previous request promise
        if (currentPromiseReject) {
            currentPromiseReject({
                canceled: true
            });
        }

        if (!currentValue) {
            clearButton.classList.remove("visible");
            return false;
        }

        // Show clearButton when there is a text
        clearButton.classList.add("visible");

        /* Create a new promise and send geocoding request */
        var promise = new Promise((resolve, reject) => {
            currentPromiseReject = reject;

            var apiKey = "8b10ea0427ff4b849df64d0ccd42f4d2";
            var url = `https://api.geoapify.com/v1/geocode/autocomplete?text=${encodeURIComponent(currentValue)}&limit=5&apiKey=${apiKey}`;

            if (options.type) {
                url += `&type=${options.type}`;
            }

            fetch(url)
                .then(response => {
                    // check if the call was successful
                    if (response.ok) {
                        response.json().then(data => resolve(data));
                    } else {
                        response.json().then(data => reject(data));
                    }
                });
        });

        promise.then((data) => {
            currentItems = data.features;

            /*create a DIV element that will contain the items (values):*/
            var autocompleteItemsElement = document.createElement("div");
            autocompleteItemsElement.setAttribute("class", "autocomplete-items");
            containerElement.appendChild(autocompleteItemsElement);

            /* For each item in the results */
            data.features.forEach((feature, index) => {
                /* Create a DIV element for each element: */
                var itemElement = document.createElement("DIV");
                /* Set formatted address as item value */
                itemElement.innerHTML = feature.properties.formatted;

                /* Set the value for the autocomplete text field and notify: */
                itemElement.addEventListener("click", function (e) {
                    inputElement.value = currentItems[index].properties.formatted;

                    callback(currentItems[index]);

                    /* Close the list of autocompleted values: */
                    closeDropDownList();
                });

                autocompleteItemsElement.appendChild(itemElement);
            });
        }, (err) => {
            if (!err.canceled) {
                console.log(err);
            }
        });
    });

    /* Add support for keyboard navigation */
    inputElement.addEventListener("keydown", function (e) {
        var autocompleteItemsElement = containerElement.querySelector(".autocomplete-items");
        if (autocompleteItemsElement) {
            var itemElements = autocompleteItemsElement.getElementsByTagName("div");
            if (e.keyCode == 40) {
                e.preventDefault();
                /*If the arrow DOWN key is pressed, increase the focusedItemIndex variable:*/
                focusedItemIndex = focusedItemIndex !== itemElements.length - 1 ? focusedItemIndex + 1 : 0;
                /*and and make the current item more visible:*/
                setActive(itemElements, focusedItemIndex);
            } else if (e.keyCode == 38) {
                e.preventDefault();

                /*If the arrow UP key is pressed, decrease the focusedItemIndex variable:*/
                focusedItemIndex = focusedItemIndex !== 0 ? focusedItemIndex - 1 : focusedItemIndex = (itemElements.length - 1);
                /*and and make the current item more visible:*/
                setActive(itemElements, focusedItemIndex);
            } else if (e.keyCode == 13) {
                /* If the ENTER key is pressed and value as selected, close the list*/
                e.preventDefault();
                if (focusedItemIndex > -1) {
                    closeDropDownList();
                }
            }
        } else {
            if (e.keyCode == 40) {
                /* Open dropdown list again */
                var event = document.createEvent('Event');
                event.initEvent('input', true, true);
                inputElement.dispatchEvent(event);
            }
        }
    });

    function setActive(items, index) {
        if (!items || !items.length) return false;

        for (var i = 0; i < items.length; i++) {
            items[i].classList.remove("autocomplete-active");
        }

        /* Add class "autocomplete-active" to the active element*/
        items[index].classList.add("autocomplete-active");

        // Change input value and notify
        inputElement.value = currentItems[index].properties.formatted;
        callback(currentItems[index]);
    }

    function closeDropDownList() {
        var autocompleteItemsElement = containerElement.querySelector(".autocomplete-items");
        if (autocompleteItemsElement) {
            containerElement.removeChild(autocompleteItemsElement);
        }

        focusedItemIndex = -1;
    }

    function addIcon(buttonElement) {
        var svgElement = document.createElementNS("http://www.w3.org/2000/svg", 'svg');
        svgElement.setAttribute('viewBox', "0 0 24 24");
        svgElement.setAttribute('height', "24");

        var iconElement = document.createElementNS("http://www.w3.org/2000/svg", 'path');
        iconElement.setAttribute("d", "M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z");
        iconElement.setAttribute('fill', 'currentColor');
        svgElement.appendChild(iconElement);
        buttonElement.appendChild(svgElement);
    }

    /* Close the autocomplete dropdown when the document is clicked. 
        Skip, when a user clicks on the input field */
    document.addEventListener("click", function (e) {
        if (e.target !== inputElement) {
            closeDropDownList();
        } else if (!containerElement.querySelector(".autocomplete-items")) {
            // open dropdown list again
            var event = document.createEvent('Event');
            event.initEvent('input', true, true);
            inputElement.dispatchEvent(event);
        }
    });

}

addressAutocomplete(document.getElementById("autocomplete-container"), (data) => {
    console.log("Selected option: ");
    console.log(data);
}, {
    placeholder: "Enter an address here"
});

addressAutocomplete(document.getElementById("autocomplete-container-country"), (data) => {
    console.log("Selected country: ");
    console.log(data);
}, {
    placeholder: "Enter a country name here",
    type: "country"
});

addressAutocomplete(document.getElementById("autocomplete-container-city"), (data) => {
    console.log("Selected city: ");
    console.log(data);
}, {
    placeholder: "Enter a city name here"
});

function changeStatus() {
    var presentOpen = $("input[name='cbOpen']").prop("checked");
    var presentClosed = $("input[name='cbClosed']").prop("checked");

    // loop througyh all the rows
    $(".incidents-table tr").each(function (i) {
        // ignore first row (which isthe header)
        if (i > 0) {

            if ($(this).find("td:nth-child(7)").text() == "") {

                if (presentOpen) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            }

            if ($(this).find("td:nth-child(7)").text() != "") {

                if (presentClosed) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            }
        }
    });
}

//$("#autocomplete-container input").val();
//let customerAddressInput = $('#autocomplete-container input');
//let customerAddress = $('#customerAddress');

//customerAddressInput.keyup(function () {
//    customerAddress.empty();
//    customerAddress.append(customerAddressInput.val());
//});

function isEmail(email) {
    var EmailRegex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return EmailRegex.test(email);
}

let customerEmailValue = $('#customerEmail');

customerEmailValue.keyup(function () {
    getKeyUpValue(customerEmailValue);
});

let techEmailValue = $('#techEmail');

techEmailValue.keyup(function () {
    getKeyUpValue(techEmailValue);
});

function getKeyUpValue(element) {
    var emailMessageContainer = $('#emailValidationMessage');
    var message = "Incorrect email format";
    var email = false;

    email = isEmail(element.val());
    emailMessageContainer.empty();

    if (!email) {

        emailMessageContainer.append(message);
    }
}

function selectedState(item) {
    var selIndex = item.options[item.selectedIndex].innerHTML;
    $('#customerState').val(selIndex);
}

function selectedCustomer() {
    var value = $('#customersDropdown').val();
    $('#incidentCustomer').val(value);
}

function selectedProduct() {
    var value = $('#productsDropdown').val();
    $('#incidentProduct').val(value);
}

function selectedTech() {
    var value = $('#techDropdown').val();
    $('#incidentTech').val(value);
}

function selectedRegCustomer() {
    var value = $('#customersRegDropdown').val();
    $('#registrationCustomer').val(value);
}

function selectedRegProduct() {
    var value = $('#productsRegDropdown').val();
    $('#registrationProduct').val(value);
}

function createPaging(page, totalItems, topPage, baseUrl, searchTerm) {
    debugger;
    window.addEventListener('load', function () {


        var __HTMLPaging = [];
        var i = 0;
        var pages = parseInt(totalItems / topPage);

        if (totalItems % topPage != 0) {
            pages++;
        }


        for (i = 0; i < pages; i++) {
            var activeClass = '';
            if (page == (i + 1)) {
                activeClass = 'active';
            }

            __HTMLPaging.push('<li class="page-item ' + activeClass + '"><a class="page-link" href="' + baseUrl + '?top=' + topPage + '&page=' + (i + 1).toString() + '&searchTerm=' + searchTerm + '">' + (i + 1).toString() + '</a></li>');
        }
        $('.pagination').html(__HTMLPaging.join(''));
    });
}