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
                allowNull: false,
                primaryKey: true,
                autoIncrement: true
            },

            last_name:
            {
                type: DataTypes.STRING,
                allowNull: false,
            },

            first_name:
            {
                type: DataTypes.STRING(30),
                allowNull: false,
            },        
        },

        {
            sequelize,
            modelName: "Person",
            freezeTableName: true,
            timestamps: false,
        }
    );

    return Person;
}