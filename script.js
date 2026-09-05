// JavaScript code to create a simple and automatic box movement animation
const box = document.getElementById('animate-box');
let position = 0;
let direction = 1;

function move() {
    // Moves the box back and forth by 50 pixels
    if (position >= 50) direction = -1;
    if (position <= -50) direction = 1;
    
    position += direction * 2;
    box.style.transform = translateX(${position}px);
    
    requestAnimationFrame(move);
}

// Starts the animation as soon as the page loads
move();