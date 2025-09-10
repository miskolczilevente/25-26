const films = 
[
    {
        name: "Film1",
        imdScore: 5.8
    },
    {
        name: "Film2",
        imdScore: 8
    },
    {
        name: "Film3",
        imdScore: 2.5
    },
]

exports.getRoutes = (req, res, next) =>
{
    res.status(200).send("<p>Az elérhető útvonalak:</p><a href='http://localhost:3000/films'>Filmek</a>");
}

exports.getFilms = (req, res, next) =>
{
    res.status(200).json(films);
}

exports.addFilm = (req, res, next) =>
{
    try
    {
        if(films.some(item => item.name == req.filmName))
        {
            const error = new Error("Már létezik");

            error.status = 400;

            throw error;
        }

        films.push(
        {
        name: req.filmName,
        imdScore: 10 
        });

        res.status(201).send(`${req.filmName} nevű film rögzítésre került!`);
        }
    catch(error)
    {
        next(error);
    }
}
