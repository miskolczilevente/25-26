const { BadRequestError } = require("../errors");

class UserService
{
    constructor(db)
    {
        this.userRepository = require("../repositories")(db).userRepository;
    }

    async getUsers()
    {
        return await this.userRepository.getUsers();
    }

    async createUser(userData)
    {
        if(!userData) throw new BadRequestError("Missing user data from payload", 
        {
            data: userData,
        });

        if(!userData.name) throw new BadRequestError("Missing username from payload",
        {
            data: userData,
        });

        if(!userData.password) throw new BadRequestError("Missing password from payload", 
        {
            data: userData,
        });

        return await this.userRepository.createUser(userData);
    }
}

module.exports = UserService;