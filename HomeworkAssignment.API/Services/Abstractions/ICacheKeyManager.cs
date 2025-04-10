namespace HomeworkAssignment.Services.Abstractions;

public interface ICacheKeyManager
{
    string CategoryListGroup();
    string CategoryList();
    string CourseList(Guid userId, object filter);
    string CourseListGroup(Guid userId);
    string CourseOwned(Guid userId);
    string CourseSingle(Guid courseId);
    string CourseSingleGroup(Guid courseId);
    string CourseAttachments(Guid courseId);
    string EnrollmentListGroup(Guid userId);
    string Enrollment(Guid userId, Guid courseId);
    string EnrollmentList(Guid userId);
    string ChapterSingleGroup(Guid courseId, Guid chapterId);
    string ChapterSingle(Guid courseId, Guid chapterId);
    string ChapterFirst(Guid courseId);
    string ChapterAttachments(Guid courseId, Guid chapterId);
    string ChapterProgress(Guid userId, Guid courseId, Guid chapterId);
    string AssignmentSingleGroup(Guid courseId, Guid chapterId, Guid assignmentId);
    string AssignmentSingle(Guid courseId, Guid chapterId, Guid assignmentId);
    string AssignmentList(Guid courseId, Guid chapterId);
    string AssignmentProgress(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId);
}