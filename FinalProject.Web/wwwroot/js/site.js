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


// Breed Recommendation Page - Card descriptions - appear when user clicks learn more button
document.addEventListener("DOMContentLoaded", function () {
    //Set all full descriptions to be hidden initially
    document.querySelectorAll(".full-description").forEach(desc => {
        desc.style.display = 'none';
    });

    document.querySelectorAll('.toggle-description').forEach(button => {
        button.addEventListener('click', function () {
            const cardBody = this.parentElement;
            const shortDesc = cardBody.querySelector('.short-description');
            const fullDesc = cardBody.querySelector('.full-description');

            if (fullDesc.style.display == 'none') {
                //show the full description and hide the short one
                fullDesc.style.display = 'block';
                shortDesc.style.display = 'none';
                this.textContent = 'Show Less';
            } else {
                //show the short description and hide the full one
                fullDesc.style.display = 'none';
                shortDesc.style.display = 'block';
                this.textContent = 'Learn More';
            }

        });
    });

});

// // Search bar
// document.addEventListener('DOMContentLoaded', function() {
//     const searchBar = document.getElementById('search-bar');
//     const searchButton = document.getElementById('search-button');
//     const breedCardsContainer = document.querySelector('.breed-cards-container');
//     const breedCards = document.querySelectorAll('.card');
//     const noResultsMessage = document.createElement('div');
//     noResultsMessage.classList.add('no-results');
//     noResultsMessage.innerText = "Sorry, we don't have that breed! Try searching another one.";
//     breedCardsContainer.appendChild(noResultsMessage);

//     function handleSearch() {
//         const searchQuery = searchBar.value.toLowerCase();
//         let hasResults = false;

//         breedCards.forEach(function(card) {
//             const breedTitle = card.querySelector('.card-title').textContent.toLowerCase();
//             if (breedTitle.includes(searchQuery)) {
//                 card.style.display = 'block';
//                 hasResults = true;
//             } else {
//                 card.style.display = 'none';
//             }
//         });

//         if (searchQuery && !hasResults) {
//             noResultsMessage.style.display = 'block';
//             breedCardsContainer.classList.add('search-active');
//         } else {
//             noResultsMessage.style.display = 'none';
//             breedCardsContainer.classList.toggle('search-active', Boolean(searchQuery));
//         }
//     }

//     searchBar.addEventListener('input', handleSearch);
//     searchButton.addEventListener('click', handleSearch);
// });