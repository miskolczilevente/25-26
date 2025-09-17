const express = require("express");

const app = express();

app.disable("x-powered-by");

app.get("/", (req, res, next) => 
{
    // 1. verzió
    // console.log(req.headers["authorization"]);

    // 2. verzió
    /* console.log(req.get("Authorization"));

    console.log(req.get("option1")); */

    res.status(200).send("teszt");
})

module.exports = app;