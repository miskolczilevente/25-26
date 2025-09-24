const { DataTypes } = require("sequelize");

module.exports = (sequelize) =>
{
    const Person = require("./Person")(sequelize, DataTypes);

    return [ Person ];
}