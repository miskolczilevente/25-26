const {Model,DataTypes} = require("sequelize")

module.exports = (sequelize) =>
{
    class Course extends Model {};

    Course.init(
        {
            ID: 
            {
                type: DataTypes.INTEGER,
                primaryKey: true,
                autoIncrement: true,
                allowNull: false,
            },
            course_name: 
            {
                type: DataTypes.STRING,
                allowNull: false,
            },
            credits: 
            {
                type: DataTypes.INTEGER,
                allowNull: false,
            },
        },
        {
            sequelize,
            modelName: "Course",
            freezeTableName: true,
            createdAt: false,
            updatedAt: false,
        },
    );

    return Course;
}