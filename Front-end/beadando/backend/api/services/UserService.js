const { where } = require("sequelize");
const { BadRequestError, NotFoundError } = require("../errors");

const bcrypt = require("bcrypt");

const salt = 14;

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

    async getUser(userID)
    {
        if(!userID) throw new BadRequestError("Missing user identification from payload");

        const user = await this.userRepository.getUser(userID);

        if(!user) throw new NotFoundError("Can not found user with this user identification", 
        {
            data: userID
        });

        return user;
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

        userData.password = bcrypt.hashSync(userData.password, salt);

        if(!userData.email) throw new BadRequestError("Missing email from payload", 
        {
            data: userData,
        });

        return await this.userRepository.createUser(userData);
    }

    async deleteUser(userID)
    {
        if(!userID) throw new BadRequestError("Missing user identification from payload");

        const user = await this.userRepository.deleteUser(userID);

        if(!user) throw new NotFoundError("Can not found user with this user identification", 
        {
            data: userID
        });

        return user;
    }

    async updateUser(userData, userID) 
    {
        if(!userID) throw new BadRequestError("Missing user identification from payload", 
        {
            data: userID
        });

        if(!userData) throw new BadRequestError("Missing user data from payload", 
        {
            data: userData,
        });

        if(!userData.name) throw new BadRequestError("Missing username from payload",
        {
            data: userData,
        });

        if(!userData.email) throw new BadRequestError("Missing email from payload", 
        {
            data: userData,
        });

        return await this.userRepository.updateUser(userID, userData);
    }
}

module.exports = UserService;