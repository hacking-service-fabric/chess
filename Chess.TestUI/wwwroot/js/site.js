function sendUserInput(id, text) {
    let speeches = document.getElementById('conversation').children,
        userSpeech = document.getElementById(`user${id}Speech`);

    let speech = document.createElement('div');
    speech.className = "speechBubble";
    speech.textContent = text;
    userSpeech.appendChild(speech);

    let space = document.createElement('div');
    space.style.height = `${speech.offsetHeight}px`;
    for (let speech of speeches) {
        if (speech !== userSpeech) {
            let spaceClone = space.cloneNode();
            speech.appendChild(spaceClone);
        }
    }
}
