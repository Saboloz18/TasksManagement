using Quartz;
using TasksManagement.Application;
using TasksManagement.Infrastructure;
using TasksManagement.Persistence;
using TasksManagementReassignmentWorker;

var builder = Host.CreateApplicationBuilder(args);

// Register Options
var reassignmentOptions = builder.Configuration
    .GetSection("QuartzOptions")
    .Get<ReassignmentOptions>();


builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("TaskReassignmentJob");
    q.AddJob<TaskReassignmentJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
    .ForJob(jobKey)
    .WithIdentity("ImmediateTrigger")
    .StartNow());

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("RecurringTrigger")
        .WithCronSchedule(reassignmentOptions.CronSchedule, x =>
            x.InTimeZone(TimeZoneInfo.Local)));

});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddApplication();
builder.Services.AddPersistence(dbConnectionString: builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddInfrastructure();
builder.Services.AddTransient<TaskReassignmentJob>();


var app = builder.Build();
app.Run();
