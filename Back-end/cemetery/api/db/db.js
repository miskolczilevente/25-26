const { Sequelize } = require("sequelize");

const sequelize = new Sequelize
(
    process.env.DB_NAME,
    process.env.DB_USER,
    process.env.DB_PASSWORD,

    {
        host: process.env.DB_HOST,
        dialect: process.env.DB_DIALECT
    }
);

(async () =>                // EZT A RÉSZT (15. sortól - 27.-sorig nem kell tudni)
{
    try
    {
        await sequelize.authenticate();

        console.log("Database connection successful");
    }
    catch(error)
    {
        console.log("Database connection failed");
    }
})();

