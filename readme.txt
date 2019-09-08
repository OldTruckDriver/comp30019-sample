-Terrain is first attempted to be created by using recursion. However for some reason it is not working, therefore we switched to using traditinoal loop method after several failed attemps.

-Water wave shader is taken from the lab but tweaked for our purpose.

-Terrain and water textures are taken from the web:
	Terrain : http://spiralgraphics.biz/packs/terrain_desert_barren/index.htm?30
	Water : https://www.videoblocks.com/video/blue-pool-water-texture-chh70oo

-We tried to tile the texture but we can't do it for some reason even though wrapping mode is set to repeat and tiling is enabled. Texture wrapping looks like its low resolution.

-Collision is handled using Unity built in mesh collider and box collider. Water and terrain are set to ignore collision.

-Camera is referenced from https://gist.github.com/gunderson/d7f096bd07874f31671306318019d996.

-Sun and moon were implemented successfully without much problem.

-Due to time and manpower constrants (2 people group), we were unable to finish everything on time. Phong shader is not completed even though normal is calculated.