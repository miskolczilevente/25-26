class AppError extends Error
{
    constructor(message = "Internal Server Error", 
    { statusCode = 500, isOperational = true, details = undefined, data = undefined } = {}, )
    {
        super(message);

        this.statusCode = statusCode;

        this.isOperational = isOperational;

        this.details = details;

        this.data = data;

        process.env.NODE_ENV !== "production" ? Error.captureStackTrace(this, this.constructor) : undefined;
    }
}

module.exports = AppError;