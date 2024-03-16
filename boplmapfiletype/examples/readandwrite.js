const fs = require('fs');

class BoplMap {
    constructor(version, projectName, mapName, developer, platforms) {
        this.version = version;
        this.projectName = projectName;
        this.mapName = mapName;
        this.developer = developer;
        this.platforms = platforms;
    }

    toJSON() {
        return JSON.stringify(this);
    }

    static fromJSON(jsonStr) {
        const data = JSON.parse(jsonStr);
        return new BoplMap(data.version, data.projectName, data.mapName, data.developer, data.platforms);
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

module.exports = { BoplMap, writeBoplMap, readBoplMap };
