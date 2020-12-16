let VovDud_nov16 = (sketch) => {

    sketch.setup = () => {
        sketch.createCanvas(400, 400);
        sketch.frameRate(30);
    };

    sketch.draw = () => {
        sketch.background(0);
        var density = sketch.map(sketch.frameCount * 4, 0, sketch.width, 20, 50)

        var count = 0.2;
        for (var x = 50; x <= sketch.width - 50; x += density) {
            for (var y = 50; y <= sketch.height - 50; y += density) {
                sketch.stroke(255)
                sketch.fill(0)
                sketch.strokeWeight(count * 0.05)
                sketch.ellipse(x, y, 20, 20);
                count++;
            }
        }
    };
};
