mergeInto(LibraryManager.library, {
  $p5canvases: undefined,

  /**
   * Tries to resume or launch p5 sketch.
   *
   * @param {string} name ID of the p5 sketch.
   * @return {boolean} Whether the p5 sketch is playing now (launched, resumed or already was playing).
   */
  PlayP5: function (name) {
    if (typeof p5canvases === 'undefined') {
      p5canvases = new Map();
    }

    name = Pointer_stringify(name);

    if (p5canvases.has(name)) {
      Play(name);
      return true;
    }

    const p5div = document.getElementById(name);
    if (!p5div) { return false; }

    const p5c = p5div.getElementsByClassName('p5Canvas').item(0);
    if (!p5c) { return false; }

    p5c.style.display = 'none';
    p5canvases.set(name, p5c);

    Play(name);
    return true;
  },

  GetP5Width: function (name) {
    name = Pointer_stringify(name);
    return p5canvases.get(name).width;
  },

  GetP5Height: function (name) {
    name = Pointer_stringify(name);
    return p5canvases.get(name).height;
  },

  GetP5Texture: function (name, texture) {
    name = Pointer_stringify(name);

    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    const level = 0;

    const canvas = p5canvases.get(name);
    const width = canvas.width;
    const height = canvas.height;
    const srcFormat = GLctx.RGBA;
    const srcType = GLctx.UNSIGNED_BYTE;
    GLctx.pixelStorei(GLctx.UNPACK_FLIP_Y_WEBGL, true);
    GLctx.texSubImage2D(GLctx.TEXTURE_2D, level, 0, 0,
                  width, height, srcFormat, srcType,
                  canvas);
  },

  PauseP5: function (name) {
    name = Pointer_stringify(name);
    Pause(name);
  },

  RecreateP5: function (name) {
    name = Pointer_stringify(name);
    Stop(name);
    Play(name);
    p5canvases.delete(name);
  }
});