
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.interfaces;


namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDto>>>
        {

        }

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;
            private readonly IuserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IuserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;

                _context = context;
            }
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities
               .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,new {currentUsername = _userAccessor.GetUsername()})
            .ToListAsync(cancellationToken);



                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}