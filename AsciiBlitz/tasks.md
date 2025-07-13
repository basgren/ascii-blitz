# Tasks

* Enhance rendering:
   * implement TileLayer for rendering tile-based map
   * implement Object layer - which will render objects in it.
   * for simplicity implement fixed number of layers for different purposes:
     * ground: tiles
     * obstacles: tiles layer - special layer which will 
     * units: object layer
     * later - effects layer (object)
   * optimize rendering:
     * add char and color buffer
     * draw per-lines (build lines using string buffer), call as few
       `Console.Write` and `Console.SetCursorPosition` as possible
* implement shooting
* implement object collision detection.
* implement smooth tank movement
* implement different mechanics of movement (rotate + front-back)
* implement 