const express = require("express");

const router = express.Router();

const carController = require("../controllers/carController");

router.get("/", carController.getCars);

router.param("carBrandFrom", (req, res, next, carBrandFrom) => 
{
    req.carBrandFrom = carBrandFrom;

    next();
})

router.param("carBrandTo", (req, res, next, carBrandTo) => 
{
    req.carBrandTo = carBrandTo;

    next();
})

router.param("carModel", (req, res, next, carModel) => 
{
    req.carModel = carModel;

    next();
})

router.patch("/:carBrandFrom/:carBrandTo", carController.updateCar);

router.delete("/:carModel", carController.deleteCar);

router.get("/:carBrandForm", carController.getBrandedCar)

module.exports = router;