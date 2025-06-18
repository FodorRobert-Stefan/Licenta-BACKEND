using Serilog;

public class DependencyException<T> : Exception
{
  public Type ThrowingClass { get; }

  public DependencyException(Type throwingClass)
      : base($"Dependency exception in {typeof(T).Name}, thrown from {throwingClass.Name}.")
  {
    ThrowingClass = throwingClass;
    LogError();
  }

  public DependencyException(Type throwingClass, string message)
      : base($"Dependency exception in {typeof(T).Name}, thrown from {throwingClass.Name}: {message}")
  {
    ThrowingClass = throwingClass;
    LogError();
  }

  public DependencyException(Type throwingClass, string message, Exception innerException)
      : base($"Dependency exception in {typeof(T).Name}, thrown from {throwingClass.Name}: {message}", innerException)
  {
    ThrowingClass = throwingClass;
    LogError();
  }

  private void LogError()
  {
    Log.Error(this, "Dependency exception in {DependencyType} from {ThrowingClass}: {ExceptionMessage}",
        typeof(T).Name, ThrowingClass?.Name ?? "Unknown", Message);
  }
}
