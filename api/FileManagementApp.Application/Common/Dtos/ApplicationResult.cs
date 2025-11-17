namespace FileManagementApp.Application.Common.Dtos;
public class ApplicationResult
{
	public bool IsSuccess { get; set; }
	public ApplicationError? Error { get; set; }

	public ApplicationResult()
	{

	}

	public ApplicationResult(ApplicationError error)
	{
		Error = error;
	}

	public static ApplicationResult<T> Success<T>(T data)
	{
		return new ApplicationResult<T>(data);
	}
	public static ApplicationResult<T> Failed<T>(ApplicationError error)
	{
		return new ApplicationResult<T>(error);
	}
	public static ApplicationResult Success()
	{
		return new ApplicationResult() { IsSuccess = true };
	}
	public static ApplicationResult Failed(ApplicationError error)
	{
		return new ApplicationResult(error);
	}
}

public class ApplicationResult<T> : ApplicationResult
{
	public T? Data { get; set; }
    public ApplicationResult()
    {
        
    }
    public ApplicationResult(ApplicationError error)
	{
		Error = error;
	}

	public ApplicationResult(T data)
	{
		Data = data;
		IsSuccess = true;
	}
}
