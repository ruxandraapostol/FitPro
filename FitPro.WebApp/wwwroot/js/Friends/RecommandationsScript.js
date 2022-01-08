var friendsOn = function () {
    $('#friendsRecommandationsBtnOn').hide();
    $('#friendsRecommandationsBtnOff').show();
    $('#myRecommandationsBtnOn').show();
    $('#myRecommandationsBtnOff').hide();
};

var meOn = function () {
    $('#friendsRecommandationsBtnOn').show();
    $('#friendsRecommandationsBtnOff').hide();
    $('#myRecommandationsBtnOn').hide();
    $('#myRecommandationsBtnOff').show();
};

var myUtils = {
    me: false,
    currentPage: 2,
    getScrollPosition: function () {
        var s = $(window).scrollTop(),
            d = $(document).height(),
            c = $(window).height();

        return (s / (d - c)) * 100;
    },
    populateMe: function (item) {
        var template = $('#myRecommandationsTemplate').html();

        var templateScript = Handlebars.compile(template);

        var context = {
            "friendUserName": item.friendUserName,
            "comment": item.comment,
            "itemName": item.name,
            "itemId": item.itemId,
            "itemLink": item.link,
            "date": item.date,
            "isWorkout": item.isWorkout
        };

        var html = templateScript(context);
        $('#RecommandationList').append(html);
    },
    populateFriends: function (item) {
        var template = $('#friendRecommandationsTemplate').html();

        var templateScript = Handlebars.compile(template);

        var context = {
            "friendUserName": item.friendUserName,
            "comment": item.comment,
            "itemName": item.name,
            "itemId": item.itemId,
            "itemLink": item.link,
            "date": item.date,
            "isWorkout": item.isWorkout
        };

        var html = templateScript(context);
        $('#RecommandationList').append(html);
    }
};

var showMyRecommandation = function () {
    myUtils.currentPage = 1;

    $.ajax({
        url: '/User/GetMyRecommandations',
        data: {
            currentPage: myUtils.currentPage
        }
    }).done(function (data) {
        $('#RecommandationList').empty();

        data.forEach(function (item) {
            myUtils.populateMe(item);
        })
        myUtils.currentPage;
    });
};


var showFriendsRecommandation = function () {
    myUtils.currentPage = 1;

    $.ajax({
        url: '/User/GetFriendsRecommandations',
        data: {
            currentPage: myUtils.currentPage
        }
    }).done(function (data) {
        $('#RecommandationList').empty();

        data.forEach(function (item) {
            myUtils.populateFriends(item);
        })
        myUtils.currentPage;
    });
};



$(document).ready(function () {
    $('#friendsRecommandationsBtnOff').click(function () {
        myUtils.me = false;
        meOn();
        showFriendsRecommandation();
    });

    $('#myRecommandationsBtnOff').click(function () {
        myUtils.me = true;
        friendsOn();
        showMyRecommandation();
    });
});

$(document).scroll(function () {
    var scrollPosition = workouts.getScrollPosition();
    if (scrollPosition > 60) {

        if (myUtils.me) {
            $.ajax({
                url: '/User/GetMyRecommandations',
                data: {
                    currentPage: myUtils.currentPage
                }
            }).done(function (data) {
                data.forEach(function (item) {
                    myUtils.populateMe(item);
                })
                myUtils.currentPage;
            });
        } else {
            $.ajax({
                url: '/User/GetFriendsRecommandations',
                data: {
                    currentPage: myUtils.currentPage
                }
            }).done(function (data) {
                data.forEach(function (item) {
                    myUtils.populateFriends(item);
                })
                myUtils.currentPage;
            });
        }
    }
});

var detailWorkout = function (event) {
    var linkUrl = $(event.currentTarget).data("link");

    window.location.href = '/Trainer/DetailWorkout?workoutLink='
        + linkUrl + '&fromSavedItems=false&fromShare=true';
}

var detailRecipe = function (event) {
    var idRecipe = $(event.currentTarget).data("idrecipe");

    window.location.href = '/Nutritionist/DetailRecipe?idRecipe='
        + idRecipe + '&fromSavedItems=false&fromShare=true';
}


var friendDetail = function (event) {
    var userName = $(event.currentTarget).data("username");
    window.location.href = '/Account/Details?userName=' + userName;
}




