using System;
using Models;

namespace SampleWebProject.Infrastructure.Interface {
    public interface IUnitOfWork : IDisposable {
        SampleEntities Context { get; }

        int SaveChanges();
    }
}
