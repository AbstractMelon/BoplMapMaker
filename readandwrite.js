const fs = require('fs');

class BoplMap {
    constructor(version, projectName, mapName, developer, blocks) {
        this.version = version;
        this.projectName = projectName;
        this.mapName = mapName;
        this.developer = developer;
        this.blocks = blocks;
    }

    toJSON() {
        return JSON.stringify(this);
    }

    static fromJSON(jsonStr) {
        const data = JSON.parse(jsonStr);
        return new BoplMap(data.version, data.projectName, data.mapName, data.developer, data.blocks);
    }
}

function writeBoplMap(filename, boplMap) {
    const jsonStr = boplMap.toJSON();
    fs.writeFileSync(filename, jsonStr);
}

function readBoplMap(filename) {
    const jsonStr = fs.readFileSync(filename, 'utf8');
    return BoplMap.fromJSON(jsonStr);
}

// Example usage:
const blocksData = [
    { id: 1, type: 'wall' },
    { id: 2, type: 'door' },
    { id: 3, type: 'window' }
];

const myMap = new BoplMap(
    '1.0',
    'MyProject',
    'MyMap',
    'John Doe',
    blocksData
);

writeBoplMap('mymap.boplmap', myMap);

const readMap = readBoplMap('mymap.boplmap');
console.log(readMap.toJSON());
