var button= $(".mobile-user-account-language .icon")
button.click(function () {
    $(".mobile-user-account-language .menulist").slideToggle()
    $(".mobile-user-account-language .fa-solid").toggleClass("active")
})