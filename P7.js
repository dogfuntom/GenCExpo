/**
 * @typedef {Object} p5
 */

/**
 * @callback p5sketch
 * @param {p5} - p5 engine
 * @returns {void}
 */

class P7 {
    /**
    * Creates a gallery of p5.js sketches.
    * Each p5 sketch is wrapped in a P6 instance.
    * @example
    * new P7({ foo, bar, baz});
    * @param {Object.<string, p5sketch>|Object.<string, P6>|Map} dict - A dictionary where the key is ID and the value is either a raw p5 sketch or a P6 instance.
    */
    constructor(dict) {
      this.p6s = new Map();
      if (dict instanceof Map) {
        for (let [id, sketch] of dict) {
          if (sketch instanceof P6) {
            this.p6s.set(id, sketch);
          } else {
            this.p6s.set(id, new P6(sketch, id));
          }
        }
      } else {
        // The idea is to avoid typing the same thing twice: ['sketch', sketch].
        // Seems like it can't be avoided with Map but certainly can be with Object.
        for (const id in dict) {
          this.p6s.set(id, new P6(dict[id], id));
        }
      }
    }

    pause(id) {
      let p6 = this.p6s.get(id);
      p6.pause();
    }

    play(id) {
      let p6 = this.p6s.get(id);
      p6.play();
    }

    step(id) {
      let p6 = this.p6s.get(id);
      p6.play();
      p6.pause();
    }

    stop(id) {
      let p6 = this.p6s.get(id);
      p6.stop();
    }

    restart(id) {
      let p6 = this.p6s.get(id);
      p6.stop();
      p6.play();
    }
  }
