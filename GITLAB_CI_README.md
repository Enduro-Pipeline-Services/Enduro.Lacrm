# GitLab CI/CD Pipeline Configuration

## Overview
This GitLab CI/CD pipeline automatically builds, tests, packages, and deploys the Enduro.Lacrm NuGet package to the Enduro Nexus repository.

## Pipeline Stages

### 1. **Build** 
- Restores NuGet dependencies
- Compiles the project in Release configuration
- Runs on all pushes, merge requests, web triggers, and tags

### 2. **Test**
- Runs all xUnit tests with code coverage
- Generates coverage reports (Cobertura format)
- Creates JUnit test result artifacts
- Displays coverage percentage in GitLab UI
- Requires build stage to complete first

### 3. **Package**
- Creates NuGet package (.nupkg) from the project
- Stores package in artifacts for 1 week
- Requires both build and test stages to pass

### 4. **Deploy**
- Pushes NuGet package to Nexus repository
- **Only runs on default branch (master/develop) or tags**
- Uses secure environment variable for authentication

## Required GitLab CI/CD Variables

You must configure the following **protected** and **masked** variable in GitLab:

### `NUGET__API__KEY`
- **Type**: Variable
- **Value**: Your Nexus NuGet API key
- **Protected**: ✅ Yes (only available on protected branches)
- **Masked**: ✅ Yes (hidden in job logs)
- **Scope**: All environments

### How to Add the Variable:
1. Go to your GitLab project: **Settings → CI/CD → Variables**
2. Click **Add Variable**
3. Key: `NUGET__API__KEY`
4. Value: `[Your Nexus API Key]`
5. Check **Protect variable** ✅
6. Check **Mask variable** ✅
7. Click **Add variable**

## GitLab Runners

The pipeline uses runners with the following tags:
- `docker` - Indicates Docker-enabled runner
- `Tulsa` - Indicates physical location/runner group

Ensure your GitLab instance has runners configured with these tags.

## Deployment Behavior

### Automatic Deployment:
- ✅ Pushes to **master** branch → Deploys to Nexus
- ✅ Pushes to **develop** branch → Deploys to Nexus (if default branch)
- ✅ Git **tags** (e.g., v2.0.0) → Deploys to Nexus

### No Deployment:
- ❌ Merge requests → Package only, no deploy
- ❌ Feature branches → Package only, no deploy

## Coverage Reporting

The pipeline includes comprehensive test coverage tracking:
- **XPlat Code Coverage**: Cross-platform coverage collection
- **ReportGenerator**: Combines and formats coverage reports
- **Cobertura XML**: Standard format for GitLab integration
- **Coverage Badge**: Automatically displayed in GitLab UI

Coverage results appear in:
- Merge request widgets
- Pipeline job output
- GitLab badges (can be added to README)

## Artifacts

Each pipeline run produces the following artifacts:

### Test Artifacts (always saved):
- JUnit test results XML
- Cobertura coverage report
- Combined coverage summary

### Package Artifacts (saved on success, 1 week retention):
- `artifacts/nuget/*.nupkg` - NuGet package files

## Pipeline Rules

The pipeline uses GitLab's rules system to control when jobs run:

```yaml
rules:
  - if: '$CI_PIPELINE_SOURCE == "merge_request_event"'  # Run on MRs
  - if: '$CI_PIPELINE_SOURCE == "push"'                 # Run on pushes
  - if: '$CI_PIPELINE_SOURCE == "web"'                  # Run via UI
  - if: '$CI_COMMIT_TAG'                                # Run on tags
```

Deploy job has additional restrictions:
```yaml
rules:
  - if: $CI_COMMIT_BRANCH == $CI_DEFAULT_BRANCH  # Only default branch
  - if: '$CI_COMMIT_TAG'                          # Or tags
```

## Nexus Repository

**Target Repository**: `https://nexus.enduropls.com/repository/nuget-hosted`

This is the Enduro internal NuGet package repository. Packages pushed here are available to all Enduro projects.

## Version Management

Package versions are controlled by the `.csproj` file:
```xml
<Version>2.0.0</Version>
<PackageVersion>2.0.0</PackageVersion>
```

When creating a new release:
1. Update version in `Enduro.Lacrm.csproj`
2. Commit and push to develop
3. Merge to master via GitFlow
4. Tag the release (e.g., `v2.0.0`)
5. Push tag → Pipeline deploys automatically

## Troubleshooting

### "401 Unauthorized" during deploy:
- Check that `NUGET__API__KEY` variable is set correctly
- Verify the API key is valid in Nexus
- Ensure variable is marked as **Protected** (if deploying from protected branch)

### "Package already exists" error:
- NuGet packages are immutable in Nexus
- Increment version number in `.csproj`
- Cannot overwrite existing package versions

### Pipeline doesn't run:
- Check that runners with `docker` and `Tulsa` tags are available
- Verify `.gitlab-ci.yml` syntax using GitLab's CI Lint tool
- Ensure repository has CI/CD enabled

### Tests fail but pass locally:
- Check that all test dependencies are restored
- Verify .NET SDK version (pipeline uses 10.0)
- Review test artifacts in pipeline job output

## Manual Pipeline Trigger

You can manually trigger a pipeline:
1. Go to **CI/CD → Pipelines**
2. Click **Run Pipeline**
3. Select branch or tag
4. Click **Run Pipeline**

This is useful for:
- Re-deploying without new commits
- Testing pipeline changes
- Deploying specific versions

## Security Notes

🔒 **Never commit secrets to `.gitlab-ci.yml`**
- All sensitive values use environment variables
- `$NUGET__API__KEY` is masked in logs
- Protected variables only available on protected branches

✅ **Best Practices**:
- Keep API keys in GitLab CI/CD variables only
- Mark sensitive variables as **Protected** and **Masked**
- Rotate API keys periodically
- Limit deploy jobs to protected branches

## Next Steps

After setting up the pipeline:

1. ✅ Add `NUGET__API__KEY` to GitLab CI/CD variables
2. ✅ Push `.gitlab-ci.yml` to repository
3. ✅ Trigger pipeline manually or via push
4. ✅ Verify successful deploy to Nexus
5. ✅ Test consuming package from Nexus in another project

## Related Documentation

- [GitLab CI/CD Documentation](https://docs.gitlab.com/ee/ci/)
- [NuGet Package Management](https://docs.microsoft.com/en-us/nuget/)
- [Nexus Repository Manager](https://help.sonatype.com/repomanager3)
