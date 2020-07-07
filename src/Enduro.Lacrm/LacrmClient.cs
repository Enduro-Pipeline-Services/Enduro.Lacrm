using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Enduro.Lacrm.Converters;
using Enduro.Lacrm.Exceptions;
using Enduro.Lacrm.Extensions;
using Enduro.Lacrm.Functions;
using Enduro.Lacrm.Models;
using Enduro.Lacrm.Parameters;
using Enduro.Lacrm.Responses;
using JetBrains.Annotations;

namespace Enduro.Lacrm
{
    [PublicAPI]
    public class LacrmClient
    {
        private readonly HttpClient _client;
        private readonly Uri _baseAddress;

        private const string ApiToken = "APIToken";
        private const string UserCode = "UserCode";
        private const string Function = "Function";
        private const string Parameters = "Parameters";

        private const string CommunicationError =
            "An error occurred communicating with the LACRM API";

        private const string HttpCommunicationError =
            "HTTP response did not indicate success.";

        private const string ApiError =
            "API Response did not indicate success.";

        private JsonSerializerOptions _opt;

        public LacrmClient(HttpClient client, Options options)
        {
            _client = client;
            _baseAddress = new Uri(options.ApiUrl)
                .AddParameter(ApiToken, options.ApiToken)
                .AddParameter(UserCode, options.UserCode);
            
            _opt = new JsonSerializerOptions {IgnoreNullValues = true};
            _opt.Converters.Add(new NumberToStringConverter());
        }

        private void ValidateParameters<TParams>(
            ILacrmFunction<TParams> function)
            where TParams : Parameter
        {
            var validate = function.Parameters.Validate();
            
            if (!validate.Success)
                throw new ValidationException(validate);
        }

        private Uri GenerateUri<TParams>(
            ILacrmFunction<TParams> function)
            where TParams : Parameter
        {
            var param = JsonSerializer.Serialize(function.Parameters, 
                _opt);
            return _baseAddress.AddParameter(Function, function.Function)
                .AddParameter(Parameters, param);
        }
        
        private async Task<string> GetCleanedResponse(
            Uri uri,
            bool preprocess = true,
            CancellationToken cancellationToken = default) 
        {
            try
            {
                var result = await _client.GetAsync(uri, cancellationToken);

                if (!result.IsSuccessStatusCode)
                    throw new HttpException(HttpCommunicationError);

                var body = await result.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(body))
                    body = "{}";
            
                return preprocess ? Preprocess(body) : body;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpException(CommunicationError, ex);
            }
        }
        
        private async Task<IDictionary<string, object>> CallApi<TParams>(
            ILacrmFunction<TParams> function,
            CancellationToken cancellationToken = default,
            bool preprocess = true)
            where TParams : Parameter
        {
            ValidateParameters(function);
            var uri = GenerateUri(function);
            var cleaned = await GetCleanedResponse(uri, preprocess, 
                cancellationToken);
            var init = JsonSerializer.Deserialize<ExpandoObject>(
                cleaned, _opt);
            
            return init;
        }

        private async Task<TResult> CallApi<TParams, TResult>(
            ILacrmFunction<TParams> function,
            CancellationToken cancellationToken = default,
            bool preprocess = true)
            where TParams : Parameter
            where TResult : LacrmResponse
        {
            ValidateParameters(function);
            var uri = GenerateUri(function);
            var cleaned = await GetCleanedResponse(uri, preprocess, 
                cancellationToken);
            var init = JsonSerializer.Deserialize<ErrorResponse>(cleaned, _opt);
            if (!init?.Success == true)
                throw new ApiException(init?.Error ?? ApiError);

            return JsonSerializer.Deserialize<TResult>(cleaned, _opt) ??
                   throw new Exception();
        }

        private static string Preprocess(string body)
        {
            // The API returns an empty array instead of an empty object. 
            return body.Replace("\"CustomFields\":[]", "\"CustomFields\":{}")
                .Replace("\"ContactCustomFields\":[]", "\"ContactCustomFields\":{}")
                .Replace("\"Options\":[]", "\"Options\":{}");
        }

        public Task<CreateContactResponse> CreateContact(
            CreateContactParams parameters,
            CancellationToken cancellationToken = default)
        {
            var function = new CreateContact(parameters);

            return CallApi<CreateContactParams, CreateContactResponse>(function,
                cancellationToken);
        }

        public async Task<GetContactResponse> GetContact(
            string contactId,
            CancellationToken cancellationToken = default)
        {
            var function = new GetContact(contactId);

            return await CallApi<GetContactParams, GetContactResponse>(function,
                cancellationToken);
        }

        public Task<EditContactResponse> EditContact(
            EditContactParams parameters,
            CancellationToken cancellationToken = default)
        {
            var function = new EditContact(parameters);

            return CallApi<EditContactParams, EditContactResponse>(function,
                cancellationToken);
        }

        public Task<DeleteContactResponse> DeleteContact(
            string contactId,
            CancellationToken cancellationToken = default)
        {
            var function = new DeleteContact(contactId);

            return CallApi<DeleteContactParams, DeleteContactResponse>(function,
                cancellationToken);
        }

        public Task<SearchContactsResponse> SearchContacts(
            string searchTerms,
            int? numRows = null,
            string? sort = null,
            string? recordType = null,
            CancellationToken cancellationToken = default)
        {
            var parameters = new SearchContactsParams(
                searchTerms, numRows, sort, recordType);
            var function = new SearchContacts(parameters);

            return CallApi<SearchContactsParams, SearchContactsResponse>(
                function, cancellationToken);
        }

        public Task<CreateNoteResponse> CreateNote(
            string contactId,
            string note,
            CancellationToken cancellationToken = default)
        {
            var parameters = new CreateNoteParams(contactId, note);
            var function = new CreateNote(parameters);

            return CallApi<CreateNoteParams, CreateNoteResponse>(function,
                cancellationToken);
        }

        public Task<CreateTaskResponse> CreateTask(
            string contactId,
            string dueDate,
            string name,
            string description,
            string? assignedTo = null,
            CancellationToken cancellationToken = default)
        {
            var parameters = new CreateTaskParams(
                contactId,
                dueDate,
                name,
                description,
                assignedTo);
            var function = new CreateTask(parameters);

            return CallApi<CreateTaskParams, CreateTaskResponse>(function,
                cancellationToken);
        }

        public Task<CreateEventResponse> CreateEvent(
            string date,
            string startTime,
            string endTime,
            string name,
            string? description = null,
            IEnumerable<string>? contacts = null,
            IEnumerable<string>? users = null,
            CancellationToken cancellationToken = default)
        {
            var function = new CreateEvent(
                date,
                startTime,
                endTime,
                name,
                description,
                contacts,
                users);

            return CallApi<CreateEventParams, CreateEventResponse>(function,
                cancellationToken);
        }

        public Task<AddContactToGroupResponse> AddContactGroup(
            string contactId,
            string groupName,
            CancellationToken cancellationToken = default)
        {
            var function = new AddContactToGroup(contactId, groupName);

            return CallApi<AddContactToGroupParams, AddContactToGroupResponse>(
                function, cancellationToken);
        }

        public Task<CreatePipelineResponse> CreatePipeline(
            string contactId,
            string pipelineId,
            string statusId,
            string? note = null,
            int? priority = null,
            Dictionary<string, string>? customFields = null,
            CancellationToken cancellationToken = default)
        {
            var function = new CreatePipeline(
                contactId,
                pipelineId,
                statusId,
                note,
                priority,
                customFields);

            return CallApi<CreatePipelineParams, CreatePipelineResponse>(
                function, cancellationToken);
        }

        public Task<UpdatePipelineItemResponse> UpdatePipelineItem(
            string pipelineItemId,
            string statusId,
            string? note = null,
            int? priority = null,
            Dictionary<string, string>? customFields = null,
            CancellationToken cancellationToken = default)
        {
            var function = new UpdatePipelineItem(
                pipelineItemId, statusId, note, priority, customFields);

            return CallApi<UpdatePipelineItemParams,
                UpdatePipelineItemResponse>(function, cancellationToken);
        }

        public Task<GetPipelineItemsAttachedToContactResponse>
            GetPipelineItemsAttachedToContact(
                string contactId,
                CancellationToken cancellationToken = default)
        {
            var function = new GetPipelineItemsAttachedToContact(contactId);

            return CallApi<GetPipelineItemsAttachedToContactParams,
                GetPipelineItemsAttachedToContactResponse>(function,
                cancellationToken,
                preprocess: false);
        }

        public Task<GetPipelineReportResponse> GetPipelineReport(
            string pipelineId,
            string sortBy,
            int? numRows = null,
            int? page = null,
            string? sortDirection = null,
            string? userFilter = null,
            string? statusFilter = null,
            CancellationToken cancellationToken = default)
        {
            var function = new GetPipelineReport(
                pipelineId,
                sortBy,
                numRows,
                page,
                sortDirection,
                userFilter,
                statusFilter);

            return CallApi<GetPipelineReportParams, GetPipelineReportResponse>(
                function, cancellationToken);
        }

        public async Task<GetPipelineSettingsResponse> GetPipelineSettings(
            CancellationToken cancellationToken = default)
        {
            var function = new GetPipelineSettings();
            var result = await CallApi(function, cancellationToken);

            return GetPipelineSettingsResponseFromDictionary(result);
        }

        private static GetPipelineSettingsResponse 
            GetPipelineSettingsResponseFromDictionary(
            IDictionary<string, object> response)
        {
            var settings = response.Where(r => r.Key != "Success")
                .Select(r => JsonSerializer.Deserialize<PipelineSetting>(
                    r.Value.ToString()));

            return new GetPipelineSettingsResponse(settings);
        }

        public Task<GetUserInfoResponse> GetUserInfo(
            CancellationToken cancellationToken = default)
        {
            var function = new GetUserInfo();

            return CallApi<GetUserInfoParams, GetUserInfoResponse>(function,
                cancellationToken);
        }

        public Task<GetCustomFieldsResponse> GetCustomFields(
            CancellationToken cancellationToken = default)
        {
            var function = new GetCustomFields();

            return CallApi<GetCustomFieldsParams, GetCustomFieldsResponse>(
                function, cancellationToken);
        }
    }
}