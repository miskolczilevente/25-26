const StudentRepository = require("./StudentRepository");

module.exports = (db) =>
{
    const studentRepository = new StudentRepository(db);

    return { studentRepository };
}