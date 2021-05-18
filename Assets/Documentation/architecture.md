Architecture
===

There are 4 segments:

1. HTML+CSS+JS side, located in WebGLTemplates folder (the name is special).
2. Mediator on JS side: Plugins/jslib.jslib (folder name is special, the file can be renamed).
3. Mediator on C# side: code in Code/P5 that uses P/Invoke-like syntax to connect with the one above.
4. The rest of C# side.

Goals
---

- Do not update (copy) textures when not visible (the most costly operation so far, 10 updating textures are already laggy).
- Do not use frustum culling without occlusion culling, even for "secondary" optimization. (Because it's a useless added complexity on its own: standing on the tip of the main "balcony" is already enough to "see" everything inside the frustum simultaneously.)
- Ignore the rest of performance concerns for now, even if they were previously handled already (because I tried to do otherwise, it didn't work).

The problem is Occlusion Culling API in Unity is not very good. It doesn't call OnBecameVisible() or OnBecameInvisible(). It has no events. The only simple way is using OnWillRender() and setting timeout for when it wasn't called recently.

Less simple way is customizing it heavily or using another OC solution entirely.

Notes
---

Most calls to JS side can work synchronously, and should. The "thread blocking" is negligible, not enough to justify multiplying complexity of already complex integration by adding async logic to it.
