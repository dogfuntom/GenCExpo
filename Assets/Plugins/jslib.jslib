mergeInto(LibraryManager.library, {
  $p5canvases: undefined,

  HasP5Instance: function (name) {
    if (typeof p5canvases === 'undefined') {
      p5canvases = new Map();
    }

    name = Pointer_stringify(name);

    if (p5canvases.has(name)) { return true; }

    const p5div = document.getElementById(name);
    if (!p5div) { return false; }

    const p5c = p5div.getElementsByClassName('p5Canvas').item(0);
    if (!p5c) { return false; }

    p5c.style.display = 'none';
    p5canvases.set(name, p5c);
    return true;
  },

  GetP5CanvasTexture: function (name, texture) {
    name = Pointer_stringify(name);

    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    const level = 0;
    const width = 400;
    const height = 400;
    const srcFormat = GLctx.RGBA;
    const srcType = GLctx.UNSIGNED_BYTE;
    GLctx.texSubImage2D(GLctx.TEXTURE_2D, level, 0, 0,
                  width, height, srcFormat, srcType,
                  p5canvases.get(name));
  }
});