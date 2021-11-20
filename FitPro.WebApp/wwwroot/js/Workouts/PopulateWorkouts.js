﻿var currentUserId = $('#CurrentUserId').val();
var currentUserRole = $('#CurrentUserRole').val();



var workouts = {
    page: 1,
    PossibleNextPage: true,
    getWorkouts: function (item) {
        var template = $('#workoutsTemplate').html();

        var templateScript = Handlebars.compile(template);


        var context = {
            "idUser": currentUserId,
            "role": currentUserRole,
            "name": item.name,
            "linkUrl": item.linkUrl,
            "idWorkout": item.idWorkout,
            "isSaved": item.isSaved
        };

        var html = templateScript(context);

        $('#workoutsList').append(html);

        var savediv = '#save_' + item.idWorkout;
        var unsavediv = '#unsave_' + item.idWorkout;

        if (item.isSaved) {
            $(savediv).hide();
            $(unsavediv).show();
        } else {
            $(savediv).show();
            $(unsavediv).hide();
        }
    },
    getScrollPosition: function () {
        var s = $(window).scrollTop(),
            d = $(document).height(),
            c = $(window).height();

        return (s / (d - c)) * 100;
    },
}

var filters = {
    hide: true
}

var getFilters = function () {
    return {
        SortColumn: $('#select-sort-W').val().split(" ")[0],
        SortColumnIndex: $('#select-sort-W').val().split(" ")[1],
        SearchString: $('#search-input-W').val(),
        LowerTimeLimit: $('#select-time').val().split(" ")[0],
        UpperTimeLimit: $('#select-time').val().split(" ")[1],
        LowerCaloriesLimit: $('#select-calories').val().split(" ")[0],
        UpperCaloriesLimit: $('#select-calories').val().split(" ")[1],
        SelectedCategories: $('#select-categories-W').val(),
        SelectedTrainers: $('#select-trainers-W').val()
    };
}

$(document).ready(function () {
    $('#select-categories-W').select2({ multiple: true });
    $('#select-trainers-W').select2({ multiple: true });

    $('#addFilter').click(function () {
        if (filters.hide) {
            filters.hide = false;
            $('.WorkoutFilters').show();
        } else {
            filters.hide = true;
            $('.WorkoutFilters').hide();
        }
    });

    $('#filterNow').click(function () {
        $('.WorkoutFilters').hide();


        workouts.page = 1;

        $.ajax({
            url: '/Trainer/GetWorkoutsList',
            data: {
                currentUserId: currentUserId,
                currentPage: workouts.page,
                FilterJsonString: JSON.stringify(getFilters()),
            },
        }).done(function (data) {
            $('#workoutsList').empty();

            if (Array.isArray(data)) {
                data.forEach(function (item) {
                    workouts.getWorkouts(item);
                });
            }
        });
    });
})


$(document).scroll(function () {
    var scrollPosition = workouts.getScrollPosition();
    if (scrollPosition > 60) {
        workouts.page++;
        $.ajax({
            url: "/Trainer/GetWorkoutsList",
            data: {
                currentUserId: currentUserId,
                currentPage: workouts.page,
                FilterJsonString: JSON.stringify(getFilters()),
            }
        }).done(function (data) {
            if (Array.isArray(data)) {
                data.forEach(function (item) {
                    workouts.getWorkouts(item);
                });
            }

        })
    }
});

var detailWorkout = function (event) {
    var linkUrl = $(event.currentTarget).data("link");

    $.ajax({
        url: '/Trainer/DetailWorkout',
        data: {
            workoutLink: linkUrl
        },
        success: function(data){
            window.location.href = '/Trainer/DetailWorkout?workoutLink=' + linkUrl;
        }
    });
}

var editWorkout = function (event) {
    var linkUrl = $(event.currentTarget).data("link");

    $.ajax({
        url: '/Trainer/EditWorkout',
        data: {
            workoutLink: linkUrl
        },
        success: function (data) {
            window.location.href = '/Trainer/EditWorkout?workoutLink=' + linkUrl;
        }
    });
}

var deleteWorkout = function (event) {
    var linkUrl = $(event.currentTarget).data("link");
    var userId = $(event.currentTarget).data("id")

    $.ajax({
        url: '/Trainer/DeleteWorkout',
        data: {
            workoutLink: linkUrl,
            currentId: userId
        },
        success: function (data) {
            window.location.href = '/Trainer/DeleteWorkout?workoutLink='
                + linkUrl + '&currentId=' + userId;
        }
    });
}

var saveWorkout = function (event) {
    var workoutId = $(event.currentTarget).data("idworkout");
    var userId = $(event.currentTarget).data("iduser")

    var savediv = '#save_' + workoutId;
    var unsavediv = '#unsave_' + workoutId;

    $(savediv).hide();
    $(unsavediv).show();

    $.ajax({
        url: '/User/SaveItem',
        data: {
            currentUserId: userId,
            itemId: workoutId,
        },
    });
}

var unsaveWorkout = function (event) {
    var workoutId = $(event.currentTarget).data("idworkout");
    var userId = $(event.currentTarget).data("iduser")

    var savediv = '#save_' + workoutId;
    var unsavediv = '#unsave_' + workoutId;

    $(savediv).show();
    $(unsavediv).hide();

    $.ajax({
        url: '/User/UnsaveItem',
        data: {
            currentUserId: userId,
            itemId: workoutId,
        },
    });
}

var shareWorkout = function (event) {
    var workoutId = $(event.currentTarget).data("idworkout");
    var userId = $(event.currentTarget).data("iduser");

    window.location.href = '/User/RecommandItem?currentUserId=' + userId
        + '&itemId=' + workoutId + '&fromPage=workouts';
}