# Boplmap Mod

## Overview
The Boplmap Mod is a custom modification for [insert game name] that introduces support for loading custom map configurations from `.boplmap` files. These files contain detailed information about map elements such as platforms, their positions, rotations, biomes, and types.

## Features
- Load custom maps from `.boplmap` files.
- Define map elements including platforms with specific properties like position, rotation, biome, and type.
- Support for creating diverse and intricate custom maps.

## Getting Started
To use the Boplmap Mod in your game, follow these steps:

1. **Installation:**
   - Download the latest version of the mod from the releases section.
   - Install BepInEx and Splatch if you haven't already.

2. **Usage:**
   - Place your `.boplmap` files in the designated maps folder.
   - Launch the game and select the Boplmap Mod from the mod menu.
   - Load your custom map and start playing!

3. **Creating Custom Maps:**
   - Use the provided `.boplmap` file example as a template.
   - Define your map elements including platforms, their positions, rotations, biomes, and types.
   - Save the file and place it in the maps folder.

4. **Feedback:**
   - We welcome any feedback or suggestions for improving the mod. Feel free to reach out to us through the mod's community forums or GitHub repository.

## Example `.boplmap` File
```json
{
  "version": "1.0",
  "projectName": "Boplmap",
  "mapName": "Boplmap",
  "developer": "Abstractmelon",
  "platforms": [
    {
      "transform": { "x": 10, "y": 20 },
      "rotation": 0,
      "biome": "Plain",
      "type": "Ball",
      "blank": false
    },
    {
      "transform": { "x": 30, "y": 40 },
      "rotation": 90,
      "biome": "Snow",
      "type": "Long",
      "blank": true
    }
  ]
}
## Compatibility
The Boplmap Mod is compatible with the following game versions:

- Bopl Battle 2.1.3
- Splotch 0.2.0, 0.2.1, 0.3.0, 0.4.0
- BepInEx 5.4.22

Ensure that you are using the appropriate version of BepInEx and Splatch for your game.

## Credits
- MapMakerSite & .boplmap Developed by Abstractmelon.
- CustomMapLoader Developed by DavidLovesJellycarWorlds
- Special thanks to Unluckycraft for their support and contributions.

## License
This mod is released under the GNU GENERAL PUBLIC LICENSE v3.0 license. See the `LICENSE` file for more information.
