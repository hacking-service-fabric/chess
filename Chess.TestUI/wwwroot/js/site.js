function sendUserInput(id, text, files) {
    const data = new FormData();
    if (files && files.length > 0) {
        data.append(files[0]);
    }
    data.set("text", text);

    fetch('/conversation',
        {
            method: "POST",
            body: data
        });

    createSpeechBubble(`user${id}Speech`, text);
}

setInterval(async () => {
    const request = await fetch('/conversation');
    const json = await request.json();
    for (const e of json) {
        createSpeechBubble("server", e.text, e.content);
    }
}, 2500);

function createSpeechBubble(id, text, imageSrc) {
    const speeches = document.getElementById('conversation').children,
        currentSpeech = document.getElementById(id);

    const speech = document.createElement('div');
    speech.className = "speechBubble";
    currentSpeech.appendChild(speech);
    if (imageSrc) {
        const image = new Image(250);
        image.src = imageSrc;
        speech.appendChild(image);
    }
    const textContent = document.createElement('div');
    textContent.textContent = text;
    speech.appendChild(textContent);

    const space = document.createElement('div');
    space.style.height = `${speech.offsetHeight}px`;
    for (const speech of speeches) {
        if (speech !== currentSpeech) {
            const spaceClone = space.cloneNode();
            speech.appendChild(spaceClone);
        }
    }
}
