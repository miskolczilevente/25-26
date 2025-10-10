const { DbError } = require("../errors");

class TeachersRepository
{
    constructor(db)
    {
        this.Teacher = db.Teacher;

        this.sequelize = db.sequelize;
    }

    async getTeachers()
    {
        try
        {
            return await this.Teacher.findAll();
        }
        catch(error)
        {
            throw new DbError("Failed to fetch teachers", 
            {
                details: error.message,
            });
        }
    }

    async createTeacher(teacherData)
    {
        try
        {
            return await this.Teacher.create(teacherData);
        }
        catch(error)
        {
            throw new DbError("Failed to create teachers object", 
            {
               details: error.message,
               data: teacherData, 
            });
        }
    }
}

module.exports = TeachersRepository;