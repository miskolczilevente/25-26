class PersonRepository
{
    // DI = Dependency Injection

    constructor(db)
    {
        this.Person = db.Person


    }

    async getPeople()
    {
        return this.Person.findAll(); //Select * FROM 
    }
}