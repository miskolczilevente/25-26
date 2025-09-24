function notFound(req, res, next)
{
    const error = new Error("Not found");

    error.status = 404;

    next(error);
}

function showError(error, req, res, next)
{
    res.status(error.status || 500).json(
    {
        code: error.status || 500,
        msg: error.message || "Internal Server Error"
    });
}

module.exports = [ notFound, showError ];