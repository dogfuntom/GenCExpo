mergeInto(LibraryManager.library, {
  $input: undefined,
  $p5c: undefined,

  HasP5Canvas: function () {
    p5c = document.getElementById('p5SketchCanvas');
    if (p5c) {
      p5c.style.display = 'none';
      return true;
    }
    return false;
  },

  GetP5CanvasTexture: function (texture) {
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    const level = 0;
    const width = 400;
    const height = 400;
    const srcFormat = GLctx.RGBA;
    const srcType = GLctx.UNSIGNED_BYTE;
    GLctx.texSubImage2D(GLctx.TEXTURE_2D, level, 0, 0,
                  width, height, srcFormat, srcType,
                  p5c);
  }
});