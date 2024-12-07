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

//like button on events index - don't have to refresh for it to update how many are going
document.querySelectorAll('.like-btn').forEach(button => {
    button.addEventListener('click', function (e) {
        e.preventDefault();
        const eventId = this.dataset.id;

        fetch(`/Event/ToggleLike/${eventId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
        .then(response => response.json()
            .then(data => {
                if (data.success) {
                    const likeCount = this.querySelector('.like-count');
                    likeCount.textContent = data.likes;

                    // Toggle icon based on like status
                    const icon = this.querySelector('i');
                    if (data.liked) {
                        icon.classList.remove('bi-hand-thumbs-up');
                        icon.classList.add('bi-hand-thumbs-up-fill');
                    } else {
                        icon.classList.remove('bi-hand-thumbs-up-fill');
                        icon.classList.add('bi-hand-thumbs-up');
                    }
                } else {
                    console.error('Error', data.message);
                }
            }))
        .catch(error => console.error('Error:', error));
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("createReviewForm");

    form.addEventListener("submit", function (e) {
        e.preventDefault(); // Prevent the default form submission

        const formData = new FormData(form);
        const url = '/Event/ReviewCreate'; // Update with your controller's action route

        fetch(url, {
            method: "POST",
            body: formData,
            headers: {
                "X-Requested-With": "XMLHttpRequest",
            },
        })
            .then((response) => response.json())
            .then((data) => {
                if (data.success) {
                    // If success, reload the page to reflect the new review
                    location.reload();
                } else {
                    // Display the error message
                    alert(data.message || "An error occurred while submitting the review.");
                }
            })
            .catch((error) => console.error("Error:", error));
    });
});

//products category filter
document.getElementById('categoryFilter').addEventListener('change', function() {
    const selectedCategory = this.value;
    const products = document.querySelectorAll('.product-card');

    products.forEach(product => {
        const category = product.getAttribute('data-category');
        if (selectedCategory == 'all' || category == selectedCategory) {
            product.classList.remove('hidden');
        } else {
            product.classList.add('hidden');
        }
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("createReviewForm");

    form.addEventListener("submit", function (e) {
        e.preventDefault(); // Prevent the default form submission

        const formData = new FormData(form);
        fetch(form.action, {
            method: "POST",
            body: formData,
        })
            .then((response) => response.json())
            .then((data) => {
                if (data.success) {
                    location.reload(); // Reload to reflect the new review
                } else {
                    alert(data.message || "An error occurred while submitting the review.");
                }
            })
            .catch((error) => console.error("Error:", error));
    });

    // Reset the form on modal close
    document.getElementById("createReviewModal").addEventListener("hidden.bs.modal", function () {
        form.reset();
    });
});
