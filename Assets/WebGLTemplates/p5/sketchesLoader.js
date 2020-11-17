const p5Sketches = new Map([['very_nov11', very_nov11], ['MaxKam_nov13', MaxKam_nov13]]);
let p5instances = [];

for (const [key, value] of p5Sketches) {
    let container = document.createElement('div');
    container.id = key;
    document.body.appendChild(container);
    p5instances.push(new p5(value, container));
}
