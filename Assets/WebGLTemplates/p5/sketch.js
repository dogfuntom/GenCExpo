var pg;
var ctx;

function setup() {
  const c = createCanvas(500, 500, WEBGL);
  c.elt.style.border = '2px solid purple';

  pg = createGraphics(400, 400);
  pg.background(200);
  ctx = pg.elt.getContext('2d');
}

function draw() {
  const f = frameCount / 200;
  pg.point(400 * noise(f), 400 * noise(f + 1));
  texture(pg);

  background(100);
  rotateX(frameCount * 0.01);
  rotateY(frameCount * 0.01);
  box(200);
}