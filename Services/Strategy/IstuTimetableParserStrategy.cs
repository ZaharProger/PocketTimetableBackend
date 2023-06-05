using PocketTimetableBackend.Constants;
using PocketTimetableBackend.Models;
using PocketTimetableBackend.Models.Http;
using RestSharp;
using System.Text.RegularExpressions;

namespace PocketTimetableBackend.Services.Strategy
{
    public class IstuTimetableParserStrategy : IDataParserStrategy
    {
        private static readonly Dictionary<string, SubjectTypes> subjectTypesDict = new()
        {
            { ParserKeys.CANCELLATION, SubjectTypes.CANCELLATION },
            { ParserKeys.CONSULTATION, SubjectTypes.CONSULTATION },
            { ParserKeys.EXAM, SubjectTypes.EXAM },
            { "", SubjectTypes.IT_ACADEMY },
            { ParserKeys.LABORATORY, SubjectTypes.LABORATORY },
            { ParserKeys.LECTURE, SubjectTypes.LECTURE },
            { ParserKeys.PRACTICE, SubjectTypes.PRACTICE },
            { ParserKeys.PROJECT, SubjectTypes.PROJECT },
            { ParserKeys.PASS, SubjectTypes.PASS }
        };

        public BaseEntity[] Parse(string targetUri)
        {
            var restClientOptions = new RestClientOptions(targetUri);
            var restClient = new RestClient(restClientOptions);
            var rawData = restClient.Get(new RestRequest()).Content ?? "";

            var weekTypePattern = new Regex("<p>неделя: <b>(?'weekType'[а-яА-Я]+)");
            var parsedWeekType = weekTypePattern.Matches(rawData);

            var parsedItems = Array.Empty<BaseEntity>();

            if (parsedWeekType.Count != 0)
            {
                var weekType = parsedWeekType[0].Groups[ParserKeys.WEEK_TYPE].Value.Trim().ToLower();

                var extraBlocksPattern = new Regex("<div class=\"class-pred\"><span style=\"[\\w\\s:;.\\-#]+\"><img src=[\\w\\s\\/+=,;:\\-]+><b>");
                var daySubjectsPattern = new Regex("(<div class=\"class-line-item\">\r?\n<div class=\"class-tails\">\r?\n<div class=\"class-time\">(?'timeStart'[\\d:]+)<\\/div>\\n)*(<div class=\"class-tail class-(even|odd|all)-week\">[а-яА-Я\\s]*<\\/div>\\n)*<div class=\"class-tail class-(?'subjectWeek'even|odd|all)-week\">\\n<div class=\"class-info\">(?'subjectType'[а-яА-Я\\s]*)(<a href=\"\\?prep=\\d*\">(?'tutor'[а-яА-Я\\s.\\-]+)<\\/a>)*<\\/div>\\n<div class=\"class-pred\">(<b style=\"(?'style'[\\w\\s:;.\\-#]+)\">)*(?'name'[а-яА-Я\\s\\-\\w\\d|»«,():\"]+)(<\\/b>|<\\/b> <\\/span>)*<\\/div>\\n<div class=\"class-info\">(<a href=\"\\?group=\\d*\">[а-яА-Я\\d\\-]+<\\/a>[,\\s]*)*(?'subGroup'[а-яА-Я\\d\\s]*)<\\/div>\\n<div class=\"class-aud\"><a href=\"\\?aud=\\d*\">(?'classRoom'[а-яА-Я\\s\\-\\d.]+)<\\/a>(<\\/div>\\n)+");
                var dayPattern = new Regex("<h3 class=\"day-heading\">(?'day'[а-яА-Я\\d,\\s]+)");
                var parsedDays = dayPattern.Matches(rawData);

                parsedItems = new BaseEntity[parsedDays.Count];
                var subjectsData = dayPattern.Split(rawData);

                for (int i = 0; i < parsedItems.Length; ++i)
                {
                    var remainedSubjectsData = subjectsData
                        .SkipWhile(subjects => !subjects.Equals(parsedDays[i].Groups[ParserKeys.DAY].Value))
                        .ToArray();
                    var daySubjects = remainedSubjectsData.Length == 0 ? null : remainedSubjectsData[1];

                    if (daySubjects != null)
                    {
                        parsedItems[i] = new StudyDay()
                        {
                            Name = $"Неделя {weekType}",
                            Day = parsedDays[i].Groups[ParserKeys.DAY].Value.Trim(),
                            Subjects = GetSubjects(daySubjects, weekType, daySubjectsPattern, extraBlocksPattern)
                        };
                    }
                }
            }

            return parsedItems;
        }

        private Subject[] GetSubjects(string subjectsString, string weekType, Regex patternToFind, Regex patternToReplace)
        {
            var subjects = new List<Subject>();

            var replacedSubjectsString = patternToReplace.Replace(subjectsString, "<div class=\"class-pred\">");
            var parsedDaySubjects = patternToFind.Matches(replacedSubjectsString);
            var lastSubjectTime = "";

            for (int i = 0; i < parsedDaySubjects.Count; ++i)
            {
                var subjectsGroups = parsedDaySubjects[i].Groups;

                if (!subjectsGroups[ParserKeys.SUBJECT_TIME_START].Value.Equals(""))
                {
                    lastSubjectTime = subjectsGroups[ParserKeys.SUBJECT_TIME_START].Value.Trim();
                }

                var subjectWeek = subjectsGroups[ParserKeys.SUBJECT_WEEK].Value.Trim().ToLower();

                var correctWeek = subjectWeek.Equals("all") ||
                    (subjectWeek.Equals("odd") && weekType.Equals("четная")) ||
                    (subjectWeek.Equals("even") && weekType.Equals("нечетная"));
                var isChanged = subjectsGroups[ParserKeys.SUBJECT_NAME_STYLE].Value.Trim().ToLower().Contains("#f9f9f9");

                if (correctWeek && !isChanged)
                {
                    var subjectName = subjectsGroups[ParserKeys.SUBJECT_NAME].Value.Trim();

                    var subjectTypeKey = subjectName.ToLower().Contains(ParserKeys.PROJECT) ?
                        ParserKeys.PROJECT : subjectName.ToLower().Contains(ParserKeys.EXAM) ?
                        ParserKeys.EXAM : subjectName.ToLower().Contains(ParserKeys.CANCELLATION) ?
                        ParserKeys.CANCELLATION : subjectsGroups[ParserKeys.SUBJECT_TYPE].Value.Trim().ToLower();

                    subjects.Add(new Subject()
                    {
                        Name = subjectName,
                        SubGroupName = subjectsGroups[ParserKeys.SUBJECT_SUB_GROUP].Value.Trim(),
                        TimeStart = lastSubjectTime,
                        SubjectType = subjectTypesDict[subjectTypeKey],
                        Tutor = subjectsGroups[ParserKeys.SUBJECT_TUTOR].Value.Trim(),
                        Classroom = subjectsGroups[ParserKeys.SUBJECT_CLASSROOM].Value.Trim()
                    });
                }
            }

            return subjects.ToArray();
        }
    }
}
