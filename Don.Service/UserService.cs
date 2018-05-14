using Don.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Don.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Regist()
        {

        }
    }
}
