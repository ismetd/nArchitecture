using Application.Features.Brands.Models;
using Application.Features.Models.Models;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Models.Queries.GetListModel;

public class GetListModelQuery : IRequest<ModelListModel>
{
    public PageRequest PageRequest { get; set; }

    public class GetListModelQueryHandler : IRequestHandler<GetListModelQuery, ModelListModel>
    {
        private IModelRepository _modelRepository;
        private IMapper _mapper;

        public GetListModelQueryHandler(IModelRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<ModelListModel> Handle(GetListModelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Model> models = await _modelRepository.GetListAsync(include:
                                            m=> m.Include(c=> c.Brand),
                                            index:request.PageRequest.Page,
                                            size:request.PageRequest.PageSize
            );
            
            ModelListModel mappedModels = _mapper.Map<ModelListModel>(models);

            return mappedModels;
        }
    }
}
