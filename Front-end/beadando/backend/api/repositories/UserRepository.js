const { DbError } = require("../errors");

const { Op } = require("sequelize");

class UserRepository
{
    constructor(db)
    {
        this.User = db.User;

        this.sequelize = db.sequelize;
    }

    async getUsers()
    {
        try
        {
            return await this.User.findAll(
            {
                attributes: [ "name", "email", "registeredAt" ],
            });
        }
        catch(error)
        {
            throw new DbError("Failed to fetch users", 
            {
                details: error.message,
            });
        }
    }

    async getUser(userID)
    {
        try
        {
            return await this.User.findOne(
            {
                where:
                {
                    [Op.or]: [ { ID: userID }, { name: userID }, { email: userID } ],
                    // WHERE ID = userID OR name = userID OR email = userID

                    /* ID:
                    {
                        [Op.or]: [5, 6]
                    } // WHERE ID = 5 OR ID = 6 */
                }
            });
        }
        catch(error)
        {
            throw new DbError("Failed to fetch user", 
            {
                details: error.message,
                data: userID,
            });
        }
    }

    async createUser(userData)
    {
        try
        {
            return await this.User.create(userData);
        }
        catch(error)
        {
            throw new DbError("Failed to create user object", 
            {
               details: error.message,
               data: userData, 
            });
        }
    }
}

module.exports = UserRepository;