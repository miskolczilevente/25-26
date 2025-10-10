const db = require("../db");

const { studentService } = require("../services")(db);

exports.getStudents = async (req, res, next) =>
{
    try
    {
        res.status(200).json(await studentService.getStudents());
    }
    catch(error)
    {
        next(error);
    }
}

exports.createStudent = async (req, res, next) =>
{
    const { name, birth_date, email ,major } = req.body || {};

    try
    {
        res.status(201).json(await studentService.createStudent({ name, birth_date, email ,major  }));
    }
    catch(error)
    {
        next(error);
    }
}