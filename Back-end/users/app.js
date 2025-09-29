const express = require("express");

const app = express();

const api = express();

const userRoutes = require("./api/routes/userRoutes");

app.use("/api", api);

api.use("/users", [userRoutes]);

module.exports = app;