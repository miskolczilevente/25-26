const express = require("express");

const app = express();

app.use(express.urlencoded({ extended: true}))

app.use(express.json())

// MVC = ModelViewController

const filmRoutes = require("./api/routes/filmRoutes");

const userRoutes = require("./api/routes/userRoutes");

const errorHandler = require("./api/middlewares/errorHandler");


app.use("/", filmRoutes);

app.use("/users", userRoutes)

app.use(errorHandler);

module.exports = app;