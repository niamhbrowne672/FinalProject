// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Show the "Back to Top" button when scrolled down, hide it otherwise

// Show the "Back to Top" button after scrolling 200px
window.onscroll = function () {
    const button = document.getElementById("back-to-top");
    if (document.documentElement.scrollTop > 200) {
        button.style.display = "block";
    } else {
        button.style.display = "none";
    }
};

// Smooth scroll to top when the button is clicked
document.getElementById("back-to-top").onclick = function () {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};
