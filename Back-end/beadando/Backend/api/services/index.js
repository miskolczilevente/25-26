const StudentService = require("./StudentService")
const TeacherService = require("./TeacherService")
const ClassroomService = require("./ClassroomService")

module.exports = (db) =>
{
    const studentService = new StudentService(db);

    const teacherService = new TeacherService(db);

    const classroomService = new ClassroomService(db);

    return { studentService, teacherService, classroomService };
}