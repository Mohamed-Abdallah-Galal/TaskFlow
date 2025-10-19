using TaskFlow.DAL.DataBase;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Enums.Task;
using TaskFlow.DAL.Enums.User;

namespace TaskFlow.DAL.DataTemp
{
    public class DbSeeder
    {
        public static void Initialize(TaskFlowDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.appUsers.Any())
            {
                // Create users with different roles
                var superAdmin = new AppUser("System", "Administrator", "superadmin@taskflow.com", Role.SuperAdmin, 1);
                var admin = new AppUser("Sarah", "Johnson", "sarah.johnson@taskflow.com", Role.Admin, 1);
                var manager = new AppUser("Michael", "Chen", "michael.chen@taskflow.com", Role.Manager, 1);
                var seniorDev = new AppUser("John", "Smith", "john.smith@taskflow.com", Role.Employee, 1);
                var uiDesigner = new AppUser("Emma", "Wilson", "emma.wilson@taskflow.com", Role.Employee, 1);
                var qaEngineer = new AppUser("Lisa", "Rodriguez", "lisa.rodriguez@taskflow.com", Role.Employee, 1);
                var juniorDev = new AppUser("Mike", "Brown", "mike.brown@taskflow.com", Role.Employee, 1);

                context.appUsers.AddRange(superAdmin, admin, manager, seniorDev, uiDesigner, qaEngineer, juniorDev);
                context.SaveChanges();

                // Create diverse tasks with different categories and priorities
                var criticalBugTask = new TaskItem(
                    "Fix Critical Authentication Bug in Production",
                    "Users are unable to login after the latest deployment. The JWT token validation is failing for all requests. This is blocking all users from accessing the system.",
                    manager.Id,
                    seniorDev.Id,
                    TaskPriority.Critical,
                    TaskCategory.BugFix,
                    DateTime.Now.AddHours(6),
                    8.0m
                );

                var dashboardTask = new TaskItem(
                    "Implement User Analytics Dashboard",
                    "Create a comprehensive dashboard showing user activity metrics, task completion rates, team performance, and system usage statistics. Include charts for daily active users and task completion trends.",
                    admin.Id,
                    uiDesigner.Id,
                    TaskPriority.Medium,
                    TaskCategory.Feature,
                    DateTime.Now.AddDays(14),
                    40.0m
                );

                var apiDocsTask = new TaskItem(
                    "Update REST API Documentation for v2.0",
                    "Document all new endpoints added in the v2.0 release. Include request/response examples, error codes, authentication requirements, and rate limiting information for each endpoint.",
                    seniorDev.Id,
                    juniorDev.Id,
                    TaskPriority.Low,
                    TaskCategory.Documentation,
                    DateTime.Now.AddDays(30),
                    16.0m
                );

                var securityTask = new TaskItem(
                    "Implement Two-Factor Authentication System",
                    "Add 2FA support for all user accounts using time-based one-time passwords (TOTP). Support both SMS and authenticator apps. Include backup code generation and recovery options.",
                    admin.Id,
                    seniorDev.Id,
                    TaskPriority.High,
                    TaskCategory.Security,
                    DateTime.Now.AddDays(7),
                    24.0m
                );

                var performanceTask = new TaskItem(
                    "Optimize Database Query Performance",
                    "Identify and optimize slow-running queries in the task management module. Focus on the task filtering, reporting, and user assignment queries that are causing performance issues.",
                    manager.Id,
                    seniorDev.Id,
                    TaskPriority.High,
                    TaskCategory.Maintenance,
                    DateTime.Now.AddDays(5),
                    12.0m
                );

                var researchTask = new TaskItem(
                    "Research AI-Powered Task Assignment",
                    "Investigate machine learning algorithms for intelligent task assignment based on user skills, workload, and historical performance. Prepare a feasibility report with implementation recommendations.",
                    admin.Id,
                    manager.Id,
                    TaskPriority.Low,
                    TaskCategory.Research,
                    DateTime.Now.AddDays(21),
                    20.0m
                );

                context.taskItems.AddRange(criticalBugTask, dashboardTask, apiDocsTask, securityTask, performanceTask, researchTask);
                context.SaveChanges();

                // Simulate task progress and status changes
                criticalBugTask.UpdateStatus(AppTaskStatus.InProgress, seniorDev.Id, "Starting investigation");
                criticalBugTask.UpdateProgress(50, seniorDev.Id);
                criticalBugTask.LogTime(4.0m, seniorDev.Id);

                dashboardTask.UpdateStatus(AppTaskStatus.InProgress, uiDesigner.Id, "Starting design phase");
                dashboardTask.UpdateProgress(25, uiDesigner.Id);

                apiDocsTask.UpdateStatus(AppTaskStatus.InProgress, juniorDev.Id, "Documenting endpoints");
                apiDocsTask.UpdateProgress(70, juniorDev.Id);

                securityTask.UpdateStatus(AppTaskStatus.Blocked, seniorDev.Id, "Waiting for security library approval");
                securityTask.UpdateProgress(10, seniorDev.Id);

                performanceTask.UpdateStatus(AppTaskStatus.InReview, seniorDev.Id, "Query optimizations completed, ready for testing");
                performanceTask.UpdateProgress(100, seniorDev.Id);
                performanceTask.LogTime(10.5m, seniorDev.Id);

                context.SaveChanges();

                // Add realistic comments with different types
                var comments = new List<Comment>
                {
                    // Critical Bug Task Comments
                    new Comment(criticalBugTask.Id, manager.Id, "This is blocking production deployment. All hands on deck!", manager.Id),
                    new Comment(criticalBugTask.Id, seniorDev.Id, "Found the issue in JWT token validation. The secret key encoding was incorrect.", seniorDev.Id),
                    new Comment(criticalBugTask.Id, qaEngineer.Id, "Tested the fix in staging environment. Login working correctly now.", qaEngineer.Id),
                    
                    // Dashboard Task Comments
                    new Comment(dashboardTask.Id, uiDesigner.Id, "Need access to analytics database to design the charts and metrics display.", uiDesigner.Id),
                    new Comment(dashboardTask.Id, admin.Id, "Database credentials and access have been granted. Please proceed with the design.", admin.Id),
                    new Comment(dashboardTask.Id, uiDesigner.Id, "Initial design mockups ready for review. Please check the attachments.", uiDesigner.Id, true),
                    
                    // API Documentation Comments
                    new Comment(apiDocsTask.Id, juniorDev.Id, "Should I include code examples for each endpoint in multiple programming languages?", juniorDev.Id),
                    new Comment(apiDocsTask.Id, seniorDev.Id, "Yes, include examples in C#, JavaScript, and Python. Focus on the main use cases.", seniorDev.Id),
                    
                    // Security Task Comments
                    new Comment(securityTask.Id, admin.Id, "Make sure to follow OWASP guidelines for 2FA implementation. Security is critical here.", admin.Id),
                    new Comment(securityTask.Id, seniorDev.Id, "Using Google Authenticator compatible TOTP implementation. Testing with Authy and Microsoft Authenticator.", seniorDev.Id),
                    new Comment(securityTask.Id, admin.Id, "Security library has been approved. You can proceed with implementation.", admin.Id),
                    
                    // Performance Task Comments
                    new Comment(performanceTask.Id, seniorDev.Id, "Identified the slow queries. Adding proper indexes and optimizing JOIN statements.", seniorDev.Id),
                    new Comment(performanceTask.Id, qaEngineer.Id, "Performance tests show 60% improvement in query response times. Great work!", qaEngineer.Id),
                    
                    // Internal notes
                    new Comment(researchTask.Id, manager.Id, "Internal discussion: Consider budget for ML services before proceeding.", manager.Id, true),
                    new Comment(researchTask.Id, admin.Id, "Budget approved for POC. Maximum $5k for initial research.", admin.Id, true)
                };

                context.comments.AddRange(comments);
                context.SaveChanges();

                // Add diverse attachments with different types
                var attachments = new List<Attachment>
                {
                    new Attachment(criticalBugTask.Id, "error_logs.txt", "/attachments/error_logs_123.txt", "text/plain", AttachmentType.Log, 1024, seniorDev.Id),
                    new Attachment(criticalBugTask.Id, "auth_error_screenshot.png", "/attachments/auth_error.png", "image/png", AttachmentType.Image, 204800, manager.Id),
                    new Attachment(dashboardTask.Id, "dashboard_mockup.fig", "/attachments/dashboard_design.fig", "application/fig", AttachmentType.Design, 512000, uiDesigner.Id),
                    new Attachment(dashboardTask.Id, "analytics_requirements.pdf", "/attachments/requirements.pdf", "application/pdf", AttachmentType.Document, 153600, admin.Id),
                    new Attachment(securityTask.Id, "2fa_specification.docx", "/attachments/2fa_spec.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", AttachmentType.Document, 256000, admin.Id),
                    new Attachment(performanceTask.Id, "query_analysis.sql", "/attachments/slow_queries.sql", "text/x-sql", AttachmentType.Code, 5120, seniorDev.Id),
                    new Attachment(performanceTask.Id, "performance_report.pdf", "/attachments/performance_improvement.pdf", "application/pdf", AttachmentType.Document, 102400, qaEngineer.Id),
                    new Attachment(researchTask.Id, "ml_research_paper.pdf", "/attachments/ai_task_assignment.pdf", "application/pdf", AttachmentType.Document, 307200, manager.Id)
                };

                context.attachments.AddRange(attachments);
                context.SaveChanges();

                // Add comprehensive task history showing full workflow
                var taskHistories = new List<TaskHistory>
                {
                    // Critical Bug Task - Emergency workflow
                    new TaskHistory(criticalBugTask.Id, manager.Id, (int)AppTaskStatus.Pending, (int)AppTaskStatus.InProgress, "Emergency - Starting immediate investigation", HistoryActionType.StatusChange),
                    new TaskHistory(criticalBugTask.Id, seniorDev.Id, (int)AppTaskStatus.InProgress, (int)AppTaskStatus.InReview, "Fix implemented and deployed to staging", HistoryActionType.StatusChange),
                    
                    // Dashboard Task - Normal feature workflow
                    new TaskHistory(dashboardTask.Id, admin.Id, (int)AppTaskStatus.Pending, (int)AppTaskStatus.InProgress, "Design phase started", HistoryActionType.StatusChange),
                    new TaskHistory(dashboardTask.Id, uiDesigner.Id, (int)AppTaskStatus.InProgress, (int)AppTaskStatus.InReview, "Design mockups completed and ready for review", HistoryActionType.StatusChange),
                    
                    // API Documentation Task
                    new TaskHistory(apiDocsTask.Id, seniorDev.Id, (int)AppTaskStatus.Pending, (int)AppTaskStatus.InProgress, "Documentation started", HistoryActionType.StatusChange),
                    new TaskHistory(apiDocsTask.Id, juniorDev.Id, (int)AppTaskStatus.InProgress, (int)AppTaskStatus.InReview, "Initial draft completed", HistoryActionType.StatusChange),
                    
                    // Security Task - Blocked workflow
                    new TaskHistory(securityTask.Id, admin.Id, (int)AppTaskStatus.Pending, (int)AppTaskStatus.InProgress, "Implementation started", HistoryActionType.StatusChange),
                    new TaskHistory(securityTask.Id, seniorDev.Id, (int)AppTaskStatus.InProgress, (int)AppTaskStatus.Blocked, "Waiting for security library approval from compliance team", HistoryActionType.StatusChange),
                    
                    // Performance Task - Completed workflow
                    new TaskHistory(performanceTask.Id, manager.Id, (int)AppTaskStatus.Pending, (int)AppTaskStatus.InProgress, "Performance analysis started", HistoryActionType.StatusChange),
                    new TaskHistory(performanceTask.Id, seniorDev.Id, (int)AppTaskStatus.InProgress, (int)AppTaskStatus.InReview, "Optimizations completed and ready for QA", HistoryActionType.StatusChange),
                    new TaskHistory(performanceTask.Id, qaEngineer.Id, (int)AppTaskStatus.InReview, (int)AppTaskStatus.Completed, "Performance improvements verified and deployed", HistoryActionType.StatusChange),
                    
                    // Assignment changes
                    new TaskHistory(dashboardTask.Id, admin.Id, 0, 0, "Reassigned from John to Emma", HistoryActionType.AssignmentChange),
                    new TaskHistory(securityTask.Id, manager.Id, 0, 0, "Priority escalated from High to Critical", HistoryActionType.PriorityChange)
                };

                context.taskHistories.AddRange(taskHistories);
                context.SaveChanges();

                // Test soft delete functionality
                var deletedComment = new Comment(criticalBugTask.Id, juniorDev.Id, "This comment will be soft deleted for testing purposes.", juniorDev.Id);
                context.comments.Add(deletedComment);
                context.SaveChanges();
                deletedComment.Delete(admin.Id);
                context.SaveChanges();

                // Test user deactivation
                var inactiveUser = new AppUser("Inactive", "User", "inactive@taskflow.com", Role.Employee, 1);
                context.appUsers.Add(inactiveUser);
                context.SaveChanges();
                inactiveUser.Deactivate(admin.Id);
                context.SaveChanges();

                Console.WriteLine("🎉 Database seeded successfully with enhanced data!");
                Console.WriteLine($"📊 Created: {context.appUsers.Count()} users, {context.taskItems.Count()} tasks, " +
                                $"{context.comments.Count()} comments, {context.attachments.Count()} attachments, " +
                                $"{context.taskHistories.Count()} history records");
            }
        }
    }
}