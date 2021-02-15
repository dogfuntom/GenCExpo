mergeInto(LibraryManager.library, {
  //$p7: undefined,

  /**
   * Tries to resume or launch p5 sketch.
   *
   * @param {string} name ID of the p5 sketch.
   * @return {boolean} Whether the p5 sketch is playing now (launched, resumed or already was playing).
   */
  PlayP5: function (name) {
    name = Pointer_stringify(name);
    if (!p7.p6s.has(name)) return false;

    p7.play(name);
    p7.p6s.get(name).div.style.display = 'none';
    return true;
  },

  GetP5Width: function (name) {
    name = Pointer_stringify(name);
    return p7.p6s.get(name).div.getElementsByClassName('p5Canvas').item(0).width;
  },

  GetP5Height: function (name) {
    name = Pointer_stringify(name);
    return p7.p6s.get(name).div.getElementsByClassName('p5Canvas').item(0).height;
  },

  GetP5Texture: function (name, texture) {
    name = Pointer_stringify(name);

    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    const level = 0;

    const canvas = p7.p6s.get(name).div.getElementsByClassName('p5Canvas').item(0);
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
    p7.pause(name);
  },

  RecreateP5: function (name) {
    name = Pointer_stringify(name);
    p7.restart(name);
  },

  StopP5: function (name) {
    name = Pointer_stringify(name);
    p7.stop(name);
  }
});