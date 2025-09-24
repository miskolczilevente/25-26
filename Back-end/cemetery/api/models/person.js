const { Model } = require("sequelize");

module.exports = (sequelize, DataTypes) =>
{
    class Person extends Model {};

    Person.init
    (
        {
            ID:
            {
                type: DataTypes.INTEGER,
                primaryKey: true,
                autoIncrement: true,
            },

            last_name:
            {
                type: DataTypes.STRING,
            },

            first_name:
            {
                type: DataTypes.STRING(50),
            }
        },

        {
            sequelize,
            modelName: "Person",
            freezeTableName: true,
            updatedAt: false,
            createdAt: "diedAt",
        }
    );

    return Person;
}