$(function () {
    reloadCalendar();

    $("form[name=datePeriod]")
        .on("change", "[name=startDate]", function () {
            var form = $(this).closest("form");
            var start = moment($(this).val()).startOf("week");
            var end = start.clone().endOf("week");

            $("[name=startDate]", form)
                .val(start.format("YYYY-MM-DD"));
            $("[name=endDate]", form)
                .val(end.format("YYYY-MM-DD"));

            reloadCalendar();
        });
});

function reloadCalendar() {
    updateCalendarHeader();
    assignCalendarDates();
}

function weekOfMonth(momentInput) {
    var prefixes = [1, 2, 3, 4, 5];

    return prefixes[0 | moment(momentInput).date() / 7];
}

function weekOfMonthWithOFfset(momentInput) {
    var firstDayOfMonth = momentInput.clone().startOf("month");
    var firstDayOfWeek = firstDayOfMonth.clone().startOf("week");
    var offset = firstDayOfMonth.diff(firstDayOfWeek, "days");

    return Math.ceil((momentInput.date() + offset) / 7);
}

function updateCalendarHeader() {
    var form = $("form[name=datePeriod]");
    var weekStart = moment($("[name=startDate]", form).val());
    var weekEnd = moment($("[name=endDate]", form).val());
    var calendar = $("#weeklyCalendar");

    var yearNumber = weekStart.year();
    var monthNumber = weekStart.month() + 1;
    var weekNumber = weekOfMonth(weekStart);

    $(".month-and-week", calendar)
        .text(`${monthNumber}월 ${FmtOrdinal.toOrdinal(weekNumber, 2, "째")} 주`);
    $(".year", calendar)
        .text(yearNumber);
}

function assignCalendarDates() {
    var calendar = $("#weeklyCalendar");
    var table = $(".hm-calendar-body", calendar);
    var form = $("form[name=datePeriod]");
    var weekStart = moment($("[name=startDate]", form).val());
    var weekEnd = moment($("[name=endDate]", form).val());

    $("thead th", table)
        .each(function (idx) {
            $(".hm-calendar-date", this)
                .text(
                    weekStart
                        .clone()
                        .day(idx)
                        .format("M/D")
                );
        });
}