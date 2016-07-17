##Texture2LUT
Unity tool to create 3D LUTs using any texture as a palette. *Written/tested with Unity 5.4.0b21; should work on 5.3+*

#Description
Palettes can be literally any texture. The tool creates a LUT which maps colours to the closest colour present in the palette texture.

This doesn't depend on the palette texture being ordered - note that this also means it takes a while as it must scan the entire texture for each output pixel!

#Installation and Use
Place the Texture2LUT folder in Assets. To launch the tool, click Window > Texture2LUT.

Generated textures can be used as a standard 3D LUT.