let very_nov11 = (sketch) => {

    let x;
    let y;
    let r; // радиус
    let a; // угол от 0 до 2Пи

    sketch.setup = () => {
        sketch.createCanvas(400, 400);
        sketch.frameRate(30);
    };

    sketch.draw = () => {
        sketch.background(220);
        sketch.translate(sketch.width / 2, sketch.height / 2);
        sketch.noFill();
        sketch.rotate(-sketch.frameCount / 2);

        sketch.beginShape();
        for (a = 0; a <= sketch.TWO_PI * 50; a += sketch.TWO_PI / 50) {
            r = 10;
            x = sketch.cos(a) * r * a / 5;
            y = sketch.sin(a) * r * a / 5;
            sketch.vertex(x, y);
        }
        sketch.endShape();
    };
};
