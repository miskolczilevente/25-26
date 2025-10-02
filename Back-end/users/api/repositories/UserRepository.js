const { DbError } = require("../errors");

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
            return await this.User.findAll();
        }
        catch(error)
        {
            throw new DbError("Failed to fetch users", 
            {
                details: error.message,
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