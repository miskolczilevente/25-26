const { use } = require("react");
const db = require("../db");

const { userService } = require("../services")(db);

exports.getUsers = async (req, res, next) =>
{
    try
    {
        res.status(200).json(await userService.getUsers());
    }
    catch(error)
    {
        next(error);
    }
}

exports.getUser = async (req, res, next) =>
{
    const userID = req.userID;

    try
    {
        res.status(200).json(await userService.getUser(userID));
    }
    catch(error)
    {
        next(error);
    }
}

exports.createUser = async (req, res, next) =>
{
    const { username, email, password } = req.body || {};

    try
    {
        res.status(201).json(await userService.createUser({ name: username, email, password }));
    }
    catch(error)
    {
        next(error);
    }
}

exports.deleteUser = async (req, res, next) =>
{
    const userID = req.userID;

    try
    {
        res.status(200).json(await userService.deleteUser(userID));
    }
    catch(error)
    {
        next(error);
    }
}

exports.updateUser = async (req, res, next) =>
{
    const userID = req.userID;
    
    console.log("req.userID:", req.userID); 

    const {username, email} = req.body || {};

    try {
        res.status(200).json(await userService.updateUser({name: username, email}, userID))    
    }
    catch (error) 
    {
        next(error)
    }
}