class PersonRepository
{
    // DI = Dependency Injection 

    constructor(dbParam)
    {
        this.Person = dbParam.Person;
    }

    async getPeople()
    {
        return await this.Person.findAll();
    }
}

module.exports = PersonRepository;