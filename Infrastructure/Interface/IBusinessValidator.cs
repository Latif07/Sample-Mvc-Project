namespace SampleWebProject.Infrastructure.Interface {
    public interface IBusinessValidator<T> : IValidator where T : class {
        ValidationResult Validate(T entity);
    }
}
