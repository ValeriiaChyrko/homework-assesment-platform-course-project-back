using System.Security.Cryptography;
using System.Text.Json;
using HomeworkAssignment.Services.Abstractions;

public class CacheKeyManager : ICacheKeyManager
{
    // === CATEGORY ===
    public string CategoryListGroup() => "group:category:list";
    public string CategoryList() => "category:list";

    // === COURSE ===
    public string CourseListGroup(Guid userId) => $"group:course:list:{userId}";
    public string CourseList(Guid userId, object filter) =>
        $"course:list:{userId}:{ComputeHash(filter)}";
    
    public string CourseSingleGroup(Guid courseId) => $"group:course:{courseId}";
    public string CourseSingle(Guid courseId) => $"course:{courseId}";

    public string CourseOwned(Guid userId) => $"course:owned:{userId}";
    public string CourseAttachments(Guid courseId) => $"course:{courseId}:attachments";

    // === ENROLLMENT ===
    public string EnrollmentListGroup(Guid userId) => $"group:enrollment:list:{userId}";
    public string EnrollmentList(Guid userId) => $"enrollment:list:{userId}";
    public string Enrollment(Guid userId, Guid courseId) => $"enrollment:{userId}:{courseId}";

    // === CHAPTER ===
    public string ChapterSingleGroup(Guid courseId, Guid chapterId) =>
        $"group:course:{courseId}:chapter:{chapterId}";
    public string ChapterSingle(Guid courseId, Guid chapterId) =>
        $"course:{courseId}:chapter:{chapterId}";

    public string ChapterFirst(Guid courseId) => $"course:{courseId}:chapter:first";
    public string ChapterAttachments(Guid courseId, Guid chapterId) =>
        $"course:{courseId}:chapter:{chapterId}:attachments";

    // === PROGRESS ===
    public string ChapterProgress(Guid userId, Guid courseId, Guid chapterId) =>
        $"progress:{userId}:course:{courseId}:chapter:{chapterId}";
    
    // === ASSIGNMENT ===
    public string AssignmentSingleGroup(Guid courseId, Guid chapterId, Guid assignmentId) =>
        $"group:course:{courseId}:chapter:{chapterId}:assignment:{assignmentId}";
    
    public string AssignmentSingle(Guid courseId, Guid chapterId, Guid assignmentId) =>
        $"course:{courseId}:chapter:{chapterId}:assignment:{assignmentId}";

    public string AssignmentList(Guid courseId, Guid chapterId) =>
        $"course:{courseId}:chapter:{chapterId}:assignments";

    public string AssignmentProgress(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId) =>
        $"progress:{userId}:course:{courseId}:chapter:{chapterId}:assignment:{assignmentId}";

    // === ATTEMPT ===
    public string AttemptList(Guid courseId, Guid chapterId, Guid assignmentId) =>
        $"course:{courseId}:chapter:{chapterId}:assignment:{assignmentId}:attempts";

    // === HASHING ===
    private static string ComputeHash(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        var hashBytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(json));
        return Convert.ToBase64String(hashBytes)
            .Replace("/", "_")
            .Replace("+", "-")[..12];
    }
}