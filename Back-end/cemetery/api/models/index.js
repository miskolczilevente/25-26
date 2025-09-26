const { DataTypes } = require("sequelize");

module.exports = (sequelize) =>
{
    const Person = require("./person")(sequelize, DataTypes);

    return { Person };
}