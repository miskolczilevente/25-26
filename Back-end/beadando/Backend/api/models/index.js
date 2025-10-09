module.exports = (sequelize) =>
{
    const Student = require("./Student")(sequelize);
 
    const Enrollment = require("./Enrollment")(sequelize);

    const Course = require("./Course")(sequelize);

    const Teacher = require("./Teacher")(sequelize);

    const Schedule = require("./Schedule")(sequelize);

    const Classroom = require("./Classroom")(sequelize);




    Student.hasMany(Enrollment, {
        foreignKey: "student_id",
        onDelete: "CASCADE"
    })
    Enrollment.belongsTo(Student, {
        foreignKey: "student_id"
    })

    Enrollment.belongsTo(Course, {
        foreignKey: "course_id"
    })
    Course.hasMany(Enrollment, {
        foreignKey: "course_id"
    })

    Course.belongsTo(Teacher, {
        foreignKey: "teacher_id"
    })

    Course.hasMany(Schedule, {
        foreignKey: "course_id"
    })
    Teacher.hasMany(Course, {
        foreignKey: "teacher_id"
    })
    Schedule.belongsTo(Course, {
        foreignKey: "course_id"
    })

    Schedule.belongsTo(Classroom, {
        foreignKey: "classroom_id"
    })
    Classroom.hasMany(Schedule, {
        foreignKey: "classroom_id"
    })

    return { Student, Enrollment, Course, Teacher, Schedule, Classroom };
}