function togglePasswordVisibility(fieldId) {
    var passwordField = document.getElementById(fieldId);
    var toggleIcon = document.getElementById("toggle" + fieldId);
    if (passwordField.type === "password") {
        passwordField.type = "text";
        toggleIcon.classList.remove("bi-eye-slash-fill");
        toggleIcon.classList.add("bi-eye-fill");
    } else {
        passwordField.type = "password";
        toggleIcon.classList.remove("bi-eye-fill");
        toggleIcon.classList.add("bi-eye-slash-fill");
    }
}