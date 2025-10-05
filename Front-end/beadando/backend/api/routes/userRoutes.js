const express = require("express");

const router = express.Router();

const userController = require("../controllers/userController");

router.get("/", userController.getUsers);

router.post("/", userController.createUser);

router.param("userID", (req, res, next, userID) => 
{
    req.userID = userID;

    next();
});

router.get("/:userID", userController.getUser);

module.exports = router;