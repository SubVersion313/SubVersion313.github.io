const buttonCamera = dropArea.querySelector(".TakePhotos");

buttonCamera.onclick = () => {
  inputCam.click(); //if user click on the button then the input also clicked
};

//get the captured media file
let inputCam = document.getElementById("capture");

inputCam.addEventListener("change", (ev) => {
  console.dir(inputCam.files[0]);
  if (inputCam.files[0].type.indexOf("image/") > -1) {
    let img = document.getElementById("img");
    img.src = window.URL.createObjectURL(inputCam.files[0]);
    img.style.display = "block";
  }

  //  console.dir(input.files[0]);
  //  if (input.files[0].type.indexOf("image/") > -1) {
  //    let img = `<img src="" alt="">`;
  //    img.src = window.URL.createObjectURL(input.files[0]);
  //    dropArea.innerHTML = img;
  //  } else if (input.files[0].type.indexOf("audio/") > -1) {
  //    let audio = document.getElementById("audio");
  //    audio.src = window.URL.createObjectURL(input.files[0]);
  //  } else if (input.files[0].type.indexOf("video/") > -1) {
  //    let video = document.getElementById("video");
  //    video.src = window.URL.createObjectURL(input.files[0]);
  //  }
});
