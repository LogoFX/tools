using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NDatabase;
using NDatabase.Api;

namespace $safeprojectname$
{
    /// <summary>
    /// Represents simple persistable object storage.
    /// </summary>
    /// <remarks>Please pay attention, the class implements IDisposable interface.</remarks>
    public sealed class Storage : IDisposable
    {
        private readonly IOdb _db;

        public Storage()
        {
            try
            {
                _db = OdbFactory.Open("\\Infra\\objects.ndb");
            }

            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                throw;
            }
        }

        [SuppressMessage("ReSharper", "EmptyDestructor")]
        ~Storage()
        {

        }

        public IQueryable<T> Get<T>()
        {
            return _db.AsQueryable<T>();
        }

        public void Store(object item)
        {
            _db.Store(item);
        }

        public void Remove(object item)
        {
            _db.Delete(item);
        }

        public void RemoveAll<T>() where T : class
        {
            var all = _db.AsQueryable<T>().ToList();
            foreach (var single in all)
            {
                _db.Delete(single);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                }
            }
        }
    }
}