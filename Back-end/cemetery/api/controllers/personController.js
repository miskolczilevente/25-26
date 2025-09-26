const PersonService = require("../services/PersonService");

const db = require("../db/db");

const personService = new PersonService(db); 

exports.getPeople = async (req, res, next) =>
{
    const people = await personService.getPeople();

    res.status(200).json(people);
}