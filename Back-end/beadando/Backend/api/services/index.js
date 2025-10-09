const StudentService = require("./StudentService")

module.exports = (db) =>
{
    const studentService = new StudentService(db);

    return { studentService };
}