let Gle_Trz_nov7 = function ($_p) {
    var startXY = [];
    const vertnum = 80;
    //let theta = 0;

    function rds(t, f) {
      return 100 + 50 * $_p.noise(t * 10, f * 0.01)
    }

    $_p.setup = function () {
      $_p.createCanvas(400, 400);
      $_p.frameRate(30);
    };

    $_p.draw = function () {
      $_p.background(200, 10);
      $_p.noFill();
      $_p.fill(255);
      $_p.translate($_p.width / 2, $_p.height / 2);
      $_p.beginShape();
      for (let theta = 0; theta < $_p.TWO_PI - $_p.TWO_PI / vertnum; theta += $_p.TWO_PI / vertnum) {
        let x = rds(theta, $_p.frameCount) * $_p.cos(theta);
        let y = rds(theta, $_p.frameCount) * $_p.sin(theta);
        if (theta == 0) {
          startXY = [
            x,
            y
          ];
          $_p.curveVertex(x, y)
        }
        $_p.curveVertex(x, y)
      }
      $_p.curveVertex(startXY[0], startXY[1]);
      $_p.curveVertex(startXY[0], startXY[1]);
      $_p.endShape()
    };
  }
