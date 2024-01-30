namespace EHR.Journey.DataDictionaryManagement.DataDictionaries
{
    [Authorize(DataDictionaryManagementPermissions.DataDictionaryManagement.Default)]
    public class DataDictionaryAppService : DataDictionaryManagementAppService, IDataDictionaryAppService
    {
        /// <summary>
        ///  注意 为了快速直接注入仓库层 规范上是不允许的
        ///  这里注入仓储也只是为了查询分页
        ///  如果是其他的操作全部通过对应manger进行操作
        /// </summary>
        private readonly IDataDictionaryRepository _dataDictionaryRepository;

        private readonly IDataDictionaryManager _dataDictionaryManager;

        public DataDictionaryAppService(
            IDataDictionaryRepository dataDictionaryRepository,
            IDataDictionaryManager dataDictionaryManager)
        {
            _dataDictionaryRepository = dataDictionaryRepository;
            _dataDictionaryManager = dataDictionaryManager;
        }

        /// <summary>
        /// 分页查询字典项
        /// </summary>
        public virtual async Task<PagedResultDto<PagingDataDictionaryOutput>> GetPagingListAsync(PagingDataDictionaryInput input)
        {
            var result = new PagedResultDto<PagingDataDictionaryOutput>();
            var totalCount = await _dataDictionaryRepository.GetPagingCountAsync(input.Filter);
            result.TotalCount = totalCount;
            if (totalCount <= 0) return result;

            var entities = await _dataDictionaryRepository.GetPagingListAsync(input.Filter, input.PageSize,
                input.SkipCount, false);
            result.Items = ObjectMapper.Map<List<DataDictionary>, List<PagingDataDictionaryOutput>>(entities);

            return result;
        }


        /// <summary>
        /// 分页查询字典项明细
        /// </summary>
        public virtual async Task<PagedResultDto<PagingDataDictionaryDetailOutput>> GetPagingDetailListAsync(
            PagingDataDictionaryDetailInput input)
        {
            var entity = await _dataDictionaryRepository.FindByIdAsync(input.DataDictionaryId);
            if (entity == null)
            {
                return new PagedResultDto<PagingDataDictionaryDetailOutput>();
            }

            var details = entity.Details
                .WhereIf(input.Filter.IsNotNullOrWhiteSpace(), e => (e.Code.Contains(input.Filter) || e.DisplayText.Contains(input.Filter)))
                .OrderBy(e => e.Order)
                .ThenBy(e => e.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.PageSize)
                .ToList();
            if (details.Count == 0)
            {
                return new PagedResultDto<PagingDataDictionaryDetailOutput>();
            }

            return new PagedResultDto<PagingDataDictionaryDetailOutput>(
                entity.Details.Count,
                ObjectMapper.Map<List<DataDictionaryDetail>, List<PagingDataDictionaryDetailOutput>>(details));
        }


        /// <summary>
        /// 创建字典类型
        /// </summary>
        [Authorize(DataDictionaryManagementPermissions.DataDictionaryManagement.Create)]
        public virtual Task CreateAsync(CreateDataDictinaryInput input)
        {
            return _dataDictionaryManager.CreateAsync(input.Code, input.DisplayText, input.Description);
        }

        /// <summary>
        /// 新增字典明细
        /// </summary>
        [Authorize(DataDictionaryManagementPermissions.DataDictionaryManagement.Create)]
        public virtual Task CreateDetailAsync(CreateDataDictinaryDetailInput input)
        {
            return _dataDictionaryManager.CreateDetailAsync(input.Id, input.Code, input.DisplayText, input.Description,
                input.Order);
        }

        /// <summary>
        /// 设置字典明细状态
        /// </summary>
        [Authorize(DataDictionaryManagementPermissions.DataDictionaryManagement.Update)]
        public virtual Task SetStatus(SetDataDictinaryDetailInput input)
        {
            return _dataDictionaryManager.SetStatus(input.DataDictionaryId, input.DataDictionayDetailId,
                input.IsEnabled);
        }

        [Authorize(DataDictionaryManagementPermissions.DataDictionaryManagement.Update)]
        public virtual Task UpdateDetailAsync(UpdateDetailInput input)
        {
            return _dataDictionaryManager.UpdateDetailAsync(input.DataDictionaryId, input.Id, input.DisplayText, input.Description,
                input.Order);
        }


        [Authorize(DataDictionaryManagementPermissions.DataDictionaryManagement.Delete)]
        public virtual Task DeleteAsync(DeleteDataDictionaryDetailInput input)
        {
            return _dataDictionaryManager.DeleteAsync(input.DataDictionaryId, input.DataDictionayDetailId);
        }


        [Authorize(DataDictionaryManagementPermissions.DataDictionaryManagement.Delete)]
        public virtual Task DeleteDataDictionaryTypeAsync(IdInput input)
        {
            return _dataDictionaryManager.DeleteDataDictionaryTypeAsync(input.Id);
        }

        [Authorize(DataDictionaryManagementPermissions.DataDictionaryManagement.Update)]
        public virtual Task UpdateAsync(UpdateDataDictinaryInput input)
        {
            return _dataDictionaryManager.UpdateAsync(input.Id, input.DisplayText, input.Description);
        }
    }
}