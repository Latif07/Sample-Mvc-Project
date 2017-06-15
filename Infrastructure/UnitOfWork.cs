using System;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure {
    public class UnitOfWork : IUnitOfWork {
        private SampleEntities _context;

        public SampleEntities Context {
            get { return _context ?? (_context = new SampleEntities()); }
        }

        public void Dispose(bool disposing) {
            if (disposing) {
                if (_context != null) {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int SaveChanges() {
            return Context.SaveChanges();
        }
    }
}