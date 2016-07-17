#Texture2LUT
Unity tool to create 3D LUTs using any texture as a palette. *Written/tested with Unity 5.4.0b21; should work on 5.3+*

##Description
Palettes can be literally any texture. The tool creates a LUT which maps colours to the closest colour present in the palette texture.

This doesn't depend on the palette texture being ordered - note that this also means it takes a while as it must scan the entire texture for each output pixel!

##Installation and Use
Place the Texture2LUT folder in Assets. To launch the tool, click Window > Texture2LUT.

Generated textures can be used as a standard 3D LUT.

##Examples
The [Quake palette](https://quakewiki.org/wiki/Quake_palette) converted to a LUT:
![Quake Palette](http://www.sigtrapgames.com/wp-content/uploads/2016/07/Qpalette.png)  ![Quake LUT](http://www.sigtrapgames.com/wp-content/uploads/2016/07/QLUT.png)

A screen of our game [Sublevel Zero](http://store.steampowered.com/app/327880/) converted to a LUT:
![SL0 Screenshot](http://www.sigtrapgames.com/wp-content/uploads/2016/07/SL0_PaletteTest.png)  ![SL0 LUT](http://www.sigtrapgames.com/wp-content/uploads/2016/07/SL0_LUT.png)