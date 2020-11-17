let MaxKam_nov13 = (sketch) => {

    let g;

    sketch.setup = () => {
        sketch.createCanvas(400, 400);
        g = sketch.createGraphics(300, 400);
        sketch.frameRate(1);
    };

    sketch.draw = () => {
        g.background('antiquewhite');
        g.fill(0);
        g.circle(
            g.width/2,
            g.height/2,
            // A bug in p5.js: when graphics' circle() is called in instance mode,
            // the 3rd parameter is suddenly radius instead of diameter.
            g.width * 0.4 * 2);

        sketch.translate(sketch.width/2, sketch.height/2);

        sketch.push();
        sketch.scale(2, 1.5);
        sketch.image(g, -g.width/2, -g.height/2);
        sketch.pop();

        sketch.image(g, -g.width/2, -g.height/2);

        sketch.push();
        sketch.scale(0.0625, 0.0625);
        sketch.translate(g.width, g.height);
        sketch.image(g, -g.width/2, -g.height/2);
        sketch.pop();
    };
};
