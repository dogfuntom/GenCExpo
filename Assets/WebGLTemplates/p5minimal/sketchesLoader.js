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

// TODO: Wrap everything into an object to avoid global scope

/**
 * @type {Object.<string, p5sketch>}
 */
const p5sketches = new Map([
    ['very_nov11', very_nov11],
    ['MaxKam_nov13', MaxKam_nov13],
    ['VovDud_nov16', VovDud_nov16],
    ['Victor_favorite', Victor_favorite],
    ['Gle_Trz_nov7', Gle_Trz_nov7],
    ['baku_nov2', baku_nov2],
    ['gvlas_nov8', gvlas_nov8],
    ['quil_oct31', quil_oct31],
    ['shirosayuri_210104', shirosayuri_210104]]);

/**
 * @type {Object.<string, p5>}
 */
let p5instances = new Map();

/**
 * @private
 * @param {Element | string} container
 * @return {p5} p5 instance.
 */
const InitPaused = (key, sketch, container) => {
    /**
     * @type {p5}
     */
    const p5inst = new p5(sketch, container);
    p5inst.noLoop();
    p5inst.disableFriendlyErrors = true;
    p5instances.set(key, p5inst);
    return p5inst;
}

for (const [key, value] of p5sketches) {
    const container = document.createElement('div');
    container.id = key;
    document.body.appendChild(container);

    InitPaused(key, value, container);
}

const Pause = (id) => {
    let p5inst = p5instances.get(id);
    if (p5inst) {
        p5inst.noLoop();
    }
    else {
        p5inst = InitPaused(id, p5sketches.get(id), id);
    }
}

const Play = (id) => {
    let p5inst = p5instances.get(id);
    if (p5inst) {
    }
    else {
        p5inst = InitPaused(id, p5sketches.get(id), id);
    }
    p5inst.loop();
}

const Step = (id) => {
    let p5inst = p5instances.get(id);
    if (p5inst) {
        p5inst.noLoop();
    }
    else {
        p5inst = InitPaused(id, p5sketches.get(id), id);
    }

    p5inst.redraw();
}

const Stop = (id) => {
    p5instances.get(id).remove();
    p5instances.delete(id);
}

const RestartByName = (key) => {
    p5instances.get(key).remove();
    p5instances.set(key, new p5(p5sketches.get(key), key));
}
