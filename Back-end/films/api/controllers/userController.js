const users = 
[
    {
        name: "User1",
        password: "password1"
    },
    {
        name: "User2",
        password: "password2"
    },
    {
        name: "User3",
        password: "password3"
    },
]

exports.getUsers = (req, res, next) =>
{
    res.status(200).json(users);
}

exports.registerUser = (req, res, next) =>
{

    const { userName, password } = req.body;
    try {
        if (!(userName && password)) {
            const error = new Error("Hiányos adat");

            error.status = 404;

            throw error;
        }

    users.push({
        name: userName,
        password: password,
    })

    res.status(201).json(
        {
            name: userName,
            password: password,
        }
    )
    } catch(error)
    {
        next(error)
    }
}
