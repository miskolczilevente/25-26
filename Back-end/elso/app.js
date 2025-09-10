const express = require("express")
const app = express()

// ENDPOINT
// /users (GET)
// Válasz: userek adatai

app.get("/users", (req, res, next) =>{
    res.status(200).send("userek adatai");
})

app.param("/users/:userName", (req, res, next, userName) =>
{
    req.userName = userName

    next()
})

app.get("/users/:userName", (req, res, next) =>{
    res.status(200).send(`Üdv, ${req.userName}`);
})

app.post("/users", (req, res, next) =>{
    res.status(201).send("Sikeres regisztráció");
})

app.post("/users", (req, res, next) =>{
    res.status(201).send(`${userName} Sikeresen regisztrált!`);
})


module.exports = app