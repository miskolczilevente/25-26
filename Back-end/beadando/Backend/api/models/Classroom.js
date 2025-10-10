const {Model, DataTypes} = require("sequelize");

module.exports = (sequelize) =>
{
    class Classroom extends Model {};

    Classroom.init
    (
        {
            ID:
            {
                type: DataTypes.INTEGER,
                primaryKey: true,
                autoIncrement: true,
                allowNull: false,
            },
            building:
            {
                type: DataTypes.STRING,
                allowNull: false,
            },
            room_number:
            {
                type: DataTypes.STRING,
                allowNull: false,
            },
            capacity:
            {
                type: DataTypes.INTEGER,
                allowNull: false,
            },
        },
        {
            sequelize,
            modelName: "Classroom",
            freezeTableName: true,
            createdAt: false,
            updatedAt: false,
        },
    );

    return Classroom;
}