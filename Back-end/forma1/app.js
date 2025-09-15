const express = require("express");

const app = express();

// Feladat
// /cars (GET)
// Válasz: array

const carRoutes = require("./api/routes/carRoutes");

// const errorHandler = require("./api/middlewares/errorHandler");

app.use("/cars", carRoutes);

// GET
// /car/<brand>
// Válasz: array a keresett brandü kocsikkal

const errorHandler = require("./api/middlewares/errorHandler")

app.use(errorHandler)

// app.use(errorHandler);

module.exports = app;