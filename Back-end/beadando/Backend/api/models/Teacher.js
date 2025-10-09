const {Model, DataTypes} = require("sequelize")

module.exports = (sequelize) => 
{
    class Teacher extends Model {};

    Teacher.init
    (
        {
            ID:
            {
                type: DataTypes.INTEGER,
                primaryKey: true,
                autoIncrement: true,
                allowNull: true,
            },
            name: 
            {
                type: DataTypes.STRING,
                allowNull: true,
            },
            department: 
            {
                type: DataTypes.STRING,
                allowNull: false,
            },
            email: 
            {
                type: DataTypes.STRING,
                allowNull: false,
                unique: "email",

                validate:
                {
                    isEmail:
                    {
                        args: true,

                        msg: "Invalid email format",
                    }
                }
            },
            position: 
            {
                type: DataTypes.STRING,
                allowNull: false,
            },
        },
        {
            sequelize,
            modelName: "Teacher",
            freezeTableName: true,
            createdAt: false,
            updatedAt: false,
        }
    );

    return Teacher;
}