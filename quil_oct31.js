let quil_oct31 = (sketch) => {
    sketch.setup = () => {
      sketch.createCanvas(400, 400);
      prevX = sketch.width / 2;
      prevY = sketch.height / 2;
      sketch.frameRate(15);
    };

    sketch.draw = () => {
      nextX = sketch.random(sketch.width);
      nextY = sketch.random(sketch.height);
      sketch.background(255, 15);
      sketch.stroke(0);
      sketch.line(prevX, prevY, nextX, nextY);
      prevX = nextX;
      prevY = nextY;
    };
  }
