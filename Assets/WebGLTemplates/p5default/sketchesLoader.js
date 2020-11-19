const p5sketches = new Map([
    ['very_nov11', very_nov11],
    ['MaxKam_nov13', MaxKam_nov13],
    ['VovDud_nov16', VovDud_nov16],
    ['Victor_favorite', Victor_favorite]]);

let p5instances = new Map();

for (const [key, value] of p5sketches) {
    let container = document.createElement('div');
    container.id = key;
    document.body.appendChild(container);
    p5instances.set(key, new p5(value, container));
}

const RestartByName = (key) => {
    p5instances.get(key).remove();
    p5instances.set(key, new p5(p5sketches.get(key), key));
}
