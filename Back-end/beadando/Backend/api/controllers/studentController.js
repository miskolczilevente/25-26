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

exports.getStudentById = async (req, res, next) => 
{
    const id = req.params.id;

    try 
    {
        res.status(200).json(await studentService.getStudentById(id));    
    }
    catch (error) 
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

exports.deleteStudent = async (req, res, next) =>
{
    const id = req.params.id;

    try 
    {
        res.status(204).json(await studentService.deleteStudent(id));
    }
    catch (error) 
    {
        next(error);
    }
}

exports.updateStudent = async (req, res, next) => 
{
    const id = req.params.id;
    
    const { name, birth_date, email ,major } = req.body || {};

    try 
    {
        res.status(200).json(await studentService.updateStudent(id, {name, birth_date, email, major}));
    }
    catch (error) 
    {
        next(error);
    }
}

