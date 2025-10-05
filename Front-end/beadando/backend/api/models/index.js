module.exports = (sequelize) =>
{
    const User = require("./User")(sequelize);

    return { User };
}