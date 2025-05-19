document.addEventListener("DOMContentLoaded", () => {
    const loginForm = document.querySelector("#loginForm");

    loginForm.addEventListener("submit", (event) => {
        event.preventDefault();

        // الحصول على قيم ID والباسورد
        const userId = document.querySelector("#userId").value.trim();
        const password = document.querySelector("#password").value.trim();

        // التحقق من القيم
        if (userId === "12345" && password === "@admin123") {
            window.location.href = "adminindex.html"; // توجيه المستخدم
        } else {
            alert("Incorrect ID or Password. Please try again!");
        }
    });

    // التعامل مع أزرار تسجيل الدخول الاجتماعي
    const socialButtons = document.querySelectorAll(".social-buttons button");

    socialButtons.forEach((button) => {
        button.addEventListener("click", () => {
            const platform = button.textContent.trim().split(" ")[2];
            alert(`You selected to continue with ${platform}!`);
        });
    });
});
