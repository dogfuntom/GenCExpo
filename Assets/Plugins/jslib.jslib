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

  InitInputForm: function () {
    input = document.createElement('input');
    input.value = '42';
    input.id = 'input';

    document.body.appendChild(input);
  },

  GetInputText: function() {
    var returnStr = input.value;
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  BindWebGLTexture2: function (texture) {
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    const level = 0;
    const width = 1;
    const height = 1;
    const srcFormat = GLctx.RGBA;
    const srcType = GLctx.UNSIGNED_BYTE;
    const pixel = new Uint8Array([0, 0, 255, 255]);  // opaque blue
    GLctx.texSubImage2D(GLctx.TEXTURE_2D, level, 0, 0,
                  width, height, srcFormat, srcType,
                  pixel);
  },

  BindWebGLTexture3: function (texture) {
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
    const level = 0;
    const width = 400;
    const height = 400;
    const srcFormat = GLctx.RGBA;
    const srcType = GLctx.UNSIGNED_BYTE;
    //const pixel = new Uint8Array([0, 0, 255, 255]);  // opaque blue
    GLctx.texSubImage2D(GLctx.TEXTURE_2D, level, 0, 0,
                  width, height, srcFormat, srcType,
                  p5c);
  }
});