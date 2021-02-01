﻿using System;
using System.Threading.Tasks;
using DLL.DBContext;

namespace DLL.Repositories
{
    public interface IUnitOfWork
    {
        IDepartmentRepository DepartmentRepository { get; }
        IStudentRepository StudentRepository { get; }
        ICourseRepository CourseRepository { get; }
        ICourseStudentRepository CourseStudentRepository { get; }
        Task<bool> SaveCompletedAsync();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly IDepartmentRepository _departmentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;

        public IDepartmentRepository DepartmentRepository =>
            _departmentRepository ?? new DepartmentRepository(_context);
        public IStudentRepository StudentRepository =>
            _studentRepository ?? new StudentRepository(_context);
        public ICourseRepository CourseRepository =>
            _courseRepository ?? new CourseRepository(_context);
        public ICourseStudentRepository CourseStudentRepository =>
            _courseStudentRepository ?? new CourseStudentRepository(_context);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public async Task<bool> SaveCompletedAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}