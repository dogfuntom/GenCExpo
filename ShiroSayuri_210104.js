let shirosayuri_210104 = (s) => {
  let particles = [];
  let particlesNumber = 1;

  s.setup = () => {
    s.createCanvas(700, 700);
    s.fill(0)
    s.noStroke()
    s.background("#14192C");

    for (let i = 0; i < particlesNumber; i++) {
      let particle = {
        pos: s.createVector(s.width / 2, s.height),
        v: s.createVector(0, -.5),
        r: 10,
        type: 'stem',
        countDown: 300
      }
      particles.push(particle)
    }
  };

  s.draw = () => {

    particlesNumber = particles.length
    for (let i = 0; i < particlesNumber; i++) {

      r = s.map(particles[i].pos.x, 0, s.width, 255, 0)
      g = s.map(particles[i].pos.y, 0, s.height, 0, 255)
      b = s.map(s.dist(s.width / 2, s.height / 2, particles[i].pos.x, particles[i].pos.y), 0, s.width, 0, 255)

      s.circle(particles[i].pos.x, particles[i].pos.y, particles[i].r)
      s.fill(r, g, b)

      particles[i].pos.add(particles[i].v)
      //replication
      if (particles[i].type == 'stem' &&
        s.random() < 0.4 / particles[i].r &&
        particles.length < 100 &&
        particles[i].r > 1) {
        let particle = {
          pos: particles[i].pos.copy(),
          v: particles[i].v.copy().rotate(s.PI / 9 * s.random([1, -1])),
          r: particles[i].r * .5,
          type: s.random() < .4 ? 'stem' : 'branch',
          countDown: 80 * particles[i].r
        }
        if (particle.type == 'branch') {
          particle.countDown = 350 //*random()
          particle.pos = particles[i].pos.copy(),
            particle.v = particles[i].v.copy().rotate(s.PI / 9 * s.random([1, -1])),
            particle.r = particles[i].r * .5,
            particle.countDown = 50 * particles[i].r
          particle.type = s.random() < .3 ? 'branch' : 'leaf'
        }
        particles.push(particle)
      }

      if (particles[i].type == 'leaf') {
        particles[i].r = 1
        particles[i].countDown = 150 * s.random()
        let v = s.createVector(0, 0.5)
        let step = 0.01
        let amount = 0
        if (amount > 1 || amount < 0) {
          step *= -1;
        }
        amount += step;
        particles[i].v = p5.Vector.lerp(particles[i].v, v, amount);
      }

      particles[i].countDown--


      //death
      if (particles[i].countDown < 0) {
        particles.splice(i, 1)
        particlesNumber--
      }

    }
  };
};
