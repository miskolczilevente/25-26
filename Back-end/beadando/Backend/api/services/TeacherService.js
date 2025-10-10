const { BadRequestError } = require("../errors");

class TeacherService
{
    constructor(db)
    {
        this.teacherRepository = require("../repositories")(db).teacherRepository;
    }

    async getTeachers()
    {
        return await this.teacherRepository.getTeachers();
    }

    async createTeacher(teacherData)
    {
        if(!teacherData) throw new BadRequestError("Missing teacher data from payload", 
        {
            data: teacherData,
        });

        if(!teacherData.name) throw new BadRequestError("Missing teacher name from payload",
        {
            data: teacherData,
        });

        if(!teacherData.department) throw new BadRequestError("Missing teacher name from payload",
        {
            data: teacherData,
        });

        if(!teacherData.email) throw new BadRequestError("Missing teacher name from payload",
        {
            data: teacherData,
        });        

        if(!teacherData.position) throw new BadRequestError("Missing teacher name from payload",
        {
            data: teacherData,
        });  

        return await this.teacherRepository.createTeacher(teacherData);
    }
}

module.exports = TeacherService;