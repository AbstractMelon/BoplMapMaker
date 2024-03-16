# Bopl Battle Map (.BOPLMAP) Format

The `.boplmap` file format is used by the Bopl Battle game to store maps created by the Bopl Map Maker tool. This README provides an overview of the `.boplmap` file format, its structure, and how to work with it.

## Overview

- **File Extension**: `.boplmap`
- **Purpose**: Store maps created for the Bopl Battle game.
- **Format**: JSON-like structure.

## File Structure

A `.boplmap` file consists of the following key components:

- **Version**: Indicates the version of the `.boplmap` format being used.
- **Project Name**: The name of the project or map set.
- **Map Name**: The name of the specific map.
- **Developer**: The creator or developer of the map.
- **Platforms**: An array of platform objects, each representing a platform in the map.

### Platform Object

Each platform object contains the following properties:

- **Location**: Coordinates of the platform on the map.
- **Biome**: The biome the platform is located in.
- **Type**: The type of platform (e.g., Long, Ball, Square).
- **Blank**: Indicates whether the platform is empty or not.

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

```