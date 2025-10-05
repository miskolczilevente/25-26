const express = require("express");

const app = express();

const api = express();

const cors = require("cors");

app.use(cors(
{
    origin: [ "http://localhost:3000", "https://discordapp.com" ],

    methods: [ "GET", "POST", "PATCH", "PUT", "DELETE" ],
}));

app.use(express.json());

app.use(express.urlencoded({ extended: true }));

const userRoutes = require("./api/routes/userRoutes");

const errorHandler = require("./api/middlewares/errorHandler");

const authRoutes = require("./api/routes/authRoutes");

app.use("/api", api);

api.use("/users", userRoutes);

api.use("/auth", authRoutes);

api.use(errorHandler.notFound);

app.use(errorHandler.showError);

app.use(errorHandler.notFound);

module.exports = app;