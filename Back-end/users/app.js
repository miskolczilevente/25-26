const express = require("express");

const app = express();

const api = express();

app.use(express.json());

app.use(express.urlencoded({ extended: true }));

const userRoutes = require("./api/routes/userRoutes");

const errorHandler = require("./api/middlewares/errorHandler");

app.use("/api", api);

api.use("/users", userRoutes);

api.use(errorHandler.notFound);

app.use(errorHandler.showError);

app.use(errorHandler.notFound);

module.exports = app;