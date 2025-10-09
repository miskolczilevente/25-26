const {Model, DataTypes} = require("sequelize")

module.exports = (sequelize) =>
{
    class Enrollment extends Model {};

    Enrollment.init
    (
        {
            ID:
            {
                type: DataTypes.INTEGER,
                primaryKey: true,
                autoIncrement: true,
                allowNull: false,
            },
            grade: 
            {
                type: DataTypes.INTEGER,
                allowNull: false,
            },
        },

        {
            sequelize,
            modelName: "Enrollment",
            freezeTableName: true,
            createdAt: "enrollment_date",
            updatedAt: false,
        },
    );

    return Enrollment;
}