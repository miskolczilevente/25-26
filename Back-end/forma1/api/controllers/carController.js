const cars =
[
    {
        model: "1293",
        brand: "BMW"
    },
    {
        model: "1312",
        brand: "Ferrari"
    },
    {
        model: "1312",
        brand: "Ferrari"
    },
    {
        model: "1312",
        brand: "Ferrari"
    },
    {
        model: "1312",
        brand: "Ferrari"
    },
    {
        model: "790",
        brand: "McLaren"
    },
]

exports.getCars = (req, res, next) =>
{   
    res.status(200).json(cars);
}

exports.updateCar = (req, res, next) =>
{
    const items = cars.filter(car => car.brand === req.carBrandFrom);

    for(let car of items)
    {
        car.brand = req.carBrandTo;
    }

    res.status(200).json(cars);
}

exports.deleteCar = (req, res, next) =>
{
    const currentCars = cars.filter(item => item.model == req.carModel);

    res.status(200).json(cars);
}

exports.getBrandedCar = (req, res, next) => {
    
    const items = cars.filter(item => item.model == req.carModel)

    res.status(200).json(items)
}