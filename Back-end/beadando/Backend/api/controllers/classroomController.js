const db = require("../db");

const { classroomService } = require("../services")(db);

exports.getClassrooms = async (req, res, next) =>
{
    try
    {
        res.status(200).json(await classroomService.getClassrooms());
    }
    catch(error)
    {
        next(error);
    }
}

exports.createClassroom = async (req, res, next) =>
{
    const { building, room_number, capacity } = req.body || {};

    try
    {
        res.status(201).json(await classroomService.createClassroom({ building, room_number, capacity }));
    }
    catch(error)
    {
        next(error);
    }
}