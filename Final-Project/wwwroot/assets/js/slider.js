var models=$(".slide")
   
let index=0
models.hide()
let slaytCount=models.length

var Settings={
    duration: '4000',
    slide: false
}

Init(Settings)
let prev
function Init(settings) {
 interval=   setInterval(function () {
    
            if(slaytCount==index+1){
                index=-1
            }
            index++
            
        
        showSlide(index)
    },settings.duration)
}

document.querySelectorAll('.prews').forEach(function (item) {
  item.addEventListener('mouseenter',function () {
      clearInterval(interval)
  })
})
document.querySelectorAll('.prews').forEach(function (item) {
    item.addEventListener('mouseleave',function () {
        Init(Settings)
    })
  })

  
$(".right-prew").click(function() {
    index ++;
    showSlide(index)
    console.log(index);
})
$(".left-prew").click(function() {
    index --;
    showSlide(index)
    console.log(index);
    
})

function showSlide(i){

    index = i;
    if (i<0) {
        index = slaytCount - 1;
    }
    if(i >= slaytCount){
        index =0;
    }   
    
    var MainSlide=document.querySelector(".slide-main")
   var Slides= document.querySelectorAll('.slide')[index].children
   var ImgElement=Slides[0].children
   var TextElement=Slides[0].children
   var  img=ImgElement[0].getAttribute("src")
    var textA = TextElement[1]
    var textB = textA.innerText
    MainSlide.children[0].children[0].setAttribute("src", img)
    document.querySelector(".title-slide").nextElementSibling.style.display="none"
    document.querySelector(".title-slide").innerHTML = textB

}