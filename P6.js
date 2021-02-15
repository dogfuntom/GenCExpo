// p6 is a wrapper around p5 sketch that
// allows constructing and removing the p5 engine multiple times.
// (The name is upper-case to make mixup less likely.)
// The difference from simply constructing/removing the engine:
// - It doesn't start the engine immedietly.
// - Each time it creates a new p5 engine, its put into the same div.

/**
 * @typedef {Object} p5
 * @property {number} width
 * @property {number} height
 * @property {boolean} disableFriendlyErrors
 * @property {function(): void} loop
 * @property {function(): void} noLoop
 * @property {function(): void} remove
 */

/**
 * @callback p5sketch
 * @param {p5} p5
 * @returns {void}
 */

class P6 {
    /**
    * Wraps a sketch into P6 instance without activating it.
    * @constructor
    * @param {p5sketch} sketch - An instance mode p5.js sketch.
    * @param {string} id - A name for the wrapping div.
    * @classdesc See the full description in the source file.
    */
    constructor(sketch, id) {
      this.sketch = sketch;
      this.engine = null;

      this.div = document.createElement("div");
      this.div.id = id;
      document.body.appendChild(this.div);
    }

    /**
    * Create the engine if not already created.
    * Resume playing if paused.
    * Ignore if already playing.
    */
    play() {
      if (!this.engine) {
        this.engine = new p5(this.sketch, this.div);
        this.engine.disableFriendlyErrors = true;
      }
      else {
        this.engine.redraw();
        this.engine.loop();
      }
    }

    /**
    * Pause if playing.
    * Ignore if not playing.
    */
    pause() {
      if (this.engine) {
        this.engine.noLoop();
      }
    }

    /**
    * Remove the engine (if there's one).
    */
    stop() {
      if (this.engine) {
        this.engine.remove();
        this.engine = null;
      }
    }
  }
