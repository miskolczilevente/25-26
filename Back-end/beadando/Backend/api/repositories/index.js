const StudentRepository = require("./StudentRepository");
const TeacherRepository = require("./TeacherRepository")
const ClassroomRepository = require("./ClassroomRepository")

module.exports = (db) =>
{
    const studentRepository = new StudentRepository(db);

    const teacherRepository = new TeacherRepository(db);

    const classroomRepository = new ClassroomRepository(db);

    return { studentRepository, teacherRepository, classroomRepository };
}