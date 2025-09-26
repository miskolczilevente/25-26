const PersonRepository = require("../repositories/PersonRepository");

class PersonService
{
    constructor(dbParam)
    {
        this.personRepository = new PersonRepository(dbParam);
    }

    async getPeople()
    {
        return await this.personRepository.getPeople();
    }
}

module.exports = PersonService;