const express = require("express");

const router = express.Router();

const authController = require("../controllers/authController");

router.post("/login", authController.login);

router.get("/status", authController.status);

router.delete("/logout", authController.logout);

module.exports = router;