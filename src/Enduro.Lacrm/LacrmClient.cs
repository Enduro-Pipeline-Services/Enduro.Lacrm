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
            
            _opt = new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
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

        /// <summary>
        /// Updates an existing task in Less Annoying CRM.
        /// </summary>
        /// <param name="parameters">The task update parameters including task ID and fields to modify</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response indicating success of the edit operation</returns>
        /// <exception cref="ValidationException">Thrown when parameters fail validation</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<EditTaskResponse> EditTask(
            EditTaskParams parameters,
            CancellationToken cancellationToken = default)
        {
            var function = new EditTask(parameters);

            return CallApi<EditTaskParams, EditTaskResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Deletes a task from Less Annoying CRM.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task to delete</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response indicating success of the delete operation</returns>
        /// <exception cref="ValidationException">Thrown when task ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<DeleteTaskResponse> DeleteTask(
            string taskId,
            CancellationToken cancellationToken = default)
        {
            var function = new DeleteTask(taskId);

            return CallApi<DeleteTaskParams, DeleteTaskResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves a single task by its unique identifier.
        /// </summary>
        /// <param name="taskId">The unique identifier of the task to retrieve</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing the requested task details</returns>
        /// <exception cref="ValidationException">Thrown when task ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error or task not found</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetTaskResponse> GetTask(
            string taskId,
            CancellationToken cancellationToken = default)
        {
            var function = new GetTask(taskId);

            return CallApi<GetTaskParams, GetTaskResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves multiple tasks with optional filtering and pagination.
        /// </summary>
        /// <param name="parameters">Filter parameters including date range, assignee, pagination settings</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing a collection of tasks matching the filter criteria</returns>
        /// <exception cref="ValidationException">Thrown when parameters fail validation</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetTasksResponse> GetTasks(
            GetTasksParams parameters,
            CancellationToken cancellationToken = default)
        {
            var function = new GetTasks(parameters);

            return CallApi<GetTasksParams, GetTasksResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves all tasks associated with a specific contact.
        /// </summary>
        /// <param name="contactId">The unique identifier of the contact</param>
        /// <param name="maxNumberOfResults">Maximum number of results per page (default: 500)</param>
        /// <param name="page">Page number for pagination (1-based)</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing tasks attached to the specified contact</returns>
        /// <exception cref="ValidationException">Thrown when contact ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetTasksAttachedToContactResponse> GetTasksAttachedToContact(
            string contactId,
            int? maxNumberOfResults = null,
            int? page = null,
            CancellationToken cancellationToken = default)
        {
            var parameters = new GetTasksAttachedToContactParams(contactId)
            {
                MaxNumberOfResults = maxNumberOfResults ?? 500,
                Page = page
            };
            var function = new GetTasksAttachedToContact(parameters);

            return CallApi<GetTasksAttachedToContactParams,
                GetTasksAttachedToContactResponse>(function, cancellationToken);
        }

        /// <summary>
        /// Updates an existing calendar event in Less Annoying CRM.
        /// </summary>
        /// <param name="parameters">The event update parameters including event ID and fields to modify</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response indicating success of the edit operation</returns>
        /// <exception cref="ValidationException">Thrown when parameters fail validation</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<EditEventResponse> EditEvent(
            EditEventParams parameters,
            CancellationToken cancellationToken = default)
        {
            var function = new EditEvent(parameters);

            return CallApi<EditEventParams, EditEventResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Deletes a calendar event from Less Annoying CRM.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event to delete</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response indicating success of the delete operation</returns>
        /// <exception cref="ValidationException">Thrown when event ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<DeleteEventResponse> DeleteEvent(
            string eventId,
            CancellationToken cancellationToken = default)
        {
            var function = new DeleteEvent(eventId);

            return CallApi<DeleteEventParams, DeleteEventResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves a single calendar event by its unique identifier.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event to retrieve</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing the requested event details</returns>
        /// <exception cref="ValidationException">Thrown when event ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error or event not found</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetEventResponse> GetEvent(
            string eventId,
            CancellationToken cancellationToken = default)
        {
            var function = new GetEvent(eventId);

            return CallApi<GetEventParams, GetEventResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves multiple events with optional filtering and pagination.
        /// </summary>
        /// <param name="parameters">Filter parameters including date range, contact ID, pagination settings</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing a collection of events matching the filter criteria</returns>
        /// <exception cref="ValidationException">Thrown when parameters fail validation</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetEventsResponse> GetEvents(
            GetEventsParams parameters,
            CancellationToken cancellationToken = default)
        {
            var function = new GetEvents(parameters);

            return CallApi<GetEventsParams, GetEventsResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves all calendar events associated with a specific contact.
        /// </summary>
        /// <param name="contactId">The unique identifier of the contact</param>
        /// <param name="maxNumberOfResults">Maximum number of results per page (default: 500)</param>
        /// <param name="page">Page number for pagination (1-based)</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing events attached to the specified contact</returns>
        /// <exception cref="ValidationException">Thrown when contact ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetEventsAttachedToContactResponse> GetEventsAttachedToContact(
            string contactId,
            int? maxNumberOfResults = null,
            int? page = null,
            CancellationToken cancellationToken = default)
        {
            var parameters = new GetEventsAttachedToContactParams(contactId)
            {
                MaxNumberOfResults = maxNumberOfResults ?? 500,
                Page = page
            };
            var function = new GetEventsAttachedToContact(parameters);

            return CallApi<GetEventsAttachedToContactParams,
                GetEventsAttachedToContactResponse>(function, cancellationToken);
        }

        /// <summary>
        /// Updates an existing note in Less Annoying CRM.
        /// </summary>
        /// <param name="parameters">The note update parameters including note ID and new content</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response indicating success of the edit operation</returns>
        /// <exception cref="ValidationException">Thrown when parameters fail validation</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<EditNoteResponse> EditNote(
            EditNoteParams parameters,
            CancellationToken cancellationToken = default)
        {
            var function = new EditNote(parameters);

            return CallApi<EditNoteParams, EditNoteResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Deletes a note from Less Annoying CRM.
        /// </summary>
        /// <param name="noteId">The unique identifier of the note to delete</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response indicating success of the delete operation</returns>
        /// <exception cref="ValidationException">Thrown when note ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<DeleteNoteResponse> DeleteNote(
            string noteId,
            CancellationToken cancellationToken = default)
        {
            var function = new DeleteNote(noteId);

            return CallApi<DeleteNoteParams, DeleteNoteResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves a single note by its unique identifier.
        /// </summary>
        /// <param name="noteId">The unique identifier of the note to retrieve</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing the requested note details</returns>
        /// <exception cref="ValidationException">Thrown when note ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error or note not found</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetNoteResponse> GetNote(
            string noteId,
            CancellationToken cancellationToken = default)
        {
            var function = new GetNote(noteId);

            return CallApi<GetNoteParams, GetNoteResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves multiple notes with optional filtering and pagination.
        /// </summary>
        /// <param name="parameters">Filter parameters including date range, contact ID, pagination settings</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing a collection of notes matching the filter criteria</returns>
        /// <exception cref="ValidationException">Thrown when parameters fail validation</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetNotesResponse> GetNotes(
            GetNotesParams parameters,
            CancellationToken cancellationToken = default)
        {
            var function = new GetNotes(parameters);

            return CallApi<GetNotesParams, GetNotesResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Retrieves all notes associated with a specific contact.
        /// </summary>
        /// <param name="contactId">The unique identifier of the contact</param>
        /// <param name="maxNumberOfResults">Maximum number of results per page (default: 500)</param>
        /// <param name="page">Page number for pagination (1-based)</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing notes attached to the specified contact</returns>
        /// <exception cref="ValidationException">Thrown when contact ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetNotesAttachedToContactResponse> GetNotesAttachedToContact(
            string contactId,
            int? maxNumberOfResults = null,
            int? page = null,
            CancellationToken cancellationToken = default)
        {
            var parameters = new GetNotesAttachedToContactParams(contactId)
            {
                MaxNumberOfResults = maxNumberOfResults ?? 500,
                Page = page
            };
            var function = new GetNotesAttachedToContact(parameters);

            return CallApi<GetNotesAttachedToContactParams,
                GetNotesAttachedToContactResponse>(function, cancellationToken);
        }

        /// <summary>
        /// Deletes a pipeline item, removing a contact from a pipeline.
        /// </summary>
        /// <param name="pipelineItemId">The unique identifier of the pipeline item to delete</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response indicating success of the delete operation</returns>
        /// <exception cref="ValidationException">Thrown when pipeline item ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<DeletePipelineItemResponse> DeletePipelineItem(
            string pipelineItemId,
            CancellationToken cancellationToken = default)
        {
            var function = new DeletePipelineItem(pipelineItemId);

            return CallApi<DeletePipelineItemParams, DeletePipelineItemResponse>(
                function, cancellationToken);
        }

        /// <summary>
        /// Retrieves a single pipeline item by its unique identifier.
        /// </summary>
        /// <param name="pipelineItemId">The unique identifier of the pipeline item to retrieve</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing the requested pipeline item details</returns>
        /// <exception cref="ValidationException">Thrown when pipeline item ID is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error or item not found</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetPipelineItemResponse> GetPipelineItem(
            string pipelineItemId,
            CancellationToken cancellationToken = default)
        {
            var function = new GetPipelineItem(pipelineItemId);

            return CallApi<GetPipelineItemParams, GetPipelineItemResponse>(
                function, cancellationToken);
        }

        /// <summary>
        /// Removes a contact from a group.
        /// </summary>
        /// <param name="contactId">The unique identifier of the contact</param>
        /// <param name="groupName">The name of the group to remove the contact from</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response indicating success of the removal operation</returns>
        /// <exception cref="ValidationException">Thrown when parameters are invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<RemoveContactFromGroupResponse> RemoveContactFromGroup(
            string contactId,
            string groupName,
            CancellationToken cancellationToken = default)
        {
            var function = new RemoveContactFromGroup(contactId, groupName);

            return CallApi<RemoveContactFromGroupParams, 
                RemoveContactFromGroupResponse>(function, cancellationToken);
        }

        /// <summary>
        /// Retrieves all groups in the LACRM account.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing a collection of all groups</returns>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<GetGroupsResponse> GetGroups(
            CancellationToken cancellationToken = default)
        {
            var function = new GetGroups();

            return CallApi<GetGroupsParams, GetGroupsResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Creates a new group in Less Annoying CRM.
        /// </summary>
        /// <param name="groupName">The name for the new group</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response containing the ID of the newly created group</returns>
        /// <exception cref="ValidationException">Thrown when group name is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error (e.g., group already exists)</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<CreateGroupResponse> CreateGroup(
            string groupName,
            CancellationToken cancellationToken = default)
        {
            var function = new CreateGroup(groupName);

            return CallApi<CreateGroupParams, CreateGroupResponse>(function,
                cancellationToken);
        }

        /// <summary>
        /// Deletes a group from Less Annoying CRM. Contacts in the group are not deleted.
        /// </summary>
        /// <param name="groupIdOrName">The unique identifier or name of the group to delete</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Response indicating success of the delete operation</returns>
        /// <exception cref="ValidationException">Thrown when group identifier is invalid</exception>
        /// <exception cref="ApiException">Thrown when the API returns an error</exception>
        /// <exception cref="HttpException">Thrown when HTTP communication fails</exception>
        public Task<DeleteGroupResponse> DeleteGroup(
            string groupIdOrName,
            CancellationToken cancellationToken = default)
        {
            var function = new DeleteGroup(groupIdOrName);

            return CallApi<DeleteGroupParams, DeleteGroupResponse>(function,
                cancellationToken);
        }
    }
}