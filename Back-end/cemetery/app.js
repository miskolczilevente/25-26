const express = require("express");

const app = express();

app.disable("x-powered-by");

const errorHandler = require("./api/middlewares/errorHandler");

app.use(errorHandler);

module.exports = app;