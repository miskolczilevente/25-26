const { BadRequestError } = require("../errors");

class ClassroomService
{
    constructor(db)
    {
        this.classroomRepository = require("../repositories")(db).classroomRepository;
    }

    async getClassrooms()
    {
        return await this.classroomRepository.getClassrooms();
    }

    async createClassroom(classroomData)
    {
        if(!classroomData) throw new BadRequestError("Missing classroom data from payload", 
        {
            data: classroomData,
        });

        if(!classroomData.building) throw new BadRequestError("Missing classroom name from payload",
        {
            data: classroomData,
        });

        if(!classroomData.room_number) throw new BadRequestError("Missing classroom name from payload",
        {
            data: classroomData,
        });

        if(!classroomData.capacity) throw new BadRequestError("Missing classroom name from payload",
        {
            data: classroomData,
        });


        return await this.classroomRepository.createClassroom(classroomData);
    }
}

module.exports = ClassroomService;