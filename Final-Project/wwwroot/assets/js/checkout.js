$(document).ready(function () {
    var button= $(".mobile-user-account-language .icon")
    button.click(function () {
        $(".mobile-user-account-language .menulist").slideToggle()
        $(".mobile-user-account-language .fa-solid").toggleClass("active")
    })
    $("#isdelivery").click(function () {
        $("#pickup").toggle()
        $("#delivery").toggle()
        $("#adresinfo").toggle()

    })

    $("#iscash").click(function () {
        $("#card").toggle()
        $("#cash").toggle()
        $(".payment").toggle()
        
    })
})