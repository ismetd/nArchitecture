using Application.Features.Models.Models;
using Application.Features.Models.Queries.GetListModel;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Requests;
using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Models.Queries.GetListModelByDynamic;


public class GetListModelByDynamicQuery : IRequest<ModelListModel>
{
    public Dynamic Dynamic { get; set; }
    public PageRequest PageRequest { get; set; }

    public class GetListModelByDynamicQueryHandler : IRequestHandler<GetListModelByDynamicQuery, ModelListModel>
    {
        private IModelRepository _modelRepository;
        private IMapper _mapper;

        public GetListModelByDynamicQueryHandler(IModelRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<ModelListModel> Handle(GetListModelByDynamicQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Model> models = await _modelRepository.GetListByDynamicAsync(request.Dynamic, include:
                m=> m.Include(c=> c.Brand),
                index:request.PageRequest.Page,
                size:request.PageRequest.PageSize
            );
            
            ModelListModel mappedModels = _mapper.Map<ModelListModel>(models);

            return mappedModels;
        }
    }
}

