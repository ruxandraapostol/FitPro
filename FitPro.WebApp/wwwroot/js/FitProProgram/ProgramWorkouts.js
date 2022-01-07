var dayRow = $('#dayRow').val();

var workouts = {
    page: 2,
    ticking: false,
    populateWorkouts: function (item) {
        var template = $('#ProgramWorkoutTemplate').html();

        var templateScript = Handlebars.compile(template);

        var context = {
            "linkUrl": item.linkUrl,
            "name": item.name,
            "programId": $("#programId").val(),
            "userId": $("#currentUserId").val()
        };

        var html = templateScript(context);


        if (item.day > dayRow) {
            dayRow = item.day;

            if (dayRow % 4 == 1) {
                var restDay = document.createElement('p');
                restDay.innerText = 'Day ' + (item.day - 1);
                $('#ProgramWorkouContainer').append(restDay);

                var line = document.createElement('hr');
                line.style.width = "100%";
                $('#ProgramWorkouContainer').append(line);


                var restIcon = document.createElement('i');
                restIcon.style.fontSize = "100px";
                restIcon.className = "fas fa-bed";
                $('#ProgramWorkouContainer').append(restIcon);

            }


            var dayDiv = document.createElement('p');
            dayDiv.innerText = 'Day ' + item.day;

            if (item.day < item.currentDay) {
                dayDiv.style.color = '#e5e5e5';
            }
            $('#ProgramWorkouContainer').append(dayDiv);

            var line = document.createElement('hr');
            line.style.width = "100%";
            $('#ProgramWorkouContainer').append(line);
        }


        $('#ProgramWorkouContainer').append(html);
    },
    getScrollPosition: function () {
        var s = $(window).scrollTop(),
            d = $(document).height(),
            c = $(window).height();
        return (s / (d - c)) * 100;
    },
};

$(document).scroll(function () {
    var scrollPosition = workouts.getScrollPosition();
    if (scrollPosition > 60) {
        $.ajax({
            url: "/User/GetProgramWorkouts",
            data: {
                programId: $("#programId").val(),
                currentPage: workouts.page
            }
        }).done(function (data) {
            data.forEach(function (item) {
                workouts.populateWorkouts(item);
            })
            workouts.page++;
        })
    }
});

var detailWorkout = function (event) {
    var linkUrl = $(event.currentTarget).data("link");

    window.location.href = '/Trainer/DetailWorkout?workoutLink=' + linkUrl
        + '&programId=' + $("#programId").val();
}

