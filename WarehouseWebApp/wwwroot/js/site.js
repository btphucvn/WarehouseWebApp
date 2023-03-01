// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function numberWithCommas(x) {
    return (x = x + '').replace(new RegExp('\\B(?=(\\d{3})+' + (~x.indexOf('.') ? '\\.' : '$') + ')', 'g'), ',');
}