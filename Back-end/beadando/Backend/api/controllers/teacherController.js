const db = require("../db");

const { teacherService } = require("../services")(db);

exports.getTeachers = async (req, res, next) =>
{
    try
    {
        res.status(200).json(await teacherService.getTeachers());
    }
    catch(error)
    {
        next(error);
    }
}

exports.createTeacher = async (req, res, next) =>
{
    const { name, department, email ,position } = req.body || {};

    try
    {
        res.status(201).json(await teacherService.createTeacher({ name, department, email ,position  }));
    }
    catch(error)
    {
        next(error);
    }
}