var shareItem = {
    page: 2,
    populateShareFriends: function (item) {
        var template = $('#shareTemplate').html();

        var templateScript = Handlebars.compile(template);

        var context = {
            "friendName": item.userName,
            "friendId": item.idUser,
        };

        var html = templateScript(context);
        $('#shareFriendsList').append(html);
    },
    getScrollPosition: function () {
        var s = $(window).scrollTop(),
            d = $(document).height(),
            c = $(window).height();

        return (s / (d - c)) * 100;
    },
}

$(document).scroll(function () {
    var scrollPosition = shareItem.getScrollPosition();
    if (scrollPosition > 60) {
        $.ajax({
            url: "/User/GetFriends",
            data: {
                currentUserId: $("#currentUserId").val(),
                currentPage: shareItem.page,
                searchStringFriends: $("#search-input-Friends")
            }
        }).done(function (data) {
            data.forEach(function (item) {
                shareItem.populateShareFriends(item);
            })
            shareItem.page++;
        })
    }
});

var shareContent = function () {
    var selectedFriends = [];
    $('input:checked').each(function () {
        selectedFriends.push($(this).data('username'));
    });

    var model = {
        Comment: $("#commentId").val(),
        IdItem: $("#itemId").val(),
        CurrentUserId: $("#currentUserId").val()
    };


    $.ajax({
        url: "/User/ShareContent",
        data: {
            jsonModel: JSON.stringify(model),
            jsonFriendsUserNames: JSON.stringify(selectedFriends)
        },
        success: function (data) {
            var prevPage = $('#fromPage').val();

            switch (prevPage) {
                case "workouts":
                    window.location.href = "/Trainer/TrainerWorkoutsList?currentId=" + $("#currentUserId").val();
                    break;
                case "recipes":
                    window.location.href = "/Nutritionist/NutritionistRecipesList";
                    break;
                case "savedItems":
                    window.location.href = "/User/SavedItems?userId=" + $("#currentUserId").val();
                    break;
                default:
                    window.location.href = "/Home/Index";
                    break;
            }
        }
    });
}