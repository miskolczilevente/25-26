// ORM = Object-Relational-Mapping

const { Sequelize } = require("sequelize");

const sequelize = new Sequelize
(
    process.env.DB_NAME,
    process.env.DB_USER,
    process.env.DB_PASSWORD,

    {
        dialect: process.env.DB_DIALECT,
        host: process.env.DB_HOST,
        logging: false,
    }
);

(async () => 
{
    try
    {
        await sequelize.authenticate();
        console.error("Database connection OK");
    }
    catch(error)
    {
        console.error("Database connection failed");
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
        console.log("Database sync OK!");
    }
    catch(error)
    {
        console.error("Database sync failed");
    }
})();

module.exports = db;