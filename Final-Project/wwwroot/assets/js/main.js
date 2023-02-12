const btn = document.getElementById('btn')
const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
const recognition = new SpeechRecognition();
recognition.continuous = true;
recognition.onstart = function () {
    document.getElementById('result').innerHTML = " "
    btn.style.color = "red"
}
recognition.onend = function () {
    btn.style.color = "#009DE0"
}
recognition.onresult = function (event) {
   
    var last = event.results.length - 1;
    const text = event.results[last][0].transcript;
   
    console.log(text)
    if (text != " ") {
        read(text);
    }
}
function read(text) {
    var speech = new SpeechSynthesisUtterance();
    speech.text = "I am not able to understand"
    if (text.includes('time')) {
        speech.text = 'It is' + new Date().getHours() + "" + new Date().getMinutes() + " right now"
    }
    else if (text.includes('hello')) {
        speech.rate = 0.3;
        speech.text = "Hello"
    }
    else if (text.includes('hi')) {
        speech.rate = 0.3;
        speech.text = "Hi!"
    }
    else if (text.includes('how are you')) {
        speech.rate = 0.3;
        speech.text = "I am fine and you?"
    }
    else if (text.includes('fine')) {
        speech.rate = 0.3;
        speech.text = "Very well!"
    }
    else if (text.includes('goodbye')) {
        speech.rate = 0.3;
        speech.text = "Good bye!"
        recognition.stop()
    }
    else if (text.includes('can you find the restaurant')) {
        speech.rate = 0.3;
        speech.text = "Yes tel me please restaurant name"
    }
    else if (text.includes('what is your name')) {
        speech.rate = 0.3;
        speech.text = "My name is sam"
    }
    else if (text.includes('my orders')) {

        var btnshoworder = document.getElementById("ordershowc");
        if (btnshoworder != null) {
            speech.text = "Okey!"
            btnshoworder.click()
        } else {
            speech.text = "Are you sure?  you do not have an order!"
        }


    }

    else if (text.includes('Sam')) {
        speech.rate = 0.3;
        speech.text = "I am here"
    }
    else if (text.includes('open restaurants')) {
        speech.text = "I am opening"
        window.open('https://localhost:44378/restaurant')
    }
    else if (text.includes('open store')) {
        speech.text = "I am opening "
        window.open('https://localhost:44378/store')
    }

    speech.rate = 1;
    speech.pitch = 2;

    window.speechSynthesis.speak(speech);


}