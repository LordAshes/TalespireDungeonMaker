# TalespireDungeonMaker
Unofficial Talespire utility that converts 2D maps to Talespire 3D slabs

##Background:##

The solution is broken down into two parts. This application written in C# and a second portion for generating the actual Slab Code. Eventually the code for one should be mreged into the code for the other (e.g. either add the slab building code to the C# application or move the C# code into the js portion) but I have not gotten around to doing that yet.

##Setup:##

1. Ensure you have a web server that serves ``D:\wwwroot`` (You can change this location by editing the reference in the code)
2. Ensure you have the content of the ``TalespireDungeonWeb.zip`` extracted to this location. This should make a ``TS`` sub-folder.
Note: Depedning on your computer setup, you may be able to run the necessary file locally without a web server.

##Usage:##

1. Go to: ``https://watabou.itch.io/one-page-dungeon`` and generate a dungeon.
2. Right click the dungeon map and use the ``Export As`` option to export a ``Json`` file.
3. Run this ``TalespireDungeon.exe`` providing the JSON file as the first command line parameter.
4. This will generate a ``layout.js`` and ``map.txt`` in ``D:\wwwroot\ts``.
5. Run the ``Convert.html``. It will show a ASCII version of the map.
6. Press the ``Convert`` button.
7. The slab code will be generated in the text box under the ``Convert`` button.
8. Copy the entire line
9. Start Talespire.
10. Paste (CTRL+V) into an open board.
