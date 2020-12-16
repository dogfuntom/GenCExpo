let baku_nov2 = (sketch) => {
    sketch.setup = () => {
      sketch.createCanvas(400, 400)
      sketch.background(220)
      sketch.frameRate(30)
    };

    sketch.draw = () => {
      sketch.noStroke();
      sketch.fill(0);
      sketch.circle(
        sketch.noise(sketch.frameCount * 0.005) * sketch.width,                 sketch.noise(sketch.frameCount / 100) * sketch.width, 5)
    };
  };
