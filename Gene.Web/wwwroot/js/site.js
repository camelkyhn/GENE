// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Side Nav
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.sidenav');
    M.Sidenav.init(elems, {});
});

// Floating Button
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.fixed-action-btn');
    M.FloatingActionButton.init(elems, {});
});

// Collapsible
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.collapsible');
    M.Collapsible.init(elems, {});
});

document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.collapsible.expandable');
    M.Collapsible.init(elems, {
        accordion: false
    });
});

// Dropdown
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.dropdown-trigger');
    M.Dropdown.init(elems, {});
});

// Date Picker
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.gene-datepicker');
    M.Datepicker.init(elems, {
        format: "yyyy-mm-dd"
    });
});

// Modal Trigger
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.modal');
    M.Modal.init(elems, {});
});

// Select
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.gene-select');
    M.FormSelect.init(elems, {});
});

// Character Counter
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.gene-text, .gene-textarea');
    M.CharacterCounter.init(elems);
});

// Tooltip
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.tooltipped');
    M.Tooltip.init(elems);
});
