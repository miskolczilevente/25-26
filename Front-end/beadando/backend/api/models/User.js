const { Model, DataTypes } = require("sequelize");

module.exports = (sequelize) =>
{
    class User extends Model {};

    User.init
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
                unique: "username"
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

            password:
            {
                type: DataTypes.STRING,
                allowNull: false,
            },

            isAdmin:
            {
                type: DataTypes.BOOLEAN,
                allowNull: false,
                defaultValue: false,
            }
        },

        {
            sequelize,
            modelName: "User",
            createdAt: "registeredAt",
            updatedAt: false,
        },
    );

    return User;
}