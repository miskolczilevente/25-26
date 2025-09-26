const { Sequelize } = require("sequelize");

const sequelize = new Sequelize
(
    process.env.DB_NAME,
    process.env.DB_USER,
    process.env.DB_PASSWORD,

    {
        logging: false,
        host: process.env.DB_HOST,
        dialect: process.env.DB_DIALECT,
    }
);

(async () => 
{
    try
    {
        await sequelize.authenticate();

        console.log("Database connection OK");
    }
    catch(error)
    {
        console.log("Database connection failed");
    }
})();

const models = require("../models")(sequelize);

const db =
{
    sequelize,
    Sequelize,
    ...models,
};

(async () => 
{
    try
    {
        await db.sequelize.sync({ force: true });

        console.log("✅ Database sync OK");
    }
    catch(error)
    {
        console.log("Failed to sync database");
    }
})();

module.exports = db;