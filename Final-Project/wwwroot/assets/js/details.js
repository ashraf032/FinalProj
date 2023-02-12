var img=document.querySelector(".bg")
var details=document.querySelector(".details")
var file=img.getAttribute("src")
details.style.backgroundImage = `url(${file})`;
details.style.backgroundSize = `cover`;

var prewicon='<svg xmlns="http://www.w3.org/2000/svg" class="HeroCarousel-module__caretIcon___ZySqK" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0z"></path><path fill="currentColor" d="M15.477 3.977a.503.503 0 01.365.841l-6.68 7.159 6.679 7.159a.502.502 0 01.159.366.5.5 0 01-.888.315l-7-7.5a.498.498 0 010-.682l7-7.5a.496.496 0 01.365-.16"></path></svg>'

var nexticon='<svg xmlns="http://www.w3.org/2000/svg" class="HeroCarousel-module__caretIcon___ZySqK" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0z"></path><path fill="currentColor" d="M8.5 20a.5.5 0 01-.365-.841L14.815 12 8.136 4.841a.5.5 0 11.729-.681l7 7.5c.18.192.18.49 0 .682l-7 7.5a.494.494 0 01-.365.16"></path></svg>'
$('.owl-carousel').owlCarousel({
    loop:true,
    nav:true,
    navText:[prewicon,nexticon],
    margin:0,
    responsiveClass:true,
    responsive:{
        0:{
            items:3,
            nav:true
        },
      
        700:{
            items:5,
            nav:false
        },
        900: {
            items: 8,
            nav: false
        },
        1000:{
            items:9,
            nav:true,
            loop:false
        }
    }
})

$(document).ready(function () {
    var button= $(".mobile-user-account-language .icon")
    button.click(function () {
        $(".mobile-user-account-language .menulist").slideToggle()
        $(".mobile-user-account-language .fa-solid").toggleClass("active")
    })
    $(window).scroll(function () {
        if ($(this).scrollTop() > 200) {
            $('#basket-main').css({ "position": "fixed", "top": "5%", "right":"3%","z-index":"999999999999" ,"height":"7%" });
        } else {
            $('#basket-main').css({ "position": "static","height":"auto"});
        }
       
        
        

    })

   
 })

 var Categorys=document.querySelectorAll(".category_Name")

 var Products=document.querySelectorAll(".products a")
  Categorys.forEach(category => {
      category.addEventListener("click",function (e) {
         
         var CategoryDataId= e.target.parentElement.getAttribute("data-id")
         e.preventDefault()
         Products.forEach(product => {
             
             var ProductDataId = product.getAttribute("data-id")
             console.log(ProductDataId)
          if(CategoryDataId!="All"){
            if(CategoryDataId==ProductDataId){
                product.style.display="block"
            }else{
             product.style.display="none"
            }
          }else{
              product.style.display="block"
          }
          
         });
      })
  });
 
 