const bcrypt = require("bcrypt");

const db = require("../db");

const { userService } = require("../services")(db);

const jwt = require("jsonwebtoken");

exports.login = async (req, res, next) =>
{
    const { userID, password } = req.body;

    let user;

    try
    {
        user = await userService.getUser(userID);
    }
    catch(error)
    {
        return next(error);
    }
    
    if(bcrypt.compareSync(password, user.password))
    {
        const token = jwt.sign({ userID: user.ID, username: user.name }, process.env.JWT_SECRET);
        
        res.status(200).json(token);
    }
    else
    {
        res.status(401).json({ message: "Wrong password" });
    }
}

exports.status = (req, res, next) =>
{
    if(req.session.username) res.status(200).json({ username: req.session.username, email: req.session.email });

    res.sendStatus(404);
}

exports.logout = (req, res, next) =>
{
    if(req.session && req.session.username) req.session.destroy();

    res.status(200).json({ message: "Logged out successfully" });
}