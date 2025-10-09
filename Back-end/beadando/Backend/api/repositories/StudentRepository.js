const { DbError } = require("../errors");

class StudentsRepository
{
    constructor(db)
    {
        this.Student = db.Student;

        this.sequelize = db.sequelize;
    }

    async getStudents()
    {
        try
        {
            return await this.Student.findAll();
        }
        catch(error)
        {
            throw new DbError("Failed to fetch students", 
            {
                details: error.message,
            });
        }
    }

    async createStudent(studentData)
    {
        try
        {
            return await this.Student.create(studentData);
        }
        catch(error)
        {
            throw new DbError("Failed to create students object", 
            {
               details: error.message,
               data: studentData, 
            });
        }
    }
}

module.exports = StudentsRepository;