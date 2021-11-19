var possibleFriends = {
    page: 1,
    getPossibleFriends: function (item) {
        var template = $('#possibleFriendTemplate').html();

        var templateScript = Handlebars.compile(template);

        var context = { "idUser": item.idUser, "status": item.status, "userName": item.userName };

        var html = templateScript(context);

        $('.possibleFriendsList').append(html);
    }
}

var requestedFriends = {
    page: 1,
    possibleNextPage: true,
    getFriendshipRequests: function (item) {
        var template = $('#friendRequestsTemplate').html();

        var templateScript = Handlebars.compile(template);

        var context = { "idUser": item.idUser, "userName": item.userName };

        var html = templateScript(context);

        $('.friendshipRequestList').append(html);
    }
}

var friends = {
    page: 1,
    possibleNextPage: true,
    getFriends: function (item) {
        var template = $('#friendsTemplate').html();

        var templateScript = Handlebars.compile(template);

        var context = { "idUser": item.idUser, "userName": item.userName, "userStreak": item.streak };

        var html = templateScript(context);

        $('#friendsList').append(html);
    }
}

var idCurrentUser = $('#currentUserId').val();

$(document).ready(function () {

    $('#search-button-PossibleFriends').click(function () {
        var searchString = $('#search-input-PossibleFriends').val();

        $.ajax({
            url: '/User/GetPossibleFriends',
            data: {
                currentUserId: idCurrentUser,
                currentPage: possibleFriends.page,
                searchStringPossibleFriends: searchString
            }
        }).done(function (data) {
            $('.possibleFriendsList').empty();


            if (Array.isArray(data)) {
                if (data.length < 5) {
                    possibleFriends.possibleNextPage = false;
                }

                data.forEach(function (item) {
                    possibleFriends.getPossibleFriends(item);
                })

                var buttonSection = document.createElement('div')
                buttonSection.className = 'button-status-container';

                if (possibleFriends.page > 1) {
                    prevBtn = document.createElement('div');
                    prevBtn.id = "possibleFriendPrev";
                    prevBtn.innerHTML = '<i class="fas fa-chevron-left"></i>';
                    buttonSection.appendChild(prevBtn);
                }

                if (possibleFriends.possibleNextPage) {
                    nextBtn = document.createElement('div');
                    nextBtn.id = "possibleFriendNext";
                    nextBtn.innerHTML = '<i class="fas fa-chevron-right"></i>';
                    buttonSection.appendChild(nextBtn);
                }

                $('.possibleFriendsList').append(buttonSection);
            }
        });

        $("#possibleFriendPrev").click(function () {
            possibleFriends.page--;
            $('#search-button-PossibleFriends').click();
        })

        $("#possibleFriendNext").click(function () {
            possibleFriends.page++;
            $('#search-button-PossibleFriends').click();
        })
    });

    $('#search-button-Friends').click(function () {
        var searchString = $('#search-input-Friends').val();

        $.ajax({
            url: '/User/GetFriends',
            data: {
                currentUserId: idCurrentUser,
                currentPage: friends.page,
                searchStringFriends: searchString
            }
        }).done(function (data) {
            $('#friendsList').empty();
            $('#friendNext').remove();


            if (Array.isArray(data)) {
                if (data.length < 10) {
                    friends.possibleNextPage = false;
                }

                data.forEach(function (item) {
                    friends.getFriends(item);
                })

                var buttonSection = document.createElement('div')
                buttonSection.className = 'button-status-container';

                if (possibleFriends.page > 1) {
                    prevBtn = document.createElement('div');
                    prevBtn.id = "friendPrev";
                    prevBtn.innerHTML = '<i class="fas fa-chevron-left"></i>';
                    buttonSection.appendChild(prevBtn);
                }

                if (possibleFriends.possibleNextPage) {
                    nextBtn = document.createElement('div');
                    nextBtn.id = "friendNext";
                    nextBtn.innerHTML = '<i class="fas fa-chevron-right"></i>';
                    buttonSection.appendChild(nextBtn);
                }

                $('.friendsContainer').append(buttonSection);
            }
        });

        $("#friendPrev").click(function () {
            friends.page--;
            $('#search-button-friends').click();
        })

        $("#friendNext").click(function () {
            friends.page++;
            $('#search-button-friends').click();
        })
    });

    $('#button-FriendshipRequest').click(function () {
        $.ajax({
            url: '/User/GetFriendRequestsList',
            data: {
                currentUserId: idCurrentUser,
                currentPage: requestedFriends.page
            }
        }).done(function (data) {
            $('.friendshipRequestList').empty();

            if (!Array.isArray(data) || data.length == 0) {
                var centerdiv = document.createElement('div');
                centerdiv.className = 'iconCenterContainer';
                var icon = document.createElement('div');
                icon.innerHTML = '<p>No friendship request</p>';
                icon.innerHTML += '<i class="fas fa-sad-tear" style="font-size: 20px"></i>';
                centerdiv.appendChild(icon);
                $('.friendshipRequestList').append(centerdiv);
            }

            if (Array.isArray(data)) {
                if (data.length < 2) {
                    requestedFriends.possibleNextPage = false;
                }

                data.forEach(function (item) {
                    requestedFriends.getFriendshipRequests(item);
                })

                var buttonSection = document.createElement('div');
                buttonSection.className = 'button-status-container';

                if (requestedFriends.page > 1) {
                    prevBtn = document.createElement('div');
                    prevBtn.id = "requestedFriendPrev";
                    prevBtn.innerHTML = '<i class="fas fa-chevron-left"></i>';
                    buttonSection.appendChild(prevBtn);
                }

                if (requestedFriends.possibleNextPage) {
                    nextBtn = document.createElement('div');
                    nextBtn.id = "requestedFriendNext";
                    nextBtn.innerHTML = '<i class="fas fa-chevron-right"></i>';
                    buttonSection.appendChild(nextBtn);
                }

                $('.friendshipRequestList').append(buttonSection);
            }
        });

        $("#requestedFriendPrev").click(function () {
            requestedFriends.page--;
            $('#button-FriendshipRequest').click();
        })

        $("#requestedFriendNext").click(function () {
            requestedFriends.page++;
            $('#button-FriendshipRequest').click();
        })
    });

    $('#button-FriendshipRequest').click();

});

var friendsAction = function (event) {
    var idUser = $(event.currentTarget).data("id");
    var action = $(event.currentTarget).data("action");
    var url = '/User/' + action;

    $.ajax({
        url: url,
        data: {
            currentUserId: idCurrentUser,
            idUser: idUser
        }
    }).done(function () {
        $('#button-FriendshipRequest').click();
        $('#search-button-Friends').click();
        $('#search-button-PossibleFriends').click();
    });
}

var friendDetail = function (event) {
    var userName = $(event.currentTarget).data("username");
    window.location.href = '/Account/Details?userName=' + userName;
}