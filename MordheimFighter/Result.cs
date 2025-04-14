
public class Result<T>
{
    public T Value { get; }
    public bool IsSuccess { get; }

    private Result(T value, bool isSuccess)
    {
        Value = value;
        IsSuccess = isSuccess;
    }

    public static Result<T> Success(T value) => new(value, true);
    public static Result<T> Failure(T value) => new(value, false);

    public Result<U> Bind<U>(Func<T, Result<U>> next)
    {
        return IsSuccess ? next(Value) : Result<U>.Failure(default!);
    }

    public Result<T> OnFailure(Func<T, Result<T>> failureHandler)
    {
        return IsSuccess ? this : failureHandler(Value);
    }

}
