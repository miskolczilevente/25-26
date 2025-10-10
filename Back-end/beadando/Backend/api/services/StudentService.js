const { BadRequestError } = require("../errors");

class StudentService
{
    constructor(db)
    {
        this.studentRepository = require("../repositories")(db).studentRepository;
    }

    async getStudents()
    {
        return await this.studentRepository.getStudents();
    }

    async getStudentById(id) 
    {
        return await this.studentRepository.getStudentById(id)
    }

    async createStudent(studentData)
    {
        if(!studentData) throw new BadRequestError("Missing student data from payload", 
        {
            data: studentData,
        });

        if(!studentData.name) throw new BadRequestError("Missing student name from payload",
        {
            data: studentData,
        });

        if(!studentData.birth_date) throw new BadRequestError("Missing birth date from payload",
        {
            data: studentData,
        });

        if(!studentData.email) throw new BadRequestError("Missing email from payload",
        {
            data: studentData,
        });

        if(!studentData.major) throw new BadRequestError("Missing major from payload",
        {
            data: studentData,
        });

        return await this.studentRepository.createStudent(studentData);
    }

    async deleteStudent(id) 
    {
        return await this.studentRepository.deleteStudent(id);
    }

    async updateStudent(id, studentData)
    {
        if(!studentData) throw new BadRequestError("Missing student data from payload", 
        {
            data: studentData,
        });

        if(!studentData.name) throw new BadRequestError("Missing student name from payload",
        {
            data: studentData,
        });

        if(!studentData.birth_date) throw new BadRequestError("Missing birth date from payload",
        {
            data: studentData,
        });

        if(!studentData.email) throw new BadRequestError("Missing email from payload",
        {
            data: studentData,
        });

        if(!studentData.major) throw new BadRequestError("Missing major from payload",
        {
            data: studentData,
        });

        return await this.studentRepository.updateStudent(id, studentData);
    }

}

module.exports = StudentService;