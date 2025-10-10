const { where } = require("sequelize");
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

    async getStudentById(id) 
    {
        try 
        {
            return await this.Student.findByPk(id);
        }
        catch (error) 
        {
            throw new DbError("Failed to fetch studentById",
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

    async deleteStudent(id)
    {
        try 
        {
            return await this.Student.destroy({where: { ID: id } });
        } catch (error) {
            throw new DbError("Failed to delete student object",
            {
                details: error.message,
            })
        }
    }

    async updateStudent(id, studentData) 
    {
        try 
        {
            return await this.Student.update( studentData, {where: { ID: id}  })
        }
        catch (error) 
        {
            throw new DbError("Failed to update student object",
            {
                details: error.message,
            })
        }
    }
}

module.exports = StudentsRepository;