const express = require("express");

const app = express();

const api = express();

app.use(express.json());

app.use(express.urlencoded({ extended: true }));

const studentRoutes = require("./api/routes/studentRoutes");

const teacherRoutes = require("./api/routes/teacherRoutes");

const classroomRoutes = require("./api/routes/classroomRoutes")

app.use("/api", api);

api.use("/students", studentRoutes);

api.use("/teachers", teacherRoutes)

api.use("/classrooms", classroomRoutes)

const errorHandler = require("./api/middlewares/errorHandler");

api.use(errorHandler.notFound);

app.use(errorHandler.showError);

app.use(errorHandler.notFound);

module.exports = app;