# Changelog

All notable changes to the Enduro.Lacrm library will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2025-01-13

### 🎉 Major Release - Full LACRM API v2 Support

This is a major release that upgrades the library to .NET 10 LTS and adds comprehensive support for the complete Less Annoying CRM API v2, expanding from 15 functions to **37+ functions**.

### Added

#### 🔧 Framework & Infrastructure
- **Upgraded to .NET 10 LTS** from .NET Standard 2.1
- Added comprehensive XML documentation for all public APIs
- Added full docfx documentation with 9 comprehensive guides
- Created complete test suite with 31+ unit and integration tests
- Added support for pagination across all list endpoints
- Enabled documentation file generation in builds

#### 📋 Task API (5 new functions)
- `EditTask()` - Update existing tasks with full field support
- `DeleteTask()` - Permanently remove tasks
- `GetTask()` - Retrieve single task by ID with metadata
- `GetTasks()` - List tasks with filtering (date range, users, completion status)
- `GetTasksAttachedToContact()` - Get all tasks for a specific contact

#### 📅 Event API (5 new functions + enhancements)
- `EditEvent()` - Update existing calendar events
- `DeleteEvent()` - Remove events from calendars
- `GetEvent()` - Retrieve single event with full details
- `GetEvents()` - List events with advanced filtering
- `GetEventsAttachedToContact()` - Get all events for a specific contact
- **Enhanced `CreateEvent()`** - Added v2 parameters:
  - IsAllDay for all-day events
  - Location field
  - Attendees array with attendance status
  - IsRecurring and RecurrenceRule for recurring events
  - EndRecurrenceDate for series management

#### 📝 Note API (5 new functions)
- `EditNote()` - Update existing notes
- `DeleteNote()` - Remove notes from contacts
- `GetNote()` - Retrieve single note by ID
- `GetNotes()` - List notes with filtering (date range, users)
- `GetNotesAttachedToContact()` - Get all notes for a specific contact

#### 🔄 Pipeline API (2 new functions)
- `DeletePipelineItem()` - Remove pipeline items
- `GetPipelineItem()` - Retrieve single pipeline item with full details

#### 👥 Group API (4 new functions)
- `RemoveContactFromGroup()` - Remove contacts from groups
- `GetGroups()` - List all available groups
- `CreateGroup()` - Create new contact groups
- `DeleteGroup()` - Remove groups

#### 🎯 New Models
- `Task` - Complete task model with all API v2 fields
- `Event` - Enhanced event model with recurring event support
- `Attendee` - Event attendee with attendance status
- `Note` - Complete note model with pipeline info
- `PipelineInfo` - Nested pipeline information in notes
- `PipelineItem` - Full pipeline item details
- `Group` - Group information with contact counts
- Metadata models: `UserMetaData`, `ContactMetaData`, `CalendarMetaData`, `PipelineMetaData`, `StatusMetaData`

#### 📚 Documentation
- Added 9 comprehensive docfx guides (Getting Started, Tasks, Events, Notes, Contacts, Pipelines, Groups, Error Handling, Pagination)
- Added migration guide from v1.0.1 to v2.0.0
- Added XML documentation to all 21 new public methods
- Updated README with complete function list and examples
- Added detailed code examples for every API function

#### 🧪 Testing
- Created comprehensive test project with xUnit
- Added 28 parameter validation tests
- Added 3 integration test examples
- Added test documentation and best practices guide

### Changed

#### Breaking Changes
- **Target Framework**: Changed from .NET Standard 2.1 to .NET 10 LTS
  - **Impact**: Requires .NET 10 runtime
  - **Migration**: Update to .NET 10 SDK
- **System.Text.Json**: Updated deprecated `IgnoreNullValues` to `DefaultIgnoreCondition`
  - **Impact**: Internal change only, no public API impact

#### Non-Breaking Changes
- Enhanced `CreateEvent()` with new optional parameters (backward compatible)
- Updated package version from 1.0.1 to 2.0.0
- Updated package description for better clarity
- Updated JetBrains.Annotations from 2020.1.0 to 2024.3.0
- Removed unnecessary Microsoft.CSharp dependency
- Updated System.Text.Json to version 10.0.0
- Changed package license from URL to SPDX expression (MIT)

### Technical Improvements

- **Code Quality**
  - Added XML documentation generation
  - Improved parameter validation across all functions
  - Consistent use of CancellationToken across all async methods
  - Enhanced nullable reference type annotations

- **Architecture**
  - Maintained existing Function/Parameter/Response pattern
  - Consistent error handling across all new functions
  - Improved code organization with proper namespacing

- **Build & Deploy**
  - Build generates XML documentation file
  - Test project integrated into solution
  - Zero build warnings (except package pruning advisory)

### Statistics

- **API Functions**: Expanded from 15 to 37+ functions (146% increase)
- **Files Added**: 90+ new files
- **Test Coverage**: 31+ tests covering all major scenarios
- **Documentation**: 200+ code examples across 9 guides
- **Models**: 15+ new model classes

### Backward Compatibility

✅ **Fully backward compatible** - All existing v1.0.1 code continues to work without changes. No breaking changes to public APIs except framework upgrade.

### Upgrade Notes

1. **Framework Upgrade**: Update your project to target .NET 10 or later
2. **NuGet Update**: Update package reference: `<PackageReference Include="Enduro.Lacrm" Version="2.0.0" />`
3. **Code Changes**: None required - existing code works as-is
4. **New Features**: Explore 22 new API functions now available

For detailed migration guidance, see [docs/articles/migration-v1-to-v2.md](docs/articles/migration-v1-to-v2.md)

---

## [1.0.1] - Original Release

### Initial Features
- Basic Contact API (Create, Get, Edit, Delete, Search)
- Basic Task API (Create)
- Basic Event API (Create)
- Basic Note API (Create)
- Pipeline API (Create, Update, GetAttached, GetReport, GetSettings)
- Group API (AddContactToGroup)
- Configuration API (GetUserInfo, GetCustomFields)
- Targeted .NET Standard 2.1

---

## Links
- [Full Documentation](docs/index.md)
- [API Reference](docs/api/)
- [Getting Started Guide](docs/articles/getting-started.md)
- [GitHub Repository](https://github.com/EnduroPipeline/Enduro-Lacrm)
