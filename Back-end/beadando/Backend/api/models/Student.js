const {Model, DataTypes} = require("sequelize")

module.exports = (sequelize) =>
{
    class Student extends Model {};

    Student.init
    (
        {
            ID: 
            {
                type: DataTypes.INTEGER,
                primaryKey: true,
                autoIncrement: true,
                allowNull: false,
            },
            name: 
            {
                type: DataTypes.STRING,
                allowNull: false,
            },
            birth_date: 
            {
                type: DataTypes.DATE,
                allowNull: false,
            },
            email:
            {
                type: DataTypes.STRING,
                allowNull:false,
                unique: "email",

                validate:
                {
                    isEmail:
                    {
                        args: true,

                        msg: "Invalid email format",
                    }
                },
            },
            major: 
            {
                type: DataTypes.STRING,
                allowNull: false,
            },
        },

        {
            sequelize,
            modelName: "Student",
            freezeTableName: true,
            createdAt: false,
            updatedAt: false,
        },
    );

    return Student;
}