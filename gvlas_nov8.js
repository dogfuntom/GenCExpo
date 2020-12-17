let gvlas_nov8 = (sketch) => {
    let x;
    let y;
    let r; // радиус
    let a; // угол

    sketch.setup = () => {
      sketch.createCanvas(400, 400);
      sketch.frameRate(15);
    };

    sketch.draw = () => {
      sketch.background(255);
      sketch.noFill();

      for (let i = 1; i < 8; i += 0.5) {
        sketch.beginShape();
        for (a = 0; a < 2 * sketch.PI; a += 0.1) {
          r = sketch.noise(a, sketch.frameCount / 50) * 100
          x = sketch.cos(a) * r;
          y = sketch.sin(a) * r;
          sketch.vertex(
            sketch.width / 2 + x * i,
            sketch.height / 2 + y * i);
        }
        sketch.endShape(sketch.CLOSE);
      }
    };
  }
