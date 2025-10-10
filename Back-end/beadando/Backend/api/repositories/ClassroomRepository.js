const { DbError } = require("../errors");

class ClassroomsRepository
{
    constructor(db)
    {
        this.Classroom = db.Classroom;

        this.sequelize = db.sequelize;
    }

    async getClassrooms()
    {
        try
        {
            return await this.Classroom.findAll();
        }
        catch(error)
        {
            throw new DbError("Failed to fetch classrooms", 
            {
                details: error.message,
            });
        }
    }

    async createClassroom(classroomData)
    {
        try
        {
            return await this.Classroom.create(classroomData);
        }
        catch(error)
        {
            throw new DbError("Failed to create classrooms object", 
            {
               details: error.message,
               data: classroomData, 
            });
        }
    }
}

module.exports = ClassroomsRepository;