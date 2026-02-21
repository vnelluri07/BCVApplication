using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Repo;
using BeersCheersVasis.Repo.UnitOfWork;
using BeersCheersVasis.Repository.Implementation;

namespace BeersCheersVasis.Repository.UnitOfWork;

//public class BcvUnitOfWork : IBcvUnitOfWork
//{
    //private readonly IUnitOfWork _unitOfWork;
    //private IUserRepository? _userRepository;

    //private IdbContext _dbContext => _unitOfWork.DbContext;

    //public BcvUnitOfWork(IUnitOfWork unitOfWork)
    //{
    //    _unitOfWork = unitOfWork;
    //}

    //public IUserRepository UserRepository
    //{
    //    get
    //    {
    //        if (_userRepository == null)
    //            _userRepository = new UserRepository(_unitOfWork);
    //        return _userRepository;
    //    }
    //}


    public class BcvUnitOfWork : IUnitOfWork
    {
        public IdbContext DbContext { get; }

        public BcvUnitOfWork(IdbContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            DbContext = dbContext;
        }

    }
//}
