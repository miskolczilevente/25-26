const {Model, DataTypes} = require("sequelize")

module.exports = (sequelize) => 
{
    class Schedule extends Model {};

    Schedule.init
    (
        {
            ID: 
            {
                type: DataTypes.INTEGER,
                primaryKey: true,
                autoIncrement: true,
                allowNull: false,
            },
            day:
            {
                type: DataTypes.STRING,
                allowNull: false,
            },
            start_time:
            {
                type: DataTypes.TIME,
                allowNull: false,
            },
            end_time:
            {
                type: DataTypes.TIME,
                allowNull: false,
            },
        },
        {
            sequelize,
            modelName: "Schedule",
            freezeTableName: true,
            createdAt: false,
            updatedAt: false,
        },
    );

    return Schedule;
}