namespace HomeworkAssignment.Services.Abstractions;

public interface ICacheKeyManager
{
    string CategoryListGroup();
    string CategoryList();
    string CourseList(Guid userId, object filter);
    string CourseListGroup(Guid userId);
    string CourseOwned(Guid userId);
    string CourseSingle(Guid courseId);
    string CourseSingleStudent(Guid userId, Guid courseId);
    string CourseSingleGroup(Guid courseId);
    string CourseAttachments(Guid courseId);
    string EnrollmentListGroup(Guid userId);
    string Enrollment(Guid userId, Guid courseId);
    string EnrollmentsAnalytics(Guid userId);
    string ChapterSingleGroup(Guid courseId, Guid chapterId);
    string ChapterList(Guid courseId);
    string ChapterSingle(Guid courseId, Guid chapterId);
    string ChapterFirst(Guid courseId);
    string ChapterNext(Guid courseId, Guid chapterId);
    string ChapterAttachments(Guid courseId, Guid chapterId);
    string ChapterProgress(Guid userId, Guid courseId, Guid chapterId);
    string AssignmentSingleGroup(Guid courseId, Guid chapterId, Guid assignmentId);
    string AssignmentSingle(Guid courseId, Guid chapterId, Guid assignmentId);
    string AssignmentList(Guid courseId, Guid chapterId);
    string AssignmentAnalytics(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId);
    string AssignmentProgress(Guid userId, Guid courseId, Guid chapterId, Guid assignmentId);
    string AttemptList(Guid courseId, Guid chapterId, Guid assignmentId);
}