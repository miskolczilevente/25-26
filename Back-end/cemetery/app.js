const express = require("express");

const app = express();

app.disable("x-powered-by");

const personRoutes = require("./api/routes/personRoutes");

app.use("/people", personRoutes);

const errorHandler = require("./api/middlewares/errorHandler");

app.use(errorHandler);

module.exports = app;