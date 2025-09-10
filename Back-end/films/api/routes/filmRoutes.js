const express = require("express");

const router = express.Router();

const filmController = require("../controllers/filmController");


// /films/:filmName
// Metódus: POST
// Művelet: Szúrjuk be a paraméterből kapott nevű filmet a statikus tömbünkben
// Válasz: ${filmName} nevű film rögzítésre került!
// Status code: 201
// HA lehet ÉS emlékszel rá: használj paraméter(hez) middlewaret

router.param("filmName", (req, res, next, filmName) => 
{
    req.filmName = filmName;

    next();
})

router.get("/", filmController.getRoutes)

router.get("/films", filmController.getFilms)

router.get("/films/:fileName", filmController.addFilm)


module.exports = router;